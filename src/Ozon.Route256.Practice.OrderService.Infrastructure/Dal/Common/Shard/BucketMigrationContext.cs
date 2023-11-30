namespace Ozon.Route256.Practice.OrderService.Infrastructure.Dal.Common.Shard;

internal class BucketMigrationContext
{
    private string _currentSchema = string.Empty;

    public void UpdateCurrentSchema(
        uint bucket) => _currentSchema = Shards.GetSchemaName(bucket);

    public string CurrentSchema
    {
        get
        {
            if (string.IsNullOrWhiteSpace(_currentSchema))
                throw new InvalidOperationException("Current db schema is not found");

            return _currentSchema;
        }
    }
}