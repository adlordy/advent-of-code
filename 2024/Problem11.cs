
namespace adlordy;

public class Problem11 : ProblemBase
{
    public int Digits(long i){
        var n = 1;
        if ( i >= 100000000L ) { n += 8; i /= 100000000L; }
        if ( i >= 10000L     ) { n += 4; i /= 10000L; }
        if ( i >= 100L       ) { n += 2; i /= 100L; }
        if ( i >= 10L        ) { n += 1; }
        return n;
    }
    public long Solve(int total)
    {
        var stones = new Dictionary<long, long>{
            {3 ,1 },
            {386358, 1},
            {86195, 1},
            {85, 1},
            {1267, 1},
            {3752457, 1},
            {0, 1},
            {741, 1}
        };

        for(var i=0;i<total;i++){
            var next = new Dictionary<long,long>(stones.Count * 2);
            foreach(var stone in stones){
                var value = stone.Key;
                var count = stone.Value;
                var newValue = -1L;
                if (value == 0){
                    value = 1;
                } else {
                    var digits = Digits(value);
                    if (digits % 2 ==0){
                        var n = digits /2;
                        var s = 1;
                        while(n>0){
                            s *= 10;
                            n--;
                        }
                        var right = value % s;
                        var left = (value - right) / s;
                        value = left;
                        newValue = right;
                    } else {
                        value *= 2024;
                    }
                }
                if (next.TryGetValue(value, out var oldCount)){
                    next[value] = count + oldCount;
                } else {
                    next[value] = count;
                }
                if (newValue!= -1) {
                    if (next.TryGetValue(newValue, out oldCount)){
                        next[newValue] = count + oldCount;
                    } else {
                        next[newValue] = count;
                    }
                }
            }
            stones = next;
        }

        return stones.Values.Sum();
    }
}