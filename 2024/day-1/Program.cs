var content = await core.AdventOfCode.GetContent(1);
var lines = content.Split("\n", StringSplitOptions.RemoveEmptyEntries);
var a = new int[lines.Length];
var b = new int[lines.Length];

for(var i=0;i<lines.Length;i++){
    var values = lines[i].Split(" ", StringSplitOptions.RemoveEmptyEntries);
    a[i] = int.Parse(values[0]);
    b[i] = int.Parse(values[1]);
}

int Promblem1(){
    Array.Sort(a);
    Array.Sort(b);
    var sum = 0;

    for(var i=0;i<a.Length;i++){
        sum += Math.Abs(a[i] - b[i]);
    }
    return sum;
}

int Problem2(){
    var c = b.GroupBy(x => x).ToDictionary(x => x.Key, g => g.Count());
    return a.Select(x => c.TryGetValue(x, out var d) ? x*d : 0).Sum();
}

Console.WriteLine(Problem2());
