select top 100 *
from sys.objects
where modify_date > '20190227'
order by modify_date desc