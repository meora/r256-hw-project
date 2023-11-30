﻿using System.Text;
using Murmur;
using Ozon.Route256.Practice.OrderService.Infrastructure.ClientBalancing;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.Dal.Common.Shard;

internal class StringShardingRule : IShardingRule<string>
{
    private readonly uint _bucketsCount;

    public StringShardingRule(
        IDbStore dbStore)
    {
        _bucketsCount = dbStore.BucketsCount;
    }

    public uint GetBucketId(
        string shardKey)
    {
        var hash = GetHashCodeFromShardKey(shardKey);
        return (uint)Math.Abs(hash) % _bucketsCount;
    }

    private int GetHashCodeFromShardKey(
        string shardKey)
    {
        var bytes = Encoding.UTF8.GetBytes(shardKey);
        var murmur = MurmurHash.Create32();
        var hash = murmur.ComputeHash(bytes);
        return BitConverter.ToInt32(hash);
    }
}