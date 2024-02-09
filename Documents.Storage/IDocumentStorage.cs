using System;
using System.Collections.Generic;
using Documents;   

namespace Documents.Storage
{
    public interface IDocumentStorage
    {
        List<Document> GetByNumber(string documentNumber);
    }
}
