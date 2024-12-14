using System.Threading.Tasks;
using System.Net;
using System.Net.Http;

namespace adlordy;

public abstract class ProblemBase
{
    private Uri _base = new Uri("https://adventofcode.com/");

    protected HttpClient GetClient()
    {
        var cookie = new CookieContainer();
        var handler = new HttpClientHandler
        {
            CookieContainer = cookie
        };
        cookie.Add(_base, new Cookie("session", Environment.GetEnvironmentVariable("SESSION")));
        return new HttpClient(handler);
    }

    protected async Task<string> GetContent(int id)
    {
        return await GetClient().GetStringAsync(GetInputUri(id));
    }

    protected async Task<StreamReader> GetReader(int id){
        var client = GetClient();
        var stream = await client.GetStreamAsync(GetInputUri(id));
        return new StreamReader(stream);
    }

    private Uri GetInputUri(int id) => new Uri(_base, $"2024/day/{id}/input");
}


public record struct Point(long X, long Y){
    public static Point operator -(Point a, Point b){
        return new Point(a.X - b.X, a.Y - b.Y);
    }

    public static Point operator +(Point a, Point b){
        return new Point(a.X + b.X, a.Y + b.Y);
    }

    public static Point operator *(Point a, long b){
        return new Point(a.X * b, a.Y * b);
    }

    public bool InBound(Point bound)
    {
        return X >= 0 && Y >=0 && X < bound.X && Y < bound.Y;
    }
}