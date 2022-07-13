using Newtonsoft.Json;
using StackExchange.Redis;
using System.Threading.Tasks;
using WebsiteBlazor.Abstract;
using AmaraCode.RainMaker.Models;

namespace WebsiteBlazor.Services;

public class RedisCacheService : ICacheService
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public RedisCacheService(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public Task<string> GetCacheValueAsync(string key)
    {
        string response;

        if (!string.IsNullOrEmpty(key))
        {
            var db = _connectionMultiplexer.GetDatabase();
            response = db.StringGetAsync(key).Result;
        }
        else
        {
            response = "";
        }

        return Task.FromResult(response);
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public async Task<T> GetCacheValueAsync<T>(string key)
    {
        var json = await GetCacheValueAsync(key);

        if (json != null)
        {
            var result = JsonConvert.DeserializeObject<T>(json);
            return result;
        }
        else
        {
            return default(T);
        }
    }

    /// <summary>
    /// Determine if a key exists in Redis
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public async Task<bool> KeyExistsAsync(string key)
    {
        if (!string.IsNullOrEmpty(key))
        {
            try
            {
                var db = _connectionMultiplexer.GetDatabase();

                /* This await method below was commented out because it never
                 * returns.  The method does not belong to use so we cannot
                 * fix the problem so the solution was to use the non-async
                 * sister method
                 *
                 */
                //var result = await db.KeyExistsAsync(key, CommandFlags.None);

                var result = await db.KeyExistsAsync(key, CommandFlags.None);

                return result;
            }
            catch
            {
                throw;
            }
        }
        else
        {
            throw new InvalidParameterException("The key cannot be null or empty.");
        }
    }

    /// <summary>
    /// Saves string value to Redis
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public async Task SetCacheValueAsync(string key, string value)
    {
        var db = _connectionMultiplexer.GetDatabase();
        await db.StringSetAsync(key, value);
    }
}