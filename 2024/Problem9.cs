
namespace adlordy;

public class Problem9 : ProblemBase {

    private async Task<(int[],byte[],byte[])> Load(){
        var content = await GetContent(9);
        content = content[..^1];
        List<byte> files = [];
        List<byte> spare = [];
        var total = 0L;
        for(var i=0;i<content.Length;i++){
            int c = content[i] - '0';
            if (i % 2 == 0){
                files.Add((byte)c);
            } else {
                spare.Add((byte)c);
            }
            total += c;
        }
        spare.Add(0);
        var buffer = new int[total];
        buffer.AsSpan().Fill(-1);
        var start = 0;
        for(var i=0;i<files.Count;i++){
            buffer.AsSpan(start, files[i]).Fill(i);
            start += files[i] + spare[i];
        }
        return (buffer, files.ToArray(), spare.ToArray());
    }

    public async Task<long> SolveA()
    {
        var (buffer, _, _) = await Load();
        var start = 0;
        var end = buffer.Length - 1;
        while (start < end)
        {
            while (buffer[start] != -1 && start < end)
                start++;
            while (buffer[end] == -1 && start < end)
                end--;
            buffer[start] = buffer[end];
            buffer[end] = -1;
        }

        return Checksum(buffer);
    }

    private static long Checksum(int[] buffer)
    {
        var checksum = 0L;
        for (var i = 0; i < buffer.Length; i++)
        {
            if (buffer[i] != -1)
                checksum += buffer[i] * i;
        }
        return checksum;
    }

    public async Task<long> SolveB(){
        var content = await GetContent(9);
        content = content[..^1];
        var list = new LinkedList<Space>();
        var index = 0;
        for(var i=0;i<content.Length;i++){
            var c = content[i] - '0';
            if (i % 2 == 0){
                list.AddLast(new Space(index++, (byte)c));
            } else {
                list.AddLast(new Space(-1, (byte)c));
            }
        }

        var right = list.Last;
        while(right != null) {
            if (right.Value.Index > -1) {
                var left = list.First;
                while(left != null && left != right){
                    if (left.Value.Index == -1) {
                        if (left.Value.Length >= right.Value.Length){
                            var newRight = list.AddAfter(right, new Space(-1, right.Value.Length));
                            list.Remove(right);
                            list.AddBefore(left, right);
                            var newSize = (byte)(left.Value.Length - right.Value.Length);
                            if (newSize > 0)
                                list.AddAfter(left, new Space(-1, newSize));
                            list.Remove(left);
                            right = newRight;
                            break;
                        }
                    }
                    left = left.Next;
                }
            }
            right = right.Previous;
        }

        return Checksum(list);
    }

    private long Checksum(LinkedList<Space> list)
    {
        var checksum = 0L;
        var index = 0;
        var left = list.First;
        while(left != null){
            if (left.Value.Index > -1){
                for(var i = 0;i<left.Value.Length;i++){
                    checksum += left.Value.Index * (index+i);
                }
            }
            index += left.Value.Length;
            left = left.Next;
        }
        return checksum;
    }
}

public record Space(int Index, byte Length);