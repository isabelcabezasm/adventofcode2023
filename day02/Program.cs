using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        var games = File.ReadLines("input.txt");
        // var sum = PossibleGames(games);
        // Console.WriteLine(sum);
        var multiplication = MinimumCubes(games);
        Console.WriteLine(multiplication);
    }

    private static long MinimumCubes(IEnumerable<string> games)
    {
        int sum = 0;
        foreach (var game in games)
        {
            var colorDict = new Dictionary<string, List<int>> { { "red", new List<int>() }, { "green", new List<int>() }, { "blue", new List<int>() } };
            var gameId = int.Parse(game.Split(':')[0].Split(' ')[1]);
            var balls = game.Split(':')[1];

            var extractions = balls.Split(';');
            parseGame(colorDict, extractions);

            // take maximum of each color
            var redMax = colorDict["red"].Max();
            var greenMax = colorDict["green"].Max();
            var blueMax = colorDict["blue"].Max();

            sum += (redMax * greenMax * blueMax);            
        }
        return sum;
    }

    static int PossibleGames(IEnumerable<string> games)
    {
        int sum = 0;
        

        foreach (var game in games)
        {
            var colorDict = new Dictionary<string, List<int>> { { "red", new List<int>() }, { "green", new List<int>() }, { "blue", new List<int>() } };
            var gameId = int.Parse(game.Split(':')[0].Split(' ')[1]);
            var balls = game.Split(':')[1];

            var extractions = balls.Split(';');
            parseGame(colorDict, extractions);
            
            // only 12 red cubes, 13 green cubes, and 14 blue cubes
            // if any of the numbers in the list colorDict["red"] is greater than 10, it's not a possible game
            if(colorDict["red"].Any(x => x > 12)|| colorDict["green"].Any(x => x > 13) || colorDict["blue"].Any(x => x > 14))
            {
                Console.WriteLine($"Game {gameId} is not possible!");
            }
            else

            {
                Console.WriteLine($"Game {gameId} is possible!");
                sum += gameId;
            }

        }

        return sum;

    }

    private static void parseGame(Dictionary<string, List<int>> colorDict, string[] games)
    {
        foreach (var game in games)
        {
            var colors = game.Split(',');
            foreach (var color in colors)
            {
                var parts = color.Trim().Split(' ');
                var number = int.Parse(parts[0]);
                var colorName = parts[1];
                colorDict[colorName].Add(number);
            }
        }
    }
}