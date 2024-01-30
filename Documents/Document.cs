namespace Documents
{
    public abstract class Document
    {
        public string Title { get; set; }
        public List<string> Authors { get; set; }
        public DateTime DatePublished { get; set; }

        public abstract string GetCardInfo();
    }
}
