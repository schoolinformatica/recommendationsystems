using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace ConsoleApplication
{
    internal class Program
    {
        private static Matrix<float> _matrix = new Matrix<float>(7, 6);

        public static void Main(string[] args)
        {
            _matrix.ReplaceDefault(float.NaN);
            var userPreferences = CsvReader<UserPref>.MapFromFile("userItem.data", ',', UserPref.Map);

            foreach (var pref in userPreferences)
            {
                _matrix[pref.Id, pref.ArticleId] = pref.Rating;
            }

            var users = new Dictionary<int, GenericVector>();
            for (var i = 1; i <= 7; i++)
            {
                users.Add(i, new GenericVector(_matrix[i]));
            }

            var neighbours = KNearest.GetNeighbours(users, 7, 3, 0.35, GenericVector.CosineSimilarity);
            foreach (var neighbour in neighbours)
            {
                Console.WriteLine("Neighbour: " + neighbour.Key + " with similarity of: " + neighbour.Value);
            }

            var a = new GenericVector(5, float.NaN, 1, 3, 5);
            var b = new GenericVector(2, 5, 4, 3, float.NaN);
            Console.WriteLine(GenericVector.CosineSimilarity(a, b));
        }
    }
}