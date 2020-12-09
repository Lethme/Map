using System;
using System.Collections.Generic;

namespace Map
{
    class Program
    {
        static void Main(string[] args)
        {
            var map = Map.Create
            (
                MapItem.Create
                (
                    6,
                    "khhfghflkglkfg",
                    "jdlkghjldg",
                    "dghdgjdgjdhjdhj"
                ),
                MapItem.Create
                (
                    2,
                    "test",
                    "test1"
                )
            );

            Console.WriteLine($"{map}\n");
            map += (7, "test");
            Console.WriteLine($"{map}\n");
        }
    }
}
