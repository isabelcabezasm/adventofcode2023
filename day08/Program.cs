using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        string[][] matrix = File.ReadLines("input.txt").Select(s=>s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)).ToArray();
        // remove ')'  ')'  and ','
        matrix = matrix.Select(row => row.Select(s => s.Replace("(", "").Replace(")", "").Replace(",","")).ToArray()).ToArray();
        
        // Console.WriteLine(TotalSteps(matrix, part:1));
        Console.WriteLine(TotalSteps(matrix, part:2));
    }

    private static long TotalSteps(string[][] matrix, int part=1)
    {
        var instructions = matrix[0][0];
        
        string[][] map = matrix.Skip(2).ToArray();

        if(part == 1)
        {
            return GetNumSteps(instructions, map, "AAA");
        }            
        else
        {
            string[] allFirstSteps = GetFirstSteps(map);
            List<long> allNumSteps = new List<long>();
            foreach (var firstStep in allFirstSteps)
            {
                allNumSteps.Add(GetNumSteps(instructions, map, firstStep, part:2));                
            }
            
            return MinimunCommunMultiple(allNumSteps);
        }

    }

    private static long MinimunCommunMultiple(List<long> allNumSteps)
    {
        long lcm = 1;
        foreach (var number in allNumSteps)
        {
            lcm = LCM(lcm, number);
        }
        return lcm;     
        
    }

    public static long GCD(long a, long b)
    {
        while (b != 0)
        {
            long temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    public static long LCM(long a, long b)
    {
        return (a / GCD(a, b)) * b;
    }

    private static string[] GetFirstSteps(string[][] map)
    {
        return map.Where(row => row[0][2] == 'A').Select(row => row[0]).ToArray();
    }

    private static long GetNumSteps(string instructions, string[][] map, string firstStep, int part=1)
    {
        long numSteps = 0;
        string nextStep = "";
        int index = getNewInddex(firstStep, map);
        
        do
        {
            int instructionIndex = (int)(numSteps % instructions.Length);
            char instruction = instructions[instructionIndex];
            numSteps++;


            if (instruction == 'R')
            {
                nextStep = map[index][3];
            }
            else if (instruction == 'L')
            {
                nextStep = map[index][2];
            }

            if (part==1 && nextStep != "ZZZ")
            {
                index = getNewInddex(nextStep, map);
            }
            else if (part==1 && nextStep == "ZZZ")
            {
                return numSteps;
            }
            else if (part==2 && nextStep[2] != 'Z')
            {
                index = getNewInddex(nextStep, map);
            }
            else if (part==2 && nextStep[2] == 'Z')
            {
                return numSteps;
            }


        } while (nextStep != "ZZZ");  //something big

        return -1; // this can't happen
        
    }

    private static int getNewInddex(string nextStep, string[][] map)
    {
        // select the row in map that contains the next step in the column 0
        int index = Array.FindIndex(map, row => row[0] == nextStep);
        return index;
    }
}