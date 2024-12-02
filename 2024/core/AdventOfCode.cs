using System.Threading.Tasks;
using System.Net;
using System.Net.Http;

namespace core;

public static class AdventOfCode
{
    public static async Task<string> GetContent(int id){
        var cookie = new CookieContainer();
        var handler = new HttpClientHandler{
            CookieContainer = cookie
        };
        cookie.Add(new Uri("https://adventofcode.com/"), new Cookie("session", Environment.GetEnvironmentVariable("SESSION")));
        var client = new HttpClient(handler);
        return await client.GetStringAsync($"https://adventofcode.com/2024/day/{id}/input");
    }
}
