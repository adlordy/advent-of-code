using System.Text.RegularExpressions;

namespace adlordy;

public class Problem13 : ProblemBase {
    Regex buttonRg = new Regex(@"Button [AB]: X\+(?<x>\d+), Y\+(?<y>\d+)");
    Regex prizeRg = new Regex(@"Prize: X=(?<x>\d+), Y=(?<y>\d+)");

    public async Task<long> SolveB(){
        var reader = await GetReader(13);

        string? line;
        long result = 0;
        while(!string.IsNullOrWhiteSpace(line = await reader.ReadLineAsync())){
            var a = ParseButton(line);
            var b = ParseButton(await reader.ReadLineAsync());
            var p = ParsePrize(await reader.ReadLineAsync());
            var r = SolveB(a,b,p);
            if (r != long.MaxValue){
                result += r;
            }
            await reader.ReadLineAsync();
        }
        return result;
    }

    private long SolveB(Point a, Point b, Point p)
    {
        p = new(p.X + 10000000000000L, p.Y + 10000000000000L);
        var n = p.Y*a.X - a.Y * p.X;
        var d = a.X * b.Y - b.X * a.Y;
        if (n % d == 0){
            var j = n / d;
            var i = (p.X - b.X * j) / a.X;
            return i*3 + j;
        }
        return long.MaxValue;
    }

    private int Solve(Point a, Point b, Point p)
    {
        var result = int.MaxValue;
        for(var i=0;i<101;i++){
            for(var j=0;j<101;j++){
                if (a*i + b*j == p){
                    result = Math.Min(result, i*3 + j);
                }
            }
        }
        return result;
    }

    private Point ParsePrize(string line)
    {
        var m = prizeRg.Match(line);
        return new Point(int.Parse(m.Groups["x"].Value), int.Parse(m.Groups["y"].Value));
    }

    private Point ParseButton(string line)
    {
        var m = buttonRg.Match(line);
        return new Point(int.Parse(m.Groups["x"].Value), int.Parse(m.Groups["y"].Value));
    }
}
