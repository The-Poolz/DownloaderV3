using System.Text;

namespace DownloaderV3.Result;

public class ResultBuilder
{
    public List<ResultObject> Result { get; set; } = new();
    public void AddResult(ResultObject result) 
    {
        lock (result) { Result.Add(result); }
    } 
        
    public void PrintResult()
    {
        Console.WriteLine(this);
    }

    public override string ToString()
    {
        return Result
            .Where(item => item.Count > 0)
            .Aggregate(new StringBuilder(), (sb, item) => sb.Append(item)).ToString();
    }
}