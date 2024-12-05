namespace adlordy;

public record Node(int Value) : IComparable<Node> {
    public ISet<Node> Edges {get;} = new SortedSet<Node>();

    public int CompareTo(Node? other)
    {
        if (other is null)
            return 1;
        return this.Value.CompareTo(other.Value);
    }
}

public class Problem5 : ProblemBase {
    string sample = """
47|53
97|13
97|61
97|47
75|29
61|13
75|53
29|13
97|29
53|29
61|53
97|53
61|29
47|13
75|47
97|75
47|61
75|61
47|29
75|13
53|13

75,47,61,53,29
97,61,53,29,13
75,29,13
75,97,47,61,53
61,13,29
97,13,75,29,47
""";
    Dictionary<int, Node> map = new Dictionary<int, Node>();

    public async Task<int> SolveA(){
        return await Solve((pages) => {
            if (Validate(pages)){
                return pages[pages.Length / 2];
            }
            return 0;
        });
    }

    public Task<int> SolveB(){
        return Solve((pages) => {
            if (!Validate(pages)){
                pages = Order(pages);
                return pages[pages.Length / 2];
            }
            return 0;
        });
    }

    private int[] Order(int[] pages)
    {
        var set = new SortedSet<Node>();
        for(var i=0;i<pages.Length;i++){
            set.Add(map[pages[i]]);
        }
        for(var i=0;i<pages.Length;i++){
            var start = map[pages[i]];
            var path = Visit(start, new SortedSet<Node>(set));
            if (path.Length == pages.Length)
                return path;
        }
        return [];
    }

    private int[] Visit(Node start, SortedSet<Node> set)
    {
        set.Remove(start);
        if (set.Count == 0)
            return [start.Value];
        foreach(var edge in start.Edges){
            if (set.Contains(edge)){
                var path = Visit(edge, new SortedSet<Node>(set));
                if (path.Length == set.Count)
                    return [start.Value, ..path];
            }
        }
        return [];
    }

    private async Task<int> Solve(Func<int[], int> solver){
        using var reader = await GetReader(5);
        string? line;
        while(!String.IsNullOrEmpty(line = await reader.ReadLineAsync())){
            var parts = line.Split("|");
            var from = int.Parse(parts[0]);
            var to = int.Parse(parts[1]);
            if (!map.TryGetValue(from, out var fromNode)){
                map[from] = fromNode = new Node(from);
            }
            if (!map.TryGetValue(to, out var toNode)){
                map[to] = toNode = new Node(to);
            }
            fromNode.Edges.Add(toNode);
        }

        var sum = 0;
        while(!String.IsNullOrEmpty(line = await reader.ReadLineAsync())){
            var pages = line.Split(",").Select(int.Parse).ToArray();
            sum += solver(pages);
        }
        return sum;
    }

    private bool Validate(int[] pages)
    {
        for(var i=0;i<pages.Length-1;i++){
            for(var j=i+1;j<pages.Length;j++){
                var left = map[pages[i]];
                var right = map[pages[j]];
                if (right.Edges.Contains(left)){
                    return false;
                }
            }
        }
        return true;
    }
}