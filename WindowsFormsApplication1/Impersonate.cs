using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;
using System.Runtime.InteropServices;

namespace WindowsFormsApplication1
{
    public static class Impersonate
    {
        public static WindowsImpersonationContext oImpersonatedUser;

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool LogonUser
            (string sUsername, string sDomain, string sPassword,
            int iLogonType, int iLogonProvider, ref IntPtr oToken);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private unsafe static extern int FormatMessage
            (int iFlags, ref IntPtr oSource, int iMessageId,
            int iLanguageId, ref String sBuffer, int iSize, IntPtr* Arguments);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool CloseHandle(IntPtr oHandle);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool DuplicateToken
            (IntPtr oExistingTokenHandle, int SECURITY_IMPERSONATION_LEVEL,
            ref IntPtr oDuplicateTokenHandle);

        // Log On Types
        const int LOGON32_LOGON_INTERACTIVE = 2;
        const int LOGON32_LOGON_NETWORK = 3;
        const int LOGON32_LOGON_NEW_CREDENTIALS = 9;

        // Log On Providers
        const int LOGON32_PROVIDER_DEFAULT = 0;
        const int LOGON32_PROVIDER_WINNT35 = 1;
        const int LOGON32_PROVIDER_WINNT40 = 2;
        const int LOGON32_PROVIDER_WINNT50 = 3;

        const int FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x00000100;
        const int FORMAT_MESSAGE_IGNORE_INSERTS = 0x00000200;
        const int FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000;

        /// <summary>
        /// Performs the Impersonation
        /// </summary>
        /// <param name="sServer">The machine you want to log in</param>
        /// <param name="sUserName">The username in that machine</param>
        /// <param name="sPassword">The password for the username</param>
        public static void ImpersonateNow(string sServer, string sUserName, string sPassword)
        {
            string sIP = sServer;

            IntPtr oTokenPointer = IntPtr.Zero;
            IntPtr oDuplicateToken = IntPtr.Zero;

            bool isSuccess = LogonUser(sUserName, sIP, sPassword,
               LOGON32_LOGON_NEW_CREDENTIALS, LOGON32_PROVIDER_DEFAULT, ref oTokenPointer);
            if (!isSuccess) { RaiseLastError(); }

            isSuccess = DuplicateToken(oTokenPointer, 2, ref oDuplicateToken);
            if (!isSuccess) { RaiseLastError(); }

            WindowsIdentity oNewIdentity = new WindowsIdentity(oDuplicateToken);
            oImpersonatedUser = oNewIdentity.Impersonate();
        }

        /// <summary>
        /// Terminated the Impersonation
        /// </summary>
        public static void ImpersonateEnd()
        {
            oImpersonatedUser.Undo();
        }

        /// <summary>
        /// Raises the last error on the Marshal object
        /// </summary>
        private static void RaiseLastError()
        {
            int iErrorCode = Marshal.GetLastWin32Error();
            string sErrorMessage = GetErrorMessage(iErrorCode);

            throw new ApplicationException(sErrorMessage);
        }

        /// <summary>
        /// Returns the readable error message
        /// </summary>
        /// <param name="iErrorCode">Error code thrown by the Marshall</param>
        /// <returns></returns>
        public unsafe static string GetErrorMessage(int iErrorCode)
        {
            int iMessageSize = 255; string sMessageBuffer = "";
            int iFlags = FORMAT_MESSAGE_ALLOCATE_BUFFER |
                   FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS;

            IntPtr oSourcePointer = IntPtr.Zero;
            IntPtr oArgumentsPointer = IntPtr.Zero;

            int iReturnValue = FormatMessage(iFlags, ref oSourcePointer,
               iErrorCode, 0, ref sMessageBuffer, iMessageSize, &oArgumentsPointer);
            if (iReturnValue == 0)
            {
                throw new ApplicationException(string.Format
                   ("Format message failed with error code '{0}'.", iErrorCode));
            }

            return sMessageBuffer;
        }
    }
}
