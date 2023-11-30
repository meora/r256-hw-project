using Npgsql;
using System.Data;
using System.Data.Common;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.Dal.Common.Shard;

internal class ShardNpgsqlConnection : DbConnection
{
    private readonly NpgsqlConnection _npgsqlConnection;

    public uint BucketId { get; }

    public ShardNpgsqlConnection(
        NpgsqlConnection npgsqlConnection,
        uint bucketId)
    {
        _npgsqlConnection = npgsqlConnection;
        BucketId = bucketId;
    }

    protected override DbCommand CreateDbCommand()
    {
        var command = _npgsqlConnection.CreateCommand();
        return new ShardNpgsqlCommand(command, BucketId);
    }

    protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel) =>
        _npgsqlConnection.BeginTransaction(isolationLevel);

    public override void ChangeDatabase(string databaseName) =>
        _npgsqlConnection.ChangeDatabase(databaseName);

    public override void Close() => _npgsqlConnection.Close();

    public override void Open() => _npgsqlConnection.Open();

    public override string ConnectionString
    {
        get => _npgsqlConnection.ConnectionString;
        set => _npgsqlConnection.ConnectionString = value;
    }

    public override string Database => _npgsqlConnection.Database;
    public override ConnectionState State => _npgsqlConnection.State;
    public override string DataSource => _npgsqlConnection.DataSource;
    public override string ServerVersion => _npgsqlConnection.ServerVersion;

    public override ValueTask DisposeAsync()
    {
        return _npgsqlConnection.DisposeAsync();
    }
}