using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Documents
{
    public class Magazine : Document
    {
        public string Title { get; set; }
        public string Publisher { get; set; }
        public string ReleaseNumber { get; set; }
        public DateTime PublishDate { get; set; }

        public override string GetCardInfo()
        {
            return "Magazine";
        }
    }
}
