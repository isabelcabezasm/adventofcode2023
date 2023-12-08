using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        string[][] matrix = File.ReadLines("input.txt").Select(s=>s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)).ToArray();
        // Console.WriteLine(TotalBet(matrix));
        Console.WriteLine(TotalBet(matrix, part:2));
    }

    private static int TotalBet(string[][] matrix, int part=1)
    {
        // list(  hand,   bet   )
        List<(string, int)> hands = new List<(string, int)>();
        matrix.ToList().ForEach(hand => hands.Add((hand[0], int.Parse(hand[1]))));

        // foreach (var hand in hands)
        // {
        //     typeOfHand2(hand.Item1);
        // }

        if(part == 1)
        {
            hands.Sort((x, y) => CompareHands(x.Item1, y.Item1));
        }
        else if(part == 2)
        {
            hands.Sort((x, y) => CompareHandsPart2(x.Item1, y.Item1));
        }
        else
        {
            Console.WriteLine("Something went wrong");
            return -1;
        }
        
        Console.WriteLine("Sorted hands:");
        hands.ForEach(hand => Console.WriteLine(hand + " - " + typeOfHand2(hand.Item1)));

        int sum = 0;
        for(int i = 0; i < hands.Count; i++)
        {
           int rank = i+1;
           sum += hands[i].Item2 * rank;
        }
        
        return sum;
    }

    private static int typeOfHand(string hand)
    {
        List<char> characters = hand.ToList();

        Dictionary<char, int> characterCounts = new Dictionary<char, int>();

        foreach (char character in characters)
        {
            if (characterCounts.ContainsKey(character))
            {
                characterCounts[character]++;
            }
            else
            {
                characterCounts[character] = 1;
            }
        }

        if(characterCounts.Count == 1)
        {
            // Console.WriteLine("Five of a kind");
            return 0; 
        }
        else if(characterCounts.Count == 2)
        {
            if(characterCounts.ContainsValue(4))
            {
                // Console.WriteLine("Four of a kind");
                return 1;
            }
            else
            {
                // Console.WriteLine("Full house");
                return 2;
            }
        }
        else if(characterCounts.Count == 3)
        {
            if(characterCounts.ContainsValue(3))
            {
                // Console.WriteLine("Three of a kind");
                return 3;
            }
            else
            {
                // Console.WriteLine("Two pair");
                return 5;
            }        
        }
        else if(characterCounts.Count == 4)
        {
            // Console.WriteLine("One pair"); 
            return 6;           
        }
        else if(characterCounts.Count == 5)
        {
            // Console.WriteLine("High card");   
            return 7;             
        }
        else
        {
            Console.WriteLine("Something went wrong");
            return -1;
        }   
    }


    private static int typeOfHand2(string hand)
    {
        List<char> characters = hand.ToList();
        Dictionary<char, int> characterCounts = new Dictionary<char, int>();
        foreach (char character in characters)
        {
            if (characterCounts.ContainsKey(character))
            {
                characterCounts[character]++;
            }
            else
            {
                characterCounts[character] = 1;
            }
        }

        if(characterCounts.ContainsKey('J'))
        {
            if(characterCounts.Count == 1)
            {
                // Console.WriteLine(hand + " - Five of a kind");
                // The same, with or without the J
                return 0; 
            }
            else if(characterCounts.Count == 2)
            {
                if(characterCounts.ContainsValue(4))  // 4 and 1
                {
                    // if J.count == 4, then it's a five of a kind
                    // if X.count == 4, then it's a five of a kind
                    // Console.WriteLine(hand + " - Five of a kind");
                    return 0;
                }
                else // 3 and 2
                {
                    // if J.count == 3, then it's a five of a kind
                    // and also if X.count == 3, then it's a five of a kind
                    // Console.WriteLine(hand + " - Five of a kind");
                    return 0;
                }
            }
            else if(characterCounts.Count == 3)
            {
                if(characterCounts.ContainsValue(3)) // 3, 1, 1
                {
                    // Console.WriteLine("Three of a kind");
                    // if j.count = 3 -> four of kind  (1)
                    // if j.count = 1 -> can be or four of kind or full house. Better Four of kind. (1)
                    return 1;
                }
                else // 2, 2, 1
                {

                    // if j.count = 2 -> Four of kind  (1)
                    if(characterCounts['J'] == 2)
                    {
                        // Console.WriteLine(hand + " - Four of a kind");
                        return 1;
                    }
                    else
                    {
                        // if j.count = 1 -> is full house (2)
                        // Console.WriteLine(hand + " - Full house");
                        return 2;
                    }
                }        
            }
            else if(characterCounts.Count == 4)
            {
                // 2, 1, 1, 1
                // if j.count = 2 -> Three of kind (3)
                // if j.count = 1 ->  or three of kind or Two pair.    Better Three of kind. (3)
                // Console.WriteLine(hand + " - Three of kind");
                return 3;           
            }
            else if(characterCounts.Count == 5)
            {
                // at least, always, one pair.
                // Console.WriteLine(hand + " - One Pair");
                return 5;             
            }
            else
            {
                Console.WriteLine("Something went wrong");
                return -1;
            }   
        }
        else 
        {
            // everything is the same. 
            return typeOfHand(hand);
        }
        
    }


    private static int valueOfCard(char card, int part=1)
    {
        char[] cardLabels= new char[] { 'A', 'K', 'Q', 'J', 'T', '9', '8', '7', '6', '5', '4', '3', '2' };    
        if (part == 2){
            cardLabels= new char[] { 'A', 'K', 'Q', 'T', '9', '8', '7', '6', '5', '4', '3', '2', 'J' };    
        }
        return Array.IndexOf(cardLabels, card);        
    }

    // This is your comparison function
    private static int CompareHands(string x, string y)
    {
        // Implement your comparison logic here
        if(typeOfHand(x)>typeOfHand(y))
        {
            // Return -1 if x < y
            return -1;
        }
        else if(typeOfHand(x)<typeOfHand(y))
        {
            return 1;
        }
        else
        {
            // we need to decide which hand is better
            // if the hands are the same type, we need to compare the cards
            for(int i = 0; i < x.Length; i++)
            {
                if(valueOfCard(x[i]) > valueOfCard(y[i]))
                {
                    // Return -1 if x < y
                    return -1;
                }
                else if(valueOfCard(x[i]) < valueOfCard(y[i]))
                {
                    // Return 1 if x > y
                    return 1;
                }
                else 
                    continue;
            }

            // it's impossible for the hands to be the same
            return 0;         // Return 0 if x == y

        }        
    }

    private static int CompareHandsPart2(string x, string y)
    {
        // Implement your comparison logic here
        if(typeOfHand2(x)>typeOfHand2(y))
        {
            // Return -1 if x < y
            return -1;
        }
        else if(typeOfHand2(x)<typeOfHand2(y))
        {
            return 1;
        }
        else
        {
            // we need to decide which hand is better
            // if the hands are the same type, we need to compare the cards
            for(int i = 0; i < x.Length; i++)
            {
                if(valueOfCard(x[i], part:2) > valueOfCard(y[i], part:2))
                {
                    // Return -1 if x < y
                    return -1;
                }
                else if(valueOfCard(x[i], part:2) < valueOfCard(y[i], part:2))
                {
                    // Return 1 if x > y
                    return 1;
                }
                else 
                    continue;
            }

            // it's impossible for the hands to be the same
            return 0;         // Return 0 if x == y

        }        
    }

}




