using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApplication
{
    public static class KNearest
    {

        public static Dictionary<int, double> GetNeighbours(Dictionary<int, GenericVector> points, int targetId, int k,
            double treshold, Func<GenericVector, GenericVector, double> similarity)
        {
            var lowest = treshold;
            var neighbours = new Dictionary<int, double>();
            var target = points[targetId];

            foreach (var point in points)
            {
                if(point.Key == targetId) continue;

                var similar = similarity(point.Value, target);
                if(!(similar > lowest && HasAdditionalItems(point.Value, target))) continue;

                if (neighbours.Count >= k)
                {

                    var smallest = Min(neighbours);

                    if (similar > smallest.Value)
                    {

                        neighbours.Remove(smallest.Key);
                        neighbours.Add(point.Key, similar);
                    }

                    lowest = Min(neighbours).Value;
                    continue;
                }

                neighbours.Add(point.Key, similar);
                Console.WriteLine("Length: " + neighbours.Count);
            }

            return neighbours;
        }

        private static KeyValuePair<int, double> Min(Dictionary<int ,double> entries)
        {
            int key = 0;
            var minValue = double.MaxValue;

            foreach (var entry in entries)
            {
                if (entry.Value < minValue)
                {
                    key = entry.Key;
                    minValue = entry.Value;
                }
            }

            return new KeyValuePair<int, double>(key, minValue);
        }

        private static bool HasAdditionalItems(GenericVector potentialNeighbour, GenericVector target)
        {
            for (int i = 0; i < target.Dimensions; i++)
            {
                if (float.IsNaN(target[i]) && !float.IsNaN(potentialNeighbour[i]))
                    return true;
            }
            return false;
        }
    }
}