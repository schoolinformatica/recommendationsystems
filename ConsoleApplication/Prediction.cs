using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApplication
{
    public static class Prediction
    {
        public static double PredictRating(Matrix<float> matrix, Dictionary<int, double> neighbours, int column, int neighbourTreshold = 0)
        {
            var sigmaNominator = 0.0;
            var sigmaDenominator = 0.0;
            var index = 0;

            foreach (var neighbour in neighbours)
            {
                if (float.IsNaN(matrix[neighbour.Key, column])) continue;
//                Console.WriteLine($"{neighbour.Value} * {matrix[neighbour.Key, column]}");
                sigmaNominator += neighbour.Value * matrix[neighbour.Key, column];
                sigmaDenominator += neighbour.Value;
                index++;
            }

            if (index < neighbourTreshold)
                return -1;
            return sigmaNominator / sigmaDenominator;
        }
    }
}