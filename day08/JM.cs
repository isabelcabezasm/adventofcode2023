// ﻿var lines = File.ReadAllLines("input.txt");

// var moves = lines[0];
// var map = lines[2..].Select(l => new { p = l[..3], left = l[7..10], right = l[12..15] }).ToDictionary(x => x.p, x => (x.left, x.right));

// var p = "AAA";
// int steps = 0;
// do
// {
//     foreach (var c in moves)
//     {
//         steps++;
//         p = c == 'R' ? map[p].right : map[p].left;
//     }
// }
// while (p != "ZZZ");

// Console.WriteLine(steps);