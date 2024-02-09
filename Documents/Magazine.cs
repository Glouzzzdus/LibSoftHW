
namespace Documents;
public class Magazine : Document
{
    public string Publisher { get; set; }
    public int ReleaseNumber { get; set; }

    public override string GetCardInfo()
    {
        return $"Magazine: {Title}, Publisher: {Publisher}, Release number: {ReleaseNumber}, Publication Date: {DatePublished}";
    }
}