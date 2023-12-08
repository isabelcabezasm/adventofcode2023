using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using Microsoft.Win32.SafeHandles;

class Program
{
    static void Main()
    {
        string[][] matrix = File.ReadLines("test.txt").Select(s=>s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)).ToArray();
        

        // Console.WriteLine(MinLocationPart1(matrix, 2));

        Console.WriteLine(MinLocationPart2(matrix));

    }

    private static long MinLocationPart2(string[][] matrix)
    {
        string[] maps = new string[] { "seed-to-soil", "soil-to-fertilizer", "fertilizer-to-water", 
                                       "water-to-light", "light-to-temperature", "temperature-to-humidity", 
                                       "humidity-to-location" };

        List<Range> seedRange = InizializeSeeds(matrix);

        foreach (var map in maps)
        {
            Console.WriteLine(map);
            List<(Range, Range)> seedToSoil = seedToSoilMap(matrix, map);
            List<Range> soilRange = new List<Range>();
            
            foreach(var sr in seedRange)
            {
                int rangeOfOrigin = getRange(sr.from, seedToSoil);
                int rangeOfDestination = getRange(sr.to, seedToSoil);

                if(rangeOfOrigin == -1 && rangeOfDestination == -1)  // both are out of range
                {   
                    // add a new range to keep following the interval.
                    soilRange.Add(new Range(sr.from, sr.to));

                } else if(rangeOfOrigin != -1 &&  rangeOfDestination == -1) // only destination is out of range.
                {
                    // add a new range from destination(sr.from) to destination(end-of-range)
                    soilRange.Add(new Range(getDestination(sr.from,seedToSoil), seedToSoil[rangeOfOrigin].Item2.to));

                    // and add another range from  end-of-range+1 to sr.to
                    soilRange.Add(new Range(seedToSoil[rangeOfOrigin].Item1.to+1, sr.to));

                }                
                else if(rangeOfOrigin == rangeOfDestination)
                {
                    soilRange.Add(new Range(getDestination(sr.from, seedToSoil), getDestination(sr.to, seedToSoil)));
                    // we don't need to add more ranges
                }
                else
                {
                    // we need to add more ranges, at least one. 
                    // 
                    // also we need to check if there are more ranges in destination. 
                }
            }
            seedRange = soilRange;

        }


        

        

       



        return long.MinValue;
    }

    private static long getDestination(long seed, List<(Range, Range)> seedToSoil)
    {
        // Look for the range:
        foreach (var item in seedToSoil)
        {
            if (item.Item1.from<= seed &&  item.Item1.to  >= seed) //seed it is in this interval.
            {
                var difference = seed - item.Item1.from;
                return item.Item2.from + difference;
            }
        }
        return seed; // Not found 
    }

    private static int getRange(long from, List<(Range, Range)> seedToSoil)
    {
        for(int i = 0; i < seedToSoil.Count; i++)
        {
            if(seedToSoil[i].Item1.from <= from && seedToSoil[i].Item1.to >= from)
            {
                return i;
            }
        }

        return -1; // not found        
    }

    private static List<(Range, Range)> seedToSoilMap(string[][] matrix, string map)
    {   
        List<(Range, Range)> seedToSoil = new List<(Range, Range)>();

        // Look for the range of the seed in the seed-to-soil map:
        Dictionary<long, (long, long)> seedToSoilDict = GetMapSection(matrix, map);

        foreach (var item in seedToSoilDict)
        {
            seedToSoil.Add((new Range(item.Value.Item1, item.Value.Item1 + item.Value.Item2 - 1), new Range(item.Key, item.Key + item.Value.Item2 - 1)));
        }

        return seedToSoil;
    }

    private static List<Range> InizializeSeeds(string[][] matrix)
    {
        List<Range> seedRange = new List<Range>();
        var seeds = matrix[0].Skip(1).ToArray();
        for (int i = 0; i < seeds.Length; i = i + 2)
        {
            long from = long.Parse(seeds[i]);
            long range = long.Parse(seeds[i + 1]);
            long to = from + range - 1;

            seedRange.Add(new Range(from, to));
        }
        return seedRange;
    }

    class Range 
    {
        public long from;
        public long to;
        public Range(long from, long to)
        {
            this.from = from;
            this.to = to;
        }
    }

    private static long MinLocationPart1(string[][] matrix, int part=1)
    {

        List<(long, long)> conversions = new List<(long, long)>();
        var seeds = matrix[0].Skip(1).ToArray();

        if(part == 2)
        {
            seeds = takeSeedsFromPairs(seeds);
        }

        Console.WriteLine("Number of seeds: " + seeds.Length);

        // Look for the range of the seed in the seed-to-soil map:
        Dictionary<long, (long, long)> seedToSoil = GetMapSection(matrix, "seed-to-soil");
        // Look for the range of the seed in the soil-to-fertilizer map:
        Dictionary<long, (long, long)> soilToFertilizer = GetMapSection(matrix, "soil-to-fertilizer");
        // Look for the range of the seed in the fertilizer-to-water map:
        Dictionary<long, (long, long)> fertilizerToWater = GetMapSection(matrix, "fertilizer-to-water");
        // Look for the range of the seed in the water-to-light map:
        Dictionary<long, (long, long)> waterToLight = GetMapSection(matrix, "water-to-light");
        // Look for the range of the seed in the light-to-temperature map:
        Dictionary<long, (long, long)> lightToTemperature = GetMapSection(matrix, "light-to-temperature");
        // Look for the range of the seed in the temperature-to-humidity map:
        Dictionary<long, (long, long)> temperatureToHumidity = GetMapSection(matrix, "temperature-to-humidity");   
        // Look for the range of the seed in the humidity-to-location map:
        Dictionary<long, (long, long)> humidityToLocation = GetMapSection(matrix, "humidity-to-location");
            
        foreach(var seed in seeds)
        {
            
            long soil = FindDestination(seedToSoil, long.Parse(seed));           
            long fertilizer = FindDestination(soilToFertilizer, soil);            
            long water = FindDestination(fertilizerToWater, fertilizer);            
            long light = FindDestination(waterToLight, water);
            long temperature = FindDestination(lightToTemperature, light);            
            long humidity = FindDestination(temperatureToHumidity, temperature);
            long location = FindDestination(humidityToLocation, humidity);
            conversions.Add((long.Parse(seed), location));
        }
        long[] locations = conversions.Select(x => x.Item2).ToArray();
        return  locations.Min();
    }

    private static string[] takeSeedsFromPairs(string[] seeds)
    {
        List<string> newSeedsList = new List<string>();
        for(int i = 0; i < seeds.Length; i=i+2)
        {
            long from = long.Parse(seeds[i]);
            long range = long.Parse(seeds[i+1]);
            long to = from + range;

            for(long j = from; j < to; j++)
            {
                newSeedsList.Add(j.ToString());
            }
        }

        return newSeedsList.ToArray();
    }

    private static long FindDestination(Dictionary<long, (long, long)> seedToSoil, long source)
    {
        // Look for the range:
        foreach (var item in seedToSoil)
        {
            if (item.Value.Item1 <= source &&  item.Value.Item1 + item.Value.Item2 >= source)
            {
                var difference = source - item.Value.Item1;
                return item.Key + difference;
            }
        }
        return source; // Not found 
    }

    private static Dictionary<long, (long, long)> GetMapSection(string[][] matrix, string nameOfSection)
    {
        var indexes = matrix
                        .SelectMany((row, i) => row.Select((element, j) => new { element, i, j }))
                        .Where(x => x.element == nameOfSection)
                        .Select(x => (x.i, x.j))
                        .ToList();

        Dictionary<long, (long, long)> dict = new Dictionary<long, (long, long)>();

        for (long i = indexes[0].i + 1; i < matrix.Length; i++)
        {
            if (matrix[i].Length == 0) break;
            dict.Add(long.Parse(matrix[i][0]), (long.Parse(matrix[i][1]), long.Parse(matrix[i][2])));
        }

        return dict;
    }
}

