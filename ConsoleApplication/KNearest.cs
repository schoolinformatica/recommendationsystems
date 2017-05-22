using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApplication
{
    public static class KNearest
    {

        public static Dictionary<int, double> GetNeighbours(Matrix<float> points, int targetId, int k,
            double treshold, Func<GenericVector, GenericVector, double> similarity)
        {
            var lowest = treshold;
            var neighbours = new Dictionary<int, double>();
            var target = new GenericVector(points[targetId]);

            foreach (var point in points)
            {
//                Console.WriteLine($"point: {point.Key} with target {targetId}");
                if(point.Key == targetId) continue;

                var vector = new GenericVector(point.Value);

                var similar = similarity(vector, target);
                if(!(similar > lowest && HasAdditionalItems(vector, target))) continue;

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