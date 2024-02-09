using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Documents.Storage
{
    public interface IFileReader
    {
        string ReadAllText(string filePath);
        bool Exists(string filePath);
    }
}
