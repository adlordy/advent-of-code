namespace adlordy;

public class Problem10 : ProblemBase {
    record struct Point(int X, int Y){
        public static Point operator -(Point a, Point b){
            return new Point(a.X - b.X, a.Y - b.Y);
        }

        public static Point operator +(Point a, Point b){
            return new Point(a.X + b.X, a.Y + b.Y);
        }

        public bool InBound(Point bound)
        {
            return X >= 0 && Y >=0 && X < bound.X && Y < bound.Y;
        }
    }

    int[][] map;
    Point bound;

    Point[] dirs = [
        new(0, 1),
        new(1, 0),
        new(0, -1),
        new(-1, 0)
    ];
    
    public async Task<int> SolveA(){
        var content = await GetContent(10);
        map = content.Split("\n", StringSplitOptions.RemoveEmptyEntries)
            .Select(l => l.ToCharArray().Select(c=>c - '0').ToArray()).ToArray();
        bound = new Point(map.Length, map[0].Length);

        var count = 0;
        for(var i=0;i<bound.X;i++){
            for(var j=0;j<bound.Y;j++){
                if (map[i][j] == 0){
                    var visited = new HashSet<Point>();
                    Traverse(new Point(i, j), visited, ref count);
                }
            }
        }
        return count;
    }

    private void Traverse(Point point, ISet<Point> visited, ref int count)
    {
        if (map[point.X][point.Y] == 9){
            if (!visited.Contains(point)){
                count++;
                visited.Add(point);
            }
            return;
        }

        foreach(var dir in dirs){
            var next = point + dir;
            if (!next.InBound(bound) || map[point.X][point.Y] != map[next.X][next.Y] - 1)
                continue;
            Traverse(next, visited, ref count);
        }
    }
}