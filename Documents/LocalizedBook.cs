using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Documents
{
    public class LocalizedBook : Book
    {
        public string OriginalPublisher { get; set; }
        public string CountryOfLocalization { get; set; }
        public string LocalPublisher { get; set; }

        public override string GetCardInfo()
        {
            return $"Localized Book: {Title}, Authors: {String.Join(", ", Authors)}, Publication Date: {DatePublished}, ISBN: {ISBN}, Original Publisher: {OriginalPublisher}, Local Publisher: {LocalPublisher}, Country: {CountryOfLocalization}, Pages: {NumberOfPages}";
        }
    }
}
