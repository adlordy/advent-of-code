namespace adlordy;

public class Problem8 : ProblemBase {
    string sample = 
    """
............
........0...
.....0......
.......0....
....0.......
......A.....
............
............
........A...
.........A..
............
............
""";
    public async Task<int> SolveB(){
        var content = await GetContent(8);

        var map = content.Split("\n", StringSplitOptions.RemoveEmptyEntries)
            .Select(l=>l.ToCharArray()).ToArray();

        var bound = new Point(map.Length, map[0].Length);
        var nodeMap = new bool[bound.X, bound.Y];
        var sets = new Dictionary<char, List<Point>>();
        for(var i=0;i<bound.X;i++)
            for(var j=0;j<bound.Y;j++){
                var c = map[i][j];
                if (c != '.'){
                    if (!sets.TryGetValue(c, out var list)){
                        sets[c] = list = new List<Point>();
                    }
                    list.Add(new Point(i, j));
                }
            }
        foreach(var set in sets.Values){
            for(var i=0;i<set.Count-1;i++){
                for(var j=i+1;j<set.Count;j++){
                    var a = set[i];
                    nodeMap[a.X, a.Y] = true;
                    var b = set[j];
                    nodeMap[b.X, b.Y] = true;
                    var d = a - b;
                    var p1 = a + d;
                    while (p1.InBound(bound)){
                        nodeMap[p1.X, p1.Y] = true;
                        p1 = p1 + d;
                    }
                    var p2 = b - d;
                    while (p2.InBound(bound)){
                        nodeMap[p2.X, p2.Y] = true;
                        p2 = p2 - d;
                    }
                }
            }
        }

        var count = 0;
        for(var i=0;i<bound.X;i++)
            for(var j=0;j<bound.Y;j++){
                count += nodeMap[i,j] ? 1 : 0;
            }
        return count;
    }
}