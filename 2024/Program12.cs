namespace adlordy;

public class Problem12 : ProblemBase {
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
    Point[] dirs = [
        new(0, 1),
        new(1, 0),
        new(0, -1),
        new(-1, 0)
    ];
    

    int[][] map;
    Point bound;

    private long Flood(Point from, int color, int[,] colors, int c){
        var queue = new Queue<Point>();
        queue.Enqueue(from);
        var area = 0;
        var perimimeter = 0;
        while(queue.Count > 0) {
            var p = queue.Dequeue();
            if (colors[p.X,p.Y] == 0){
                colors[p.X, p.Y] = color;
                area++;
                foreach(var dir in dirs){
                    var next = p + dir;
                    if (!next.InBound(bound)){
                        perimimeter++;
                    } else {
                        if (colors[next.X, next.Y] == 0 && map[next.X][next.Y] == c){
                            queue.Enqueue(next);
                        } else if (colors[next.X, next.Y] != color){
                            perimimeter++;
                        }
                    }
                }
            }
        }
        return perimimeter * area;
    }

    public async Task<long> SolveA(){
        var content = await GetContent(12);
        map = content.Split("\n", StringSplitOptions.RemoveEmptyEntries)
            .Select(l => l.ToCharArray().Select(c=>c - 'A').ToArray()).ToArray();
        bound = new Point(map.Length, map[0].Length);
        var color = new int[bound.X,bound.Y];
        
        var cost = 0L;
        var c = 1;
        for(var i=0;i<bound.X;i++){
            for(var j=0;j<bound.Y;j++){
                if (color[i,j] == 0){
                    cost += Flood(new Point(i, j), c++, color, map[i][j]);
                }
            }
        }

        return cost;
    }
}