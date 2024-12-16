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

    public static Point operator /(Point a, long b){
        return new Point(a.X / b, a.Y / b);
    }

    public static Point operator %(Point a, Point b){
        return new Point(a.X % b.X, a.Y % b.Y);
    }

    public bool InBound(Point bound)
    {
        return X >= 0 && Y >=0 && X < bound.X && Y < bound.Y;
    }

    public static Point FromDirection(Direction d){
        return d switch{
            Direction.Right => new Point(0, 1),
                Direction.Down => new Point(1, 0),
                Direction.Left => new Point(0, -1),
                Direction.Up => new Point(-1, 0),
                _ => throw new ArgumentOutOfRangeException(),
        };
    }
}

    public enum Direction {
        Right,
        Down,
        Left,
        Up
    }

    public static class DirectionExtensions{
        public static char GetChar(this Direction direction){
            return direction switch {
                Direction.Right => '>',
                Direction.Down => 'v',
                Direction.Left => '<',
                Direction.Up => '^',
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        public static Direction Clocwise(this Direction direction){
            return direction switch {
                Direction.Right => Direction.Down,
                Direction.Down => Direction.Left,
                Direction.Left => Direction.Up,
                Direction.Up => Direction.Right,
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        public static Direction CounterClocwise(this Direction direction){
            return direction switch {
                Direction.Right => Direction.Up,
                Direction.Down => Direction.Right,
                Direction.Left => Direction.Down,
                Direction.Up => Direction.Left,
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        public static Direction Opposite(this Direction direction){
            return direction switch {
                Direction.Right => Direction.Left,
                Direction.Down => Direction.Up,
                Direction.Left => Direction.Right,
                Direction.Up => Direction.Down,
                _ => throw new ArgumentOutOfRangeException(),
            };
        }
    }