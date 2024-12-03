namespace adlordy;

public class Problem1 : ProblemBase {
    private async  Task<(int[], int[])> Load()
    {
        var content = await GetContent(1);
        var lines = content.Split("\n", StringSplitOptions.RemoveEmptyEntries);
        var a = new int[lines.Length];
        var b = new int[lines.Length];

        for(var i=0;i<lines.Length;i++){
            var values = lines[i].Split(" ", StringSplitOptions.RemoveEmptyEntries);
            a[i] = int.Parse(values[0]);
            b[i] = int.Parse(values[1]);
        }
        return (a,b);
    }

    public async Task<int> SolveA()
    {
        var (a, b) = await Load();
        Array.Sort(a);
        Array.Sort(b);
        var sum = 0;

        for(var i=0;i<a.Length;i++){
            sum += Math.Abs(a[i] - b[i]);
        }
        return sum;
    }

    public async Task<int> SolveB()
    {
        var (a, b) = await Load();
        var c = b.GroupBy(x => x).ToDictionary(x => x.Key, g => g.Count());
        return a.Select(x => c.TryGetValue(x, out var d) ? x*d : 0).Sum();
    }
}
