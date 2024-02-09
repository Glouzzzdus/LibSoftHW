using System.Collections.Generic;
using System.IO;
using Documents;
using Documents.Caching;
using Newtonsoft.Json;

namespace Documents.Storage
{
    public class FileDocumentStorage : IDocumentStorage
    {
        private const string DocumentsFolderPath = "./documents/";

        private readonly ICache _cache;
        private readonly string _rootPath;
        private readonly IFileReader _fileReader;
        private readonly IDirectoryHelper _directoryHelper;

        public FileDocumentStorage(ICache cache, string rootPath, IFileReader fileReader, IDirectoryHelper directoryHelper)
        {
            _cache = cache;
            _rootPath = rootPath;
            _fileReader = fileReader;
            _directoryHelper = directoryHelper;
        }


        private readonly Dictionary<string, TimeSpan> cacheTimes = new()
        {
            { nameof(Book), TimeSpan.FromHours(1) },
            { nameof(LocalizedBook), TimeSpan.FromHours(2) },
            { nameof(Patent), TimeSpan.Zero }, // Do not cache.
            { nameof(Magazine), TimeSpan.MaxValue } // Cache indefinitely.
        };

        public FileDocumentStorage(ICache cache)
        {
            this._cache = cache;
        }

        public List<Document> GetByNumber(string documentNumber)
        {
            List<Document> documents = new List<Document>();

            // Use _directoryHelper instead of System.IO.Directory
            string[] filePaths = _directoryHelper.GetFiles($"{DocumentsFolderPath}*_{documentNumber}.json");

            foreach (string filePath in filePaths)
            {
                // Use _fileReader instead of System.IO.File
                string jsonData = _fileReader.ReadAllText(filePath);
                dynamic docData = JsonConvert.DeserializeObject<dynamic>(jsonData);
                Document doc;

                switch ((string)docData.type)
                {
                    case nameof(Book):
                        doc = JsonConvert.DeserializeObject<Book>(jsonData);
                        break;
                    case nameof(LocalizedBook):
                        doc = JsonConvert.DeserializeObject<LocalizedBook>(jsonData);
                        break;
                    case nameof(Patent):
                        doc = JsonConvert.DeserializeObject<Patent>(jsonData);
                        break;
                    case nameof(Magazine):
                        doc = JsonConvert.DeserializeObject<Magazine>(jsonData);
                        break;
                    default:
                        doc = null;
                        break;
                }

                if (doc != null)
                {
                    TimeSpan documentTypeCacheTime;
                    if (cacheTimes.TryGetValue(doc.GetType().Name, out documentTypeCacheTime))
                    {
                        DateTime expirationTime = DateTime.Now + documentTypeCacheTime;
                        _cache.Add(filePath, doc, expirationTime);
                    }
                }
            }
            return documents;
        }
    }
}