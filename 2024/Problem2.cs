using System;

namespace adlordy;

public class Problem2 : ProblemBase {
    private async Task<int[][]> Load()
    {
        var content = await GetContent(2);
        var lines = content.Split("\n", StringSplitOptions.RemoveEmptyEntries);

        return lines.Select(line => {
            var parts = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            return parts.Select(int.Parse).ToArray();
        }).ToArray();
    }

    private bool Check(IEnumerable<int> numbers){
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

    private bool Check2(int[] numbers){
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

    public async Task<int> SolveA(){
        var count = 0;
        foreach(var numbers in await Load()){
            if (Check(numbers)){
                count++;
            }
        }
        return count;
    }

    public async Task<int> SolveB(){
        var count = 0;
        foreach(var numbers in await Load()){
            if (Check2(numbers)){
                count++;
            }
        }
        return count;
    }
}
