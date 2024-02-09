using System.Runtime.Caching;
using Documents;
using Documents.Caching;
using Documents.Storage;

class Program
{
    static void Main(string[] args)
    {
        ObjectCache memoryCache = System.Runtime.Caching.MemoryCache.Default;
        ICache cache = new Documents.Caching.MemoryCache(memoryCache);
        IDocumentStorage storage = new FileDocumentStorage(cache);

        Console.WriteLine("Enter document number:");
        string documentNumber = Console.ReadLine();

        List<Document> foundDocuments = storage.GetByNumber(documentNumber);

        Console.WriteLine($"Found {foundDocuments.Count} documents:");

        foreach (Document doc in foundDocuments)
        {
            Console.WriteLine(doc.GetCardInfo());
        }
    }
}