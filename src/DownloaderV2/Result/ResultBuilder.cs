using System.Text;

namespace DownloaderV2.Result;

public class ResultBuilder
{
    public List<ResultObject> Result { get; set; } = [];
    public void AddResult(ResultObject result) 
    {
        lock (result) { Result.Add(result); }
    } 
        
    public void PrintResult() => Console.WriteLine(this);

    public override string ToString()
    {
        return Result
            .Where(item => item.Count > 0)
            .Aggregate(new StringBuilder(), (sb, item) => sb.Append(item)).ToString();
    }
}