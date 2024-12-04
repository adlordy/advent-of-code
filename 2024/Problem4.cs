using System.ComponentModel.Design;

namespace adlordy;

public class Problem4 : ProblemBase {
    private readonly char[] word = ['X', 'M', 'A', 'S'];
    private readonly (int, int)[] dir = [
            (0, 1),
            (1, 1),
            (1, 0),
            (1, -1),
            (0, -1),
            (-1, -1),
            (-1, 0),
            (-1, 1),
    ];
    bool Check(char[][] chars, int i, int j, int k){
        var d = dir[k];
        for(var n=0;n<word.Length;n++){
            if (i<0 || j<0 || i>=chars.Length || j >= chars[0].Length)
                return false;
            if (chars[i][j] != word[n])
                return false;
            i += d.Item1;
            j += d.Item2;
        }
        return true;
    }

    private async Task<int> Solve(Func<char[][],int,int,int> check){
        var content = await GetContent(4);

        var lines = content.Split("\n", StringSplitOptions.RemoveEmptyEntries);
        var chars = lines.Select(l => l.ToCharArray()).ToArray();
        var count = 0;
        for(var i=0;i<chars.Length;i++){
            for(var j=0;j<chars[i].Length;j++){
                count += check(chars, i, j);
            }
        }
        return count;
    }

    public Task<int> SolveA(){
        return Solve((chars, i, j) => {
            var count = 0;
            for(var k=0;k<dir.Length;k++){
                if (Check(chars, i, j, k))
                    count++;
            }
            return count;
        });
    }

    public Task<int> SolveB(){
        return Solve((chars, i, j) => {
            if (i<1 || j<1 || i>=chars.Length-1 || j>=chars[0].Length-1)
                return 0;
            if (chars[i][j] != 'A')
                return 0;
            var a = chars[i-1][j-1];
            var b = chars[i-1][j+1];
            var c = chars[i+1][j+1];
            var d = chars[i+1][j-1];

            if ((a == 'M' && c =='S'|| a == 'S' && c == 'M') && (b == 'M' && d == 'S' || b == 'S' && d == 'M'))
                return 1;
            return 0;
        });
    }
}