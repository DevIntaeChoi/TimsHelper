select top 100 g.SettleStatus, c.goodsName, g.*
from tbl_Goods_Master g
	left outer join tbl_Goods_Code c on g.GoodsCode = c.GoodsCode
	left outer join (
		select GoodsCode, sum(BookingAmt) - sum(CancelAmt) as Amt
		from tbl_Goods_Account_Detail
		group by GoodsCode
	) d on g.GoodsCode = d.GoodsCode and d.Amt > 0
	left outer join tbl_SettleReport_Master rm on g.GoodsCode = rm.GoodsCode and rm.CancelOrNot = 'N'
where g.StartDate > '20170101' and g.EndDate < '20170501'
and g.GoodsCode in ( select GoodsCode from tbl_Goods_Account_Detail )
and g.SettleStatus in ('57001', '57002')
and g.goodscode in (select GoodsCode from tbl_Settle_TermInfo where PayAccNo <> '' )
and g.OnOff <> 'C'
and g.BizCode <> '19303'
and d.GoodsCode is not null
--and g.PaymentFlag &lt;%gt; '66001'		-- 기타상품조건
--and g.SettleMethod = '59004'		-- 상시상품
and rm.GoodsCode is null