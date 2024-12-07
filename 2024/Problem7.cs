namespace adlordy;

public class Problem7 : ProblemBase {
    string sample = """
190: 10 19
3267: 81 40 27
83: 17 5
156: 15 6
7290: 6 8 6 15
161011: 16 10 13
192: 17 8 14
21037: 9 7 18 13
292: 11 6 16 20
""";
    public async Task<long> SolveA(){
        var reader = await GetReader(7);
        long count = 0;
        string? line;
        while(!String.IsNullOrEmpty(line = await reader.ReadLineAsync())){
            var split = line.IndexOf(':');
            var result = long.Parse(line[..split]);
            var numbers = line[(split+2)..].Split(" ").Select(long.Parse).ToArray();
            var digits = numbers.Select(n => (long) Math.Pow(10, n.ToString().Length)).ToArray();
            var n = 1L<<((numbers.Length-1)*2);
            for(var i=0; i<n; i++) {
                if (Try(numbers, digits, result, i)){
                    count += result;
                    break;
                }
            }
        }
        return count;
    }

    private bool Try(long[] numbers, long[] digits, long result, int n)
    {
        var sum = numbers[0];
        var index = 3L;
        for(var j=0;j<numbers.Length-1;j++){
            var bits = (index & n) >> j*2;
            switch(bits){
                case 0:
                    sum *= numbers[j+1];
                    break;
                case 1:
                    sum += numbers[j+1];
                    break;
                case 2:
                    sum = sum * digits[j+1] + numbers[j+1];
                    break;
                case 3:
                    return false;
            }
            index <<= 2;
            if (sum > result)
                return false;
        }
        return sum == result;
    }
}