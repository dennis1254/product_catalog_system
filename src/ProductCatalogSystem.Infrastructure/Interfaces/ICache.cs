
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalogSystem.Infrastructure.Interfaces
{
    public interface ICache
    {
        Task<bool> SetString(string key, string data, string collection = "default", long expirationInMin = 1440);
        Task<bool> RemoveString(string key, string collection = "default");
        Task<string> GetString(string key, string collection = "default");
    }
}
