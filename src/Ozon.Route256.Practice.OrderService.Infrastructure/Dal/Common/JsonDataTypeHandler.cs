using Dapper;
using System.Data;
using System.Text.Json;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.Dal.Common;

internal class JsonDataTypeHandler<T> : SqlMapper.TypeHandler<T>
{
    private readonly JsonSerializerOptions options = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    public override void SetValue(IDbDataParameter parameter, T value)
    {
        parameter.Value = JsonSerializer.Serialize(value, options);
    }

    public override T Parse(object value)
    {
        if (value is string json)
        {
            try
            {
                return JsonSerializer.Deserialize<T>(json, options);
            }
            catch (Exception)
            {
                return default;
            }
        }

        return default;
    }
}
