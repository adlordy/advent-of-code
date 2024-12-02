var content = await core.AdventOfCode.GetContent(2);
var lines = content.Split("\n", StringSplitOptions.RemoveEmptyEntries);

bool Check(IEnumerable<int> numbers){
    var e = numbers.GetEnumerator();
    e.MoveNext();
    var prev = e.Current;
    var dir = 0;
    while(e.MoveNext()){
        var current = e.Current;
        var diff = current - prev;
        var absDiff = Math.Abs(diff);
        if (dir == 0)
            dir = Math.Sign(diff);
        if (absDiff < 1 || absDiff > 3 || dir != Math.Sign(diff))
            return false;
        prev = current;
    }
    return true;
}

bool Check2(int[] numbers){
    if (Check(numbers)){
        return true;
    }
    else 
    {
        for(var i=0;i<numbers.Length;i++){
            int[] variation = [..numbers[..i],..numbers[(i+1)..]];
            if (Check(variation))
                return true;
        }
    }
    return false;
}

int Problem1(){
    var count = 0;
    foreach(var line in lines){
        var parts = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        var numbers = parts.Select(int.Parse);
        if (Check(numbers)){
            count++;
        }
    }
    return count;
}

int Problem2(){
    var count = 0;
    foreach(var line in lines){
        var parts = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        var numbers = parts.Select(int.Parse).ToArray();
        if (Check2(numbers)){
            count++;
        }
    }
    return count;
}
Console.WriteLine(Problem2());