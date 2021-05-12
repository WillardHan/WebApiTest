using System;
using System.Collections.Generic;
using System.Text;

namespace WebApi.Infrastructure.Redis
{
    public interface IRedisService
    {
        bool Contains(string key);
        T Get<T>(string key, Func<T> callback = null, TimeSpan? expiry = null, bool @lock = false);
        bool Set<T>(string key, T value, TimeSpan? expiry = null);
        bool Remove(string key, Action callback = null, bool @lock = false);
        bool RemoveByPattern(string pattern);
        Dictionary<T, V> HashGetAll<T, V>(string key);
        V HashGet<T, V>(string key, T hashName);
        bool HashSet<T, V>(string key, T hashName, V hashValue);
        bool HashRemove<T>(string key, T hashName);
    }
}
