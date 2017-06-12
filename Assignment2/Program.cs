using System;
using System.Collections.Generic;
using System.Linq;
using CsvReader;

namespace Assignment2
{
    internal class Program
    {
        private const string PathSmallData = "userItem.data";
        private const string PathBigData = "u.data";
        
        
        public static void Main(string[] args)
        {
            var smallData = CsvReader<UserRating>.MapFromFile(PathSmallData, ',', UserRating.Map);
            var bigData = CsvReader<UserRating>.MapFromFile(PathBigData, '\t', UserRating.Map);
            var smallPredicter = new Predicter(smallData);
            var bigPredicter = new Predicter(bigData);
            
            Console.WriteLine("### Predicted ratings for user 7 ###");
            foreach (var predictedRating in smallPredicter.PredictMissingRatings(7))
            {
                 Console.WriteLine($"Predicted rating for {predictedRating.Key} is {predictedRating.Value}");   
            }
            
            Console.WriteLine("### Predicted ratings for user 3 ###");
            foreach (var predictedRating in smallPredicter.PredictMissingRatings(3))
            {
                Console.WriteLine($"Predicted rating for {predictedRating.Key} is {predictedRating.Value}");   
            }
            
            smallPredicter.UpdateRating(3, 105, 4.0);
            Console.WriteLine("### Predicted ratings for user 7 after update ###");
            foreach (var predictedRating in smallPredicter.PredictMissingRatings(7))
            {
                Console.WriteLine($"Predicted rating for {predictedRating.Key} is {predictedRating.Value}");   
            }
            
            Console.WriteLine("### Predicted ratings for user 186 ###");
            foreach (var predictedRating in bigPredicter.PredictMissingRatings(186).OrderByDescending(x => x.Value).Take(5))
            {
                Console.WriteLine($"Predicted rating for {predictedRating.Key} is {predictedRating.Value}");   
            }

        }
    }
}