namespace adlordy;

public class Problem12 : ProblemBase {
    record Direction(Point Move, Point From, Point To);

    record Edge(Point to, Point origin);
    class EdgeList{
        List<Edge> List = new List<Edge>();

        internal void Add(Edge value)
        {
            List.Add(value);
        }

        internal (Point to, Point toOrigin) Get(Point origin)
        {
            if (List.Count == 1){
                return (List[0].to, List[0].origin);
            }

            var edge = List.Single(l => l.origin == origin);
            return (edge.to, edge.origin);
        }
    }
    
    Direction[] dirs = [
        new(new(1, 0),  new(1, 1),  new(1, 0)),
        new(new(0, 1),  new(0, 1), new(1, 1)),
        new(new(-1, 0), new(0, 0), new(0, 1)),
        new(new(0, -1), new(1, 0), new(0, 0)),
    ];
    

    int[][] map;
    Point bound;

    private long Flood(Point from, int color, int[,] colors, int c){
        var queue = new Queue<Point>();
        queue.Enqueue(from);
        var edges = new EdgeList[bound.X + 1, bound.Y + 1];
        var area = 0;
        var edgeSet = new HashSet<(Point from, Point to, Point origin)>();
        void AddEdge(Point origin, Point from, Point to){
            edgeSet.Add((from, to, origin));
            var edgeList = edges[from.X, from.Y];
            if (edgeList == null)
                edgeList = edges[from.X, from.Y] = new EdgeList();
            edgeList.Add(new(to, origin));
        }
        
        while(queue.Count > 0) {
            var p = queue.Dequeue();
            if (colors[p.X,p.Y] == 0){
                colors[p.X, p.Y] = color;
                area++;
                foreach(var dir in dirs){
                    var next = p + dir.Move;
                    if (!next.InBound(bound)){
                        AddEdge(p, p + dir.From, p + dir.To);
                    } else {
                        if (colors[next.X, next.Y] == 0 && map[next.X][next.Y] == c){
                            queue.Enqueue(next);
                        } else if (colors[next.X, next.Y] != color){
                            AddEdge(p, p + dir.From, p + dir.To);
                        }
                    }
                }
            }
        }
        var sides = 0;
        while(edgeSet.Count > 0){
            var start = edgeSet.First();
            edgeSet.Remove(start);
            var startFrom = start.from;
            var next = start.to;
            var dir = start.from - start.to;
            var origin = start.origin;
            while(next != startFrom){
                var toList = edges[next.X, next.Y]!;
                var (to, toOrigin) = toList.Get(origin);
                edgeSet.Remove((next, to, toOrigin));
                origin = toOrigin;
                var nextDir = next - to;
                if (nextDir != dir)
                    sides++;
                dir = nextDir;
                next = to;
            }
            if (dir != start.from - start.to)
                sides++;
        }

        return sides * area;
    }

    public async Task<long> SolveB(){
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
