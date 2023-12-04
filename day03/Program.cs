using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        var matrix = File.ReadLines("input.txt").ToArray();
        Console.WriteLine(SumOfPartNumbers(matrix));
        // var sum = PossibleGames(games);
        // Console.WriteLine(sum);
        //var multiplication = MinimumCubes(games);
        //Console.WriteLine(multiplication);
    }

    private static int SumOfPartNumbers(string[] matrix)
    {
        List<Tuple<string, Position, Position>> numbers = new List<Tuple<string, Position, Position>>();

        for (int i = 0; i < matrix.Length; i++)
        {
            var matches = Regex.Matches(matrix[i], @"\d+");
            foreach (Match match in matches)
            {
                numbers.Add(new Tuple<string, Position, Position>
                (
                    match.Value,
                    new Position(i, match.Index),
                    new Position(i, match.Index + match.Length - 1)
                ));
            }
        }
        var sum = 0;
        foreach (var number in numbers)
        {
            Console.WriteLine($"Number: {number.Item1}, Start: ({number.Item2.X}, {number.Item2.Y}), End: ({number.Item3.X}, {number.Item3.Y})");
            
            if(AdjacentSymbols(number, matrix))
            {   
                sum += int.Parse(number.Item1);
                // Console.WriteLine("Adjacent symbols");
            }
        }
        return sum;
    }

    private static bool AdjacentSymbols(Tuple<string, Position, Position> number, string[] matrix)
    {
        if(
           // if number has a symbol in the previous position,         
           (number.Item2.Y > 0 && matrix[number.Item2.X][number.Item2.Y - 1] != '.') || 
           // or if it has a symbol in the next position
           (number.Item3.Y < matrix[0].Length-1 && matrix[number.Item2.X][number.Item3.Y + 1] != '.') ||
           // or if it has a symbol in the previous row
           (number.Item2.X  > 0 && SymbolInRowAbove(number, matrix)) ||
              // or if it has a symbol in the next row
           (number.Item2.X < matrix.Length-1 && SymbolInRowBelow(number, matrix))
        )
        {
            // Console.WriteLine("Adjacent symbols");
            return true;
        }
        return false;
    }

    private static bool SymbolInRowAbove(Tuple<string, Position, Position> number, string[] matrix)
    {
        // or if it has symbol in the row above (over the number)
        // Console.WriteLine("For number: ", number.Item1);
        for(int i = number.Item2.Y-1; i <= number.Item3.Y+1; i++)
        {   
            if(i < 0 || i >= matrix[0].Length) continue;
            // Console.WriteLine($"Checking ({number.Item2.X-1}, {i})");
            // Console.WriteLine($"Value: {matrix[number.Item2.X-1][i]}");
            if(matrix[number.Item2.X-1][i] != '.')
            {
                return true;
            }
        }
        
        return false;
    }

    private static bool SymbolInRowBelow(Tuple<string, Position, Position> number, string[] matrix)
    {
        // or if it has symbol in the row below (under the number)
        for(int i = number.Item2.Y-1; i <= number.Item3.Y+1; i++)
        {   
            if(i < 0 || i > matrix.Length-1) continue;
            // Console.WriteLine($"Checking ({number.Item2.X+1}, {i})");
            // Console.WriteLine($"Value: {matrix[number.Item2.X+1][i]}");
            if(matrix[number.Item2.X+1][i] != '.')
            {
                return true;
            }
        }
        
        return false;
    }

    class Position {
        private readonly int x;
        private readonly int y;

        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int X { get { return x; } }
        public int Y { get { return y; } }
    }
}