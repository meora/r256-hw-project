using Dapper;
using System.Data;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.Dal.Common;

internal class DateTimeHandler : SqlMapper.TypeHandler<DateTime>
{
    public override void SetValue(IDbDataParameter parameter, DateTime value)
    {
        parameter.Value = value.ToUniversalTime();
    }

    public override DateTime Parse(object value)
    {
        return DateTime.SpecifyKind(((DateTime)value).ToUniversalTime(), DateTimeKind.Utc);
    }
}
