
using System.Collections.Specialized;
using System.Runtime.CompilerServices;

namespace adlordy;

public class Problem15 : ProblemBase{
    string sample = 
"""
########
#..O.O.#
##@.O..#
#...O..#
#.#.O..#
#...O..#
#......#
########

<^^>>>vv<v>>v<<
""";

    public async Task<long> SolveA(){
        var reader = await GetReader(15);
        var map = await Map.Parse(reader);
        string? line;
        while(!String.IsNullOrEmpty(line = await reader.ReadLineAsync())){
            var moves = line.ToCharArray().Select(c => ParseDirection(c));
            foreach(var move in moves){
                if (map.Robot.CanMove(move)){
                    map.Robot.Move(move);
                }
            }
        }
        return map.GetScore();
    }

    private Direction ParseDirection(char c) => c switch
    {
        '^' => Direction.Up,
        'v' => Direction.Down,
        '>' => Direction.Right,
        '<' => Direction.Left,
        _ => throw new ArgumentOutOfRangeException(),
    };

    enum Direction {
        Right,
        Down,
        Left,
        Up
    }

    class Map {
        private List<Cell[]> _lines;
        private Robot _robot;

        public static async Task<Map> Parse(TextReader reader){
            var map = new Map();
            var lines = new List<Cell[]>();
            string? line;
            while(!string.IsNullOrWhiteSpace(line = await reader.ReadLineAsync())){
                lines.Add(line.ToCharArray().Select((c, i) => Cell.FromChar(c, new Point(lines.Count, i), map)).ToArray());
            }
            map.SetLines(lines);
            return map;
        }

        private void SetLines(List<Cell[]> lines)
        {
            _lines = lines;
        }

        public Cell this[Point p] {
            get => _lines[(int)p.X][p.Y];
            set => _lines[(int)p.X][p.Y] = value;
        } 

        public Point Get(Point pos, Direction dir)
        {
            return pos + dir switch {
                Direction.Right => new Point(0, 1),
                Direction.Down => new Point(1, 0),
                Direction.Left => new Point(0, -1),
                Direction.Up => new Point(-1, 0),
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        internal long GetScore()
        {
            var sum = 0L;
            for(var i=0;i<_lines.Count;i++){
                for(var j=0;j<_lines[i].Length;j++){
                    var cell = _lines[i][j];
                    Console.Write(cell.ToString());
                    if (cell is Box box){
                        if (box is Robot)
                            continue;
                        sum += box.Pos.X * 100 + box.Pos.Y;
                    }
                }
                Console.WriteLine();
            }
            return sum;
        }

        public Robot Robot {get;set;}
    }
    abstract class Cell(Point pos){
        public Point Pos {get; set;} = pos;

        public static Cell FromChar(char c, Point pos, Map map)
        {
            return c switch {
                '#' => new Wall(map, pos),
                'O' => new Box(map, pos),
                '@' => new Robot(map, pos),
                '.' => new Empty(map, pos),
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        public abstract bool CanMove(Direction dir);
        public abstract Cell Move(Direction dir);
    }

    class Wall(Map map, Point pos) : Cell(pos){
        public override bool CanMove(Direction dir) => false;

        public override Cell Move(Direction dir) => throw new InvalidOperationException();

        public override string ToString() => "#";
    }

    class Box(Map map, Point pos) : Cell(pos){
        public override bool CanMove(Direction dir) {
            Point next = map.Get(Pos, dir);
            return map[next].CanMove(dir);
        }

        public override Cell Move(Direction dir) {
            var next = map.Get(Pos, dir);
            var moved = map[next].Move(dir);
            map[next] = this;
            map[Pos] = moved;
            moved.Pos = Pos;
            Pos = next;
            return moved;
        }

        public override string ToString() => "O";
    }

    class Empty(Map map, Point pos) : Cell(pos)
    {
        public override bool CanMove(Direction dir) => true;

        public override Cell Move(Direction dir){
            return this;
        }

        public override string ToString() => ".";
    }

    class Robot : Box{
        public Robot(Map map, Point pos) : base(map, pos){
            map.Robot = this;
        }

        public override string ToString() => "@";
    }
}