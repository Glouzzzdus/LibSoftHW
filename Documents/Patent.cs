using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Documents
{
    public class Patent : Document
    {
        public DateTime ExpirationDate { get; set; }
        public string UniqueId { get; set; }

        public override string GetCardInfo()
        {
            return $"Patent: {Title}, Authors: {String.Join(", ", Authors)}, Publication Date: {DatePublished}, Unique ID: {UniqueId}, Expires: {ExpirationDate}";
        }
    }
}
