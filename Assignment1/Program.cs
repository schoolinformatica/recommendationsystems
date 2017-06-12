using System;
using System.Collections.Generic;
using System.Linq;
using Matrix;
using CsvReader;

namespace ConsoleApplication
{
    internal class Program
    {
        private static Matrix<float> _matrixSmall;


        public static void Main(string[] args)
        {
            var usersDistinct = new HashSet<int>();
            var articlesDistinct = new HashSet<int>();

            var userPreferences = CsvReader<UserPref>.MapFromFile("userItem.data", ',', UserPref.Map);

            for (var i = 0; i < userPreferences.Count; i++)
            {
                var userPref = userPreferences[i];
                usersDistinct.Add(userPref.Id);
                articlesDistinct.Add(userPref.ArticleId);
            }

            _matrixSmall = new Matrix<float>(usersDistinct.Count, articlesDistinct.Count);
            _matrixSmall.ReplaceDefault(float.NaN);


            foreach (var pref in userPreferences)
            {
                _matrixSmall[pref.Id, pref.ArticleId] = pref.Rating;
            }

            // get neighbours of user 7
            var neighboursSeven = KNearest.GetNeighbours(_matrixSmall, 7, 3, 0.35, GenericVector.PearsonCoefficient);

            Console.WriteLine("### Neighbours of 7 (Pearson): ");
            foreach (var neighbour in neighboursSeven)
            {
                Console.WriteLine("Neighbour: " + neighbour.Key + " with similarity of: " + neighbour.Value);
            }
            Console.WriteLine("\n");
            Console.WriteLine("### Neighbours of 4 (Pearson): ");
            var neighboursFour = KNearest.GetNeighbours(_matrixSmall, 4, 3, 0.35, GenericVector.PearsonCoefficient);

            foreach (var neighbour in neighboursFour)
            {
                Console.WriteLine("Neighbour: " + neighbour.Key + " with similarity of: " + neighbour.Value);
            }

            Console.WriteLine("\n");
            Console.WriteLine("### Predicted ratings 7: ");
            Console.WriteLine("Predicted 101: " + Prediction.PredictRating(_matrixSmall, neighboursSeven, 101));
            Console.WriteLine("Predicted 103: " + Prediction.PredictRating(_matrixSmall, neighboursSeven, 103));
            Console.WriteLine("Predicted 106: " + Prediction.PredictRating(_matrixSmall, neighboursSeven, 106));

            Console.WriteLine("\n");
            Console.WriteLine("### Predicted ratings 4: ");
            Console.WriteLine("Predicted 101: " + Prediction.PredictRating(_matrixSmall, neighboursFour, 101));

            _matrixSmall[7, 106] = 2.8f;

            var neighboursAfterUpdate = KNearest.GetNeighbours(_matrixSmall, 7, 3, 0.35, GenericVector.PearsonCoefficient);
            Console.WriteLine("### Neighbours of 7 after update 2.8 (Pearson): ");
            foreach (var neighbour in neighboursAfterUpdate)
            {
                Console.WriteLine("Neighbour: " + neighbour.Key + " with similarity of: " + neighbour.Value);
            }

            Console.WriteLine("\n");
            Console.WriteLine("### Predicted ratings 7 after update 2.8: ");
            Console.WriteLine("Predicted 101: " + Prediction.PredictRating(_matrixSmall, neighboursAfterUpdate, 101));
            Console.WriteLine("Predicted 103: " + Prediction.PredictRating(_matrixSmall, neighboursAfterUpdate, 103));


            _matrixSmall[7, 106] = 5f;

            var neighboursAfterSecondUpdate = KNearest.GetNeighbours(_matrixSmall, 7, 3, 0.35, GenericVector.PearsonCoefficient);
            Console.WriteLine("\n");
            Console.WriteLine("### Neighbours of 7 after update 5 (Pearson): ");
            foreach (var neighbour in neighboursAfterSecondUpdate)
            {
                Console.WriteLine("Neighbour: " + neighbour.Key + " with similarity of: " + neighbour.Value);
            }

            Console.WriteLine("\n");
            Console.WriteLine("### Predicted ratings 7 after update 5: ");
            Console.WriteLine("Predicted 101: " + Prediction.PredictRating(_matrixSmall, neighboursAfterSecondUpdate, 101));
            Console.WriteLine("Predicted 103: " + Prediction.PredictRating(_matrixSmall, neighboursAfterSecondUpdate, 103));

            var usersBig = new HashSet<int>();
            var articlesBig = new HashSet<int>();
            var userPreferencesBig = CsvReader<UserPref>.MapFromFile("u.data", '\t', UserPref.Map);

            for (var i = 0; i < userPreferencesBig.Count; i++)
            {
                var userPref = userPreferencesBig[i];
                usersBig.Add(userPref.Id);
                articlesBig.Add(userPref.ArticleId);
            }

            var matrixBig = new Matrix<float>(usersBig.Count, articlesBig.Count);

            matrixBig.ReplaceDefault(float.NaN);
            foreach (var pref in userPreferencesBig)
            {
                matrixBig[pref.Id, pref.ArticleId] = pref.Rating;
            }

            var recom = GetRecommendations(matrixBig, 186, articlesBig);
            var bestEight = recom.OrderByDescending(x => x.Value).Take(8);
            foreach (var rating in bestEight)
            {
                Console.WriteLine($"Movie Id: {rating.Key} with rating {rating.Value}");
            }

        }

        public static Dictionary<int, double> GetRecommendations(Matrix<float> matrix, int user, HashSet<int> articles)
        {
            var recommendations = new Dictionary<int, double>();

            var neighbours = KNearest.GetNeighbours(matrix, user, 25, 0.35, GenericVector.PearsonCoefficient);

            foreach (var article in articles)
            {
                if(!float.IsNaN(matrix[user, article])) continue;
                var rating = Prediction.PredictRating(matrix, neighbours, article, 3);
                if (rating >= 0)
                {
                    recommendations.Add(article, rating);
                }
            }
            return recommendations;
        }
    }
}