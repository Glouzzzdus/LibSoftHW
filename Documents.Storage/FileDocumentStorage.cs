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

        private readonly ICache cache;

        private readonly Dictionary<string, TimeSpan> cacheTimes = new()
        {
            { nameof(Book), TimeSpan.FromHours(1) },
            { nameof(LocalizedBook), TimeSpan.FromHours(2) },
            { nameof(Patent), TimeSpan.Zero }, // Do not cache.
            { nameof(Magazine), TimeSpan.MaxValue } // Cache indefinitely.
        };

        public FileDocumentStorage(ICache cache)
        {
            this.cache = cache;
        }

        public List<Document> GetByNumber(string documentNumber)
        {
            List<Document> documents = new List<Document>();

            string[] filePaths = Directory.GetFiles(DocumentsFolderPath, $"*_{documentNumber}.json");

            foreach (string filePath in filePaths)
            {
                string jsonData = File.ReadAllText(filePath);
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
                        cache.Add(filePath, doc, expirationTime);
                    }
                }
            }
            return documents;
        }
    }
}