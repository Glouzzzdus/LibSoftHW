using System;
using System.Collections.Generic;
using System.Linq;
using Documents;
using System.Text;
using System.Threading.Tasks;

namespace Documents.Caching
{
    public interface ICache
    {
        void Add(string key, Document value, DateTimeOffset expirationTime);
        bool TryGetValue(string key, out Document value);
    }
}
