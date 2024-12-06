namespace adlordy;

public record struct Pos(int i, int j){
    public static Pos operator +(Pos a, Pos b) => new Pos(a.i + b.i, a.j + b.j);
}

public class Problem6 : ProblemBase {
    string sample = """
....#.....
.........#
..........
..#.......
.......#..
..........
.#..^.....
........#.
#.........
......#...
""";
    private Pos[] dir = [
        new(-1, 0),
        new(0, 1),
        new(1, 0),
        new(0, -1)
    ];
    public async Task<int> SolveA()
    {
        var (map, s) = await Load();
        return Traverse(map, s, true).Item2;
    }

    public async Task<int> SolveB()
    {
        var (map, s) = await Load();
        var limit = map.Length * map[0].Length;
        var count = 0;
        for(var i=0;i<map.Length;i++){
            for(var j=0;j<map[0].Length;j++){
                var o = new Pos(i, j);
                if (o != s && map[o.i][o.j] == '.'){
                    map[o.i][o.j] = '#';
                    var (steps, _) = Traverse(map, s, false);
                    if (steps>=limit){
                        count++;
                    }
                    map[o.i][o.j] = '.';
                }
            }
        }
        return count;
    }

    private (int,int) Traverse(char[][] map, Pos s, bool mark)
    {
        var d = 0;
        var l = s;
        var count = 0;
        var step = 0;
        var limit = map.Length * map[0].Length;
        while (s.i >= 0 && s.j >= 0 && s.i < map.Length && s.j < map[0].Length && step < limit)
        {
            step++;
            var c = map[s.i][s.j];
            if (c == '#')
            {
                d = (d + 1) % dir.Length;
                s = l + dir[d];
            }
            else
            {
                if (map[s.i][s.j] == '.' && mark)
                {
                    map[s.i][s.j] = 'X';
                    count++;
                }
                l = s;
                s = s + dir[d];
            }
        }
        return (step, count);
    }

    private async Task<(char[][],Pos)> Load()
    {
        var content = await GetContent(6);
        var map = content.Split("\n", StringSplitOptions.RemoveEmptyEntries)
            .Select(l => l.ToCharArray()).ToArray();
        var s = map.Select((l, i) => new Pos(i, Array.IndexOf(l, '^')))
            .Single(o => o.j > -1);
        map[s.i][s.j] = '.';
        return (map, s);
    }
}
