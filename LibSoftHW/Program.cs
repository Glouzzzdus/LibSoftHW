using Documents;
using Documents.Storage;
class Program
{
    static void Main(string[] args)
    {
        IDocumentStorage storage = new FileDocumentStorage(); 

        Console.WriteLine("Enter document number:");
        string documentNumber = Console.ReadLine();

        List<Documents.Document> foundDocuments = storage.GetByNumber(documentNumber);

        Console.WriteLine($"Found {foundDocuments.Count} documents:");

        foreach (Document doc in foundDocuments)
        {
            Console.WriteLine(doc.GetCardInfo());
        }
    }
}