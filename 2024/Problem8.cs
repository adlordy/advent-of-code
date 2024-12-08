namespace adlordy;

public record struct P(int x, int y){
    public static P operator -(P a, P b){
        return new P(a.x - b.x, a.y - b.y);
    }

    public static P operator +(P a, P b){
        return new P(a.x + b.x, a.y + b.y);
    }

    public bool InBound(P bound)
    {
        return x >= 0 && y >=0 && x < bound.x && y < bound.y;
    }
}
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

        var bound = new P(map.Length, map[0].Length);
        var nodeMap = new bool[bound.x, bound.y];
        var sets = new Dictionary<char, List<P>>();
        for(var i=0;i<bound.x;i++)
            for(var j=0;j<bound.y;j++){
                var c = map[i][j];
                if (c != '.'){
                    if (!sets.TryGetValue(c, out var list)){
                        sets[c] = list = new List<P>();
                    }
                    list.Add(new P(i, j));
                }
            }
        foreach(var set in sets.Values){
            for(var i=0;i<set.Count-1;i++){
                for(var j=i+1;j<set.Count;j++){
                    var a = set[i];
                    nodeMap[a.x, a.y] = true;
                    var b = set[j];
                    nodeMap[b.x, b.y] = true;
                    var d = a - b;
                    var p1 = a + d;
                    while (p1.InBound(bound)){
                        nodeMap[p1.x, p1.y] = true;
                        p1 = p1 + d;
                    }
                    var p2 = b - d;
                    while (p2.InBound(bound)){
                        nodeMap[p2.x, p2.y] = true;
                        p2 = p2 - d;
                    }
                }
            }
        }

        var count = 0;
        for(var i=0;i<bound.x;i++)
            for(var j=0;j<bound.y;j++){
                count += nodeMap[i,j] ? 1 : 0;
            }
        return count;
    }
}