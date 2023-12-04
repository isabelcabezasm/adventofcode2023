using System.Linq;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        var matrix = File.ReadLines("input.txt");
        Console.WriteLine(SumOfPoints(matrix));
        Console.WriteLine(SumOfScratchcards (matrix));
    }

    private static int SumOfScratchcards(IEnumerable<string> matrix)
    {
        Dictionary<int, int> numberOfWinningNumbersInScratchcards = winningNumbersByCard(matrix);
        Dictionary<int, int> numberOfCards = inicializeDictionary(matrix.Count());  //at the beggining we have 1 card of each game

        for(int i= 1; i<= matrix.Count(); i++)
        {
            var numOfWinningNumbers = numberOfWinningNumbersInScratchcards[i];
            var numOfCards = numberOfCards[i];

            for(int j = 1; j <= numOfWinningNumbers; j++)
            {
                numberOfCards[i+j] = numberOfCards[i+j] + numOfCards;
            }
        }
        return numberOfCards.Values.Sum();
    }

    private static Dictionary<int, int> inicializeDictionary(int numberOfCards)
    {
        Dictionary<int, int> dict = new Dictionary<int, int>();
        for(int i = 1; i <= numberOfCards; i++){
            dict[i] = 1;
        }
        return dict;
    }

    private static Dictionary<int, int> winningNumbersByCard(IEnumerable<string> matrix)
    {
        Dictionary<int, int> dict = new Dictionary<int, int>();

        foreach (var game in matrix)
        {
            var games = game.Split(':');
            var idGame = int.Parse(Regex.Replace(games[0], @"\s+", " ").Split(' ')[1]);

            var winningNumbers = games[1].Split('|')[0].Trim().Replace("  ", " ").Split(' ').Select(int.Parse).ToList();
            var numberYouHave = games[1].Split('|')[1].Trim().Replace("  ", " ").Split(' ').Select(int.Parse).ToList();
            var winningNumbersYouHave = winningNumbers.Intersect(numberYouHave).ToList();
            dict[idGame] = winningNumbersYouHave.Count;
        }

        return dict;
    }

    private static double SumOfPoints(IEnumerable<string> matrix)
    {
        var sum = 0.0;
        foreach (var line in matrix)
        {
            var games = line.Split(':');
            var winningNumbers = games[1].Split('|')[0].Trim().Replace("  ", " ").Split(' ').Select(int.Parse).ToList();
            var numberYouHave = games[1].Split('|')[1].Trim().Replace("  ", " ").Split(' ').Select(int.Parse).ToList();
            var winningNumbersYouHave = winningNumbers.Intersect(numberYouHave).ToList();
            if(winningNumbersYouHave.Count > 0)
            {
                var pointsForThisGame = Math.Pow(2, winningNumbersYouHave.Count-1);
                sum += pointsForThisGame;
            
            }
        }
        return sum;
    }
}