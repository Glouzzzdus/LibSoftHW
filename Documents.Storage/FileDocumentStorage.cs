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
                Document doc = JsonConvert.DeserializeObject<Document>(jsonData);
                documents.Add(doc);
            }

            return documents;
        }
    }
}