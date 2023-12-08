using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        string[][] matrix = File.ReadLines("input.txt").Select(s=>s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)).ToArray();
        
        Console.WriteLine(CalculateDifferentWays(matrix));

    }

    private static long CalculateDifferentWays(string[][] matrix)
    {
        var times = matrix[0];
        var distances = matrix[1];        
        var multiplyNumOfWays = 1;

        for(long i = 1; i < times.Length; i++)
        {
            var time = long.Parse(times[i]);
            var distance = long.Parse(distances[i]);

            var numofWays = 0;
            long index = 0;

            if(time % 2 == 0) index = time/2;
            else index = (time/2)+1;
         
            for( long j = index; j < time; j++)
            {
               if((time-j) * j > distance)
               {    
                    // Console.WriteLine(j);
                    numofWays++;
               }
               else {
                   break;
               }
            }

            if(time % 2 == 0) numofWays = (numofWays*2)-1;
            else numofWays = (numofWays*2);

            Console.WriteLine(numofWays + " for time: "+ time);
            multiplyNumOfWays *= (numofWays);
        }

        return multiplyNumOfWays;
    }
}