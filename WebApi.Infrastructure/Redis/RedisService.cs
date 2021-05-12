using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApi.Infrastructure.Attributes;
using WebApi.Infrastructure.Exceptions;
using WebApi.Infrastructure.LifetimeInterfaces;

namespace WebApi.Infrastructure.Redis
{
    [Service(ServiceLifetime.Singleton)]
    public class RedisService : IRedisService, ISingletonInterface
    {
        private readonly IDatabase database;
        private readonly ILogger<RedisService> logger;

        public RedisService(
            IConnectionMultiplexer connection,
            ILogger<RedisService> logger
            )
        {
            database = connection.GetDatabase();
            this.logger = logger;
        }

        public bool Contains(string key)
        {
            return database.KeyExists(key, CommandFlags.None);
        }

        public T Get<T>(string key, Func<T> callback = null, TimeSpan? expiry = null, bool @lock = false)
        {
            var data = database.StringGet(key);
            if (!data.IsNullOrEmpty)
            {
                return JsonConvert.DeserializeObject<T>(data);
            }
            if (callback == null) return default(T);

            if (@lock &&
                !database.LockTake(key, Environment.MachineName, TimeSpan.MaxValue))
            {
                throw new ValidateLevelException("try again later");
            }

            var fault = default(SystemLevelException);
            var value = default(T);
            try
            {
                value = callback.Invoke();
                if (value != null)
                {
                    Set(key, value, expiry);
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                fault = new SystemLevelException(e.Message, e);
            }
            finally
            {
                if (@lock)
                {
                    database.LockRelease(key, Environment.MachineName);
                }
            }

            if (fault != null) throw fault;

            return value;
        }

        public bool Set<T>(string key, T value, TimeSpan? expiry = null)
        {
            return database.StringSet(key, JsonConvert.SerializeObject(value), expiry ?? TimeSpan.FromSeconds(3600));
        }

        public bool Remove(string key, Action callback = null, bool @lock = false)
        {
            if (@lock)
            {
                if (!database.LockTake(key, Environment.MachineName, TimeSpan.MaxValue)) return false;
            }

            var result = true;
            var fault = default(SystemLevelException);
            if (Contains(key))
            {
                result = database.KeyDelete(key, CommandFlags.None);
            }
            if (result)
            {
                try
                {
                    callback?.Invoke();
                }
                catch (Exception e)
                {
                    logger.LogError(e, e.Message);
                    fault = new SystemLevelException(e.Message, e);
                }
            }

            if (@lock)
            {
                database.LockRelease(key, Environment.MachineName);
            }
            if (fault != null)
            {
                throw fault;
            }

            return result;
        }

        public bool RemoveByPattern(string pattern)
        {
            foreach (var endpoint in database.Multiplexer.GetEndPoints())
            {
                var server = database.Multiplexer.GetServer(endpoint);
                var keys = server.Keys(database.Database, pattern, 999);
                foreach (var key in keys) database.KeyDelete(key);
            }

            return true;
        }

        public Dictionary<T, V> HashGetAll<T, V>(string key)
        {
            var data = database.HashGetAll(key);
            if (data.Length > 0)
            {
                return data.ToDictionary(m => JsonConvert.DeserializeObject<T>(m.Name), m => JsonConvert.DeserializeObject<V>(m.Value));
            }
            else
            {
                return default;
            }
        }

        public V HashGet<T, V>(string key, T hashName)
        {
            var data = database.HashGet(key, JsonConvert.SerializeObject(hashName));
            if (!data.IsNullOrEmpty)
            {
                return JsonConvert.DeserializeObject<V>(data);
            }
            else
            {
                return default;
            }
        }

        public bool HashSet<T, V>(string key, T hashName, V hashValue)
        {
            return database.HashSet(key, JsonConvert.SerializeObject(hashName), JsonConvert.SerializeObject(hashValue));
        }

        public bool HashRemove<T>(string key, T hashName)
        {
            return database.HashDelete(key, JsonConvert.SerializeObject(hashName));
        }
    }
}
