using System.Collections.Generic;
using System.IO;
using Documents;
using Newtonsoft.Json;

namespace Documents.Storage
{
    public class FileDocumentStorage : IDocumentStorage
    {
        private const string DocumentsFolderPath = "./documents/";

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
                    documents.Add(doc);
                }
            }
            return documents;
        }
    }
}