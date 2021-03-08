
DECLARE @aSubject VARCHAR(100), @aBody VARCHAR(1000)

--알림메일 발송
SET @aSubject = 'test'
SET @aBody = 'test content'

EXEC DBMAIL.msdb.dbo.sp_send_dbmail
@profile_name = 'SMTP21',
@recipients = 'kdy2509@naver.com;',
@body_format ='HTML',
@subject =  @aSubject,
@body = @aBody
