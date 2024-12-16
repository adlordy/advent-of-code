
namespace adlordy;

public class Problem16 : ProblemBase {
    public async Task<long> SolveA(){
    var reader = await GetReader(16);
//         var reader = new StringReader(
// """
// #################
// #...#...#...#..E#
// #.#.#.#.#.#.#.#.#
// #.#.#.#...#...#.#
// #.#.#.#.###.#.#.#
// #...#.#.#.....#.#
// #.#.#.#.#.#####.#
// #.#...#.#.#.....#
// #.#.#####.#.###.#
// #.#.#.......#...#
// #.#.###.#####.###
// #.#.#...#.....#.#
// #.#.#.#####.###.#
// #.#.#.........#.#
// #.#.#.#########.#
// #S#.............#
// #################
// """            
//        );

        string? line;
        List<char[]> map = new();
        Point start = new();
        Point end = new();
        while(!string.IsNullOrEmpty(line = await reader.ReadLineAsync())){
            var startIndex = line.IndexOf('S');
            if (startIndex > -1)
                start = new Point(map.Count, startIndex);
            var endIndex = line.IndexOf('E');
            if (endIndex > -1)
                end = new Point(map.Count, endIndex);
            map.Add(line.ToCharArray());
        }

        var priorityQueue = new PriorityQueue<QueueItem, long>();
        priorityQueue.Enqueue(new(start, Direction.Right, 0, []), 0);
        while(priorityQueue.Count > 0){
            var item = priorityQueue.Dequeue();
            var path = new HashSet<Point>(item.Path) { item.Position };
            if (priorityQueue.Count > 0 
                && priorityQueue.Peek().Score == item.Score)
            {
                var nexItem = priorityQueue.Dequeue();
                path.UnionWith(nexItem.Path);
            }
            
            var next = item.Position + Point.FromDirection(item.Direction);
            char c = map[(int)next.X][next.Y];
            if (c == 'E') {
                return item.Path.Count + 1;
            }

            if (c == '.'){
                priorityQueue.Enqueue(new(next, item.Direction, item.Score + 1, path), item.Score + 1);
            }

            var clockwise = item.Direction.Clocwise();
            var nextClockwise = item.Position + Point.FromDirection(clockwise);
            if (map[(int)nextClockwise.X][nextClockwise.Y] != '#'){
                priorityQueue.Enqueue(new(nextClockwise, item.Direction.Clocwise(), item.Score + 1001, path), item.Score + 1000);
            }
            var counterClockwise = item.Direction.CounterClocwise();
            var nextCounterClockwise = item.Position + Point.FromDirection(counterClockwise);
            if (map[(int)nextCounterClockwise.X][nextCounterClockwise.Y] != '#'){
                priorityQueue.Enqueue(new(nextCounterClockwise, item.Direction.CounterClocwise(), item.Score + 1001, path), item.Score + 1000);
            }

            if (c != 'S'){
                map[(int)next.X][next.Y] = item.Direction.GetChar();
            }
        }

        return 0;
    }

    record struct QueueItem(Point Position, Direction Direction, long Score, HashSet<Point> Path);
}
