using System.Threading.Tasks;

namespace WebsiteBlazor.Abstract;

public interface ICacheService
{
    Task<string> GetCacheValueAsync(string key);

    Task<T> GetCacheValueAsync<T>(string key);

    Task<bool> KeyExistsAsync(string key);

    Task SetCacheValueAsync(string key, string value);
}