class Day01
{
    static void Main()
    {
        FirstPart();
        SecondPart();

    }
    static void FirstPart()
    {
        var text = File.ReadAllText(@"input.txt");
        var endOfLine = "\r\n";
        var matrix = text.Split(endOfLine);
        var sum = 0;

        for(int i=0; i<matrix.Length; i++)
        {
            char  firstNumber = '0', lastNumber = '0';            
            for(int j = 0; j< matrix[i].Length; j++)
            {   
                if(char.IsDigit(matrix[i][j]))
                {
                    firstNumber = matrix[i][j];
                    break;
                }
            }
            for(int j = matrix[i].Length-1; j>=0 ; j--)
            {                   
                if(char.IsDigit(matrix[i][j]))
                {
                    lastNumber = matrix[i][j];
                    break;
                }
            }
            var calibrationValue = int.Parse(firstNumber.ToString()+lastNumber.ToString());
            sum += calibrationValue;                  
        }
        Console.WriteLine(sum);
        // 54968
    }

    static void SecondPart()
    {
        var text = File.ReadAllText(@"input.txt");
        var endOfLine = "\r\n";
        var matrix = text.Split(endOfLine);
        string[] numbers = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

        var sum = 0;

        for(int i=0; i<matrix.Length; i++) // For each row in the text
        {
            string  firstNumber = "0", lastNumber = "0";            

            for(int j = 0; j< matrix[i].Length; j++)
            {   
                if(char.IsDigit(matrix[i][j]))
                {
                    firstNumber = matrix[i][j].ToString();
                    break;
                }
                else
                {
                    foreach(var number in numbers)
                    {
                        if(matrix[i].Contains(number) && matrix[i].IndexOf(number) == j)
                        {
                           firstNumber = (numbers.ToList().IndexOf(number)+1).ToString();
                           break;
                        }
                    }
                    if (firstNumber != "0")
                    {
                        break;
                    }
                }
            }
            for(int j = matrix[i].Length-1; j>=0 ; j--)
            {                   
                if(char.IsDigit(matrix[i][j]))
                {
                    lastNumber = matrix[i][j].ToString();
                    break;
                }
                else
                {
                    foreach(var number in numbers)
                    {
                        if(matrix[i].Contains(number) && matrix[i].LastIndexOf(number) == j)
                        {
                           lastNumber = (numbers.ToList().IndexOf(number)+1).ToString();
                           break;
                        }
                    }
                    if (lastNumber != "0")
                    {
                        break;
                    }

                }
            }
            var calibrationValue = int.Parse(firstNumber.ToString()+lastNumber.ToString());
            sum += calibrationValue;                  
        }
        Console.WriteLine(sum);
        // 54094
    }

}

