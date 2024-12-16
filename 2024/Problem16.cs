
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
        priorityQueue.Enqueue(new(start, Direction.Right, 0, [start]), 0);
        long best = long.MaxValue;
        var paths = new HashSet<Point>{start, end};
        while(priorityQueue.Count > 0){
            var item = priorityQueue.Dequeue();
            var next = item.Position + Point.FromDirection(item.Direction);

            char c = map[(int)next.X][next.Y];
            if (c == 'E'){
                best = item.Score;
                paths.UnionWith(item.Path);
            } else {
                if (c == '.' && item.Score + 1 <= best){
                    priorityQueue.Enqueue(new(next, item.Direction, item.Score + 1, [..item.Path, next]), item.Score + 1);
                }

                var clockwise = item.Direction.Clocwise();
                var nextClockwise = item.Position + Point.FromDirection(clockwise);
                if ((map[(int)nextClockwise.X][nextClockwise.Y] == '.' || nextClockwise == end) && item.Score + 1000 <= best){
                    priorityQueue.Enqueue(new(item.Position, item.Direction.Clocwise(), item.Score + 1000, item.Path), item.Score + 1000);
                }
                var counterClockwise = item.Direction.CounterClocwise();
                var nextCounterClockwise = item.Position + Point.FromDirection(counterClockwise);
                if ((map[(int)nextCounterClockwise.X][nextCounterClockwise.Y] == '.' || nextCounterClockwise == end) && item.Score + 1000 <= best){
                    priorityQueue.Enqueue(new(item.Position, item.Direction.CounterClocwise(), item.Score + 1000, item.Path), item.Score + 1000);
                }
            }
        }

        return paths.Count;
    }

    record struct QueueItem(Point Position, Direction Direction, long Score, List<Point> Path);
}
