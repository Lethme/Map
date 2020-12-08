using System;

namespace Map
{
    class Program
    {
        static void Main(string[] args)
        {
            var map = Map.Create
            (
                MapItem.Create(
                    5,
                    "Nico",
                    "Ded",
                    "Shvabra"
                ),
                MapItem.Create(
                    3,
                    "dfgjkd",
                    "dkdjghgfh",
                    "Nico"
                )
            );

            Console.WriteLine($"\n{map}\n");

            map.SortItems(SortType.Ascending);

            Console.WriteLine($"\n{map}\n");

            map.SortData(SortType.Descending);

            Console.WriteLine($"\n{map}\n");
        }
    }
}
