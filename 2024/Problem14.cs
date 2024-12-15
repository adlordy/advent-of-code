using System.Text.RegularExpressions;

namespace adlordy;

public class Problem14 : ProblemBase {
    Regex lineRg = new Regex(@"p=(?<x>-?\d+),(?<y>-?\d+) v=(?<vx>-?\d+),(?<vy>-?\d+)");
    public async Task<long> SolveA(){
        var reader = await GetReader(14);

        string? line;
        var bound = new Point(101, 103);
        var mid = bound / 2;
        var n = 100;
        long[] count = [0, 0, 0, 0]; 
        while(!string.IsNullOrEmpty(line = await reader.ReadLineAsync()))
        {
            var match = lineRg.Match(line);
            var p = new Point(long.Parse(match.Groups["x"].Value), long.Parse(match.Groups["y"].Value));
            var v = new Point(long.Parse(match.Groups["vx"].Value), long.Parse(match.Groups["vy"].Value));
            for(var i=0;i<n;i++)
                p = (p + v + bound) % bound;
            var index = GetQuadrant(mid, p);
            if (index > -1){
                count[index]++;
            }
        }
        return count[0]*count[1]*count[2]*count[3];
    }

    private static int GetQuadrant(Point mid, Point p)
    {
        if (p.X < mid.X)
        {
            if (p.Y < mid.Y)
            {
                return 0;
            }
            else if (p.Y > mid.Y)
            {
                return 1;
            }
        }
        else if (p.X > mid.X)
        {
            if (p.Y < mid.Y)
            {
                return 2;
            }
            else if (p.Y > mid.Y)
            {
                return 3;
            }
        }
        return -1;
    }
}