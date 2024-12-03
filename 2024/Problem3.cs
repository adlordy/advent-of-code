using System.Text.RegularExpressions;

namespace adlordy;

public class Problem3 : ProblemBase 
{
    public async Task<int> SolveA(){
        var content = await GetContent(3);
        var regex = new Regex(@"mul\((?<a>\d{1,3}),(?<b>\d{1,3})\)", RegexOptions.Compiled);
        var sum = 0;
        var matches = regex.Matches(content);
        foreach(var match in matches.OfType<Match>()){
            var a = int.Parse(match.Groups["a"].Value);
            var b = int.Parse(match.Groups["b"].Value);
            sum += a * b;
        }
        return sum;
    }

    public async Task<int> SolveB(){
        var content = await GetContent(3);
        var regex = new Regex(@"do\(\)|don't\(\)|mul\((?<a>\d{1,3}),(?<b>\d{1,3})\)", RegexOptions.Compiled);
        var sum = 0;
        var enabled = true;
        var matches = regex.Matches(content);
        foreach(var match in matches.OfType<Match>()){
            if (match.Value == "do()"){
                enabled = true;
            } else if (match.Value == "don't()"){
                enabled = false;
            } else if (enabled) {
                var a = int.Parse(match.Groups["a"].Value);
                var b = int.Parse(match.Groups["b"].Value);
                sum += a * b;
            }
        }
        return sum;
    }
}