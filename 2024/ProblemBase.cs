using System.Threading.Tasks;
using System.Net;
using System.Net.Http;

namespace adlordy;

public abstract class ProblemBase
{
    private Uri _base = new Uri("https://adventofcode.com/");

    protected async Task<string> GetContent(int id)
    {
        var cookie = new CookieContainer();
        var handler = new HttpClientHandler
        {
            CookieContainer = cookie
        };
        cookie.Add(_base, new Cookie("session", Environment.GetEnvironmentVariable("SESSION")));
        var client = new HttpClient(handler);
        return await client.GetStringAsync(new Uri(_base, $"2024/day/{id}/input"));
    }
}
