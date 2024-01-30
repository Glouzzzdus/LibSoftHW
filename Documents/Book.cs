using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Documents
{
    public class Book : Document
    {
        public string ISBN { get; set; }
        public int NumberOfPages { get; set; }
        public string Publisher { get; set; }

        public override string GetCardInfo()
        {
            return $"Book: {Title}, Authors: {String.Join(", ", Authors)}, Publication Date: {DatePublished}, ISBN: {ISBN}, Publisher: {Publisher}, Pages: {NumberOfPages}";
        }
    }
}
