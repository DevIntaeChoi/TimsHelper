-- SP ???? ?˻?1
SELECT ROUTINE_NAME
        FROM INFORMATION_SCHEMA.ROUTINES
    WHERE ROUTINE_DEFINITION LIKE '%into tbl_Goods_PayRate%'
        AND ROUTINE_TYPE='PROCEDURE'
        order by ROUTINE_NAME
