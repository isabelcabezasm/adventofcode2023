using System.Security;

class Program
{
    static void Main()
    {
        int[][] matrix = File.ReadLines("input.txt").Select(s => s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray()).ToArray();
        Console.WriteLine(SumOfNextInSequences(matrix, part: 1));
        Console.WriteLine(SumOfNextInSequences(matrix, part: 2));
    }

    private static int SumOfNextInSequences(int[][] matrix, int part = 1)
    {
        int sum = 0;
        foreach (var sequence in matrix)
        {
            List<List<int>> differences = new List<List<int>>
            {
                part == 1 ? sequence.ToList() : sequence.Reverse().ToList()
            };

            do
            {
                differences.Add(new List<int>());
                for (int i = differences[differences.Count - 2].Count - 1; i > 0; i--)
                {
                    if (part == 1)
                    {
                        differences[differences.Count - 1].Insert(0, differences[differences.Count - 2][i] - differences[differences.Count - 2][i - 1]);
                    }
                    else if (part == 2)
                    {
                        differences[differences.Count - 1].Insert(0, differences[differences.Count - 2][i - 1] - differences[differences.Count - 2][i]);
                    }

                }
            }
            // if all the numbers are NOT the same.
            while (!differences[differences.Count - 1].All(x => x == differences[differences.Count - 1][0]));

            // Now, we can calculate the next one in the sequence. 
            // from the bottom: differences[differences.Count - 2]
            for (int i = differences.Count - 2; i >= 0; i--)
            {
                if (part == 1)
                {
                    differences[i].Add(differences[i].Last() + differences[i + 1].Last());
                }
                else if (part == 2)
                {
                    differences[i].Add(differences[i].Last() - differences[i + 1].Last());
                }
            }
            sum += differences[0].Last();
        }
        return sum;

    }

}