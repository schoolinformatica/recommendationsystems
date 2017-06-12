using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using DynamicMatrix;

namespace Assignment2
{
    public class Predicter
    {
        private DynamicMatrix<double> _ratings = new DynamicMatrix<double>();
        private DynamicMatrix<Deviation> _deviations = new DynamicMatrix<Deviation>();
        private Dictionary<int, HashSet<int>> _ratedBy = new Dictionary<int, HashSet<int>>();
        private HashSet<int> _articles = new HashSet<int>();

        public Predicter(IEnumerable<UserRating> ratings)
        {
            foreach (var rating in ratings)
            {
                _ratings[rating.UserId, rating.ArticleId] = rating.Rating;
                _articles.Add(rating.ArticleId);
                
                if(!_ratedBy.ContainsKey(rating.ArticleId))
                    _ratedBy[rating.ArticleId] = new HashSet<int>();

                _ratedBy[rating.ArticleId].Add(rating.UserId);
            }
            
            ComputeAllDeviations();
        }

        public Dictionary<int, double> PredictMissingRatings(int user)
        {
            var predictedRatings = new Dictionary<int, double>();
            var articlesRatedByUser = _ratings[user];
            
            foreach (var article in _articles)
            {
                if(articlesRatedByUser.ContainsKey(article)) continue;

                predictedRatings[article] = PredictRating(user, article);
            }

            return predictedRatings;
        }

        public void UpdateRating(int user, int article, double rating)
        {
            _ratings[user, article] = rating;
            UpdateDeviations(user, article);
        }

        private void UpdateDeviations(int user, int article)
        {
            foreach (var rating in _ratings[user])
            {
                if(rating.Key == article) continue;
                var deviation = _ratings[user, article] - _ratings[user, rating.Key];
                
                _deviations[article, rating.Key].Update(deviation);
                _deviations[rating.Key, article].Update(-deviation);
            }
        }

        private double PredictRating(int user, int articleId)
        {
            var numerator = 0.0;
            var denominator = 0.0;

            foreach (var rating in _ratings[user])
            {
                var deviation = _deviations[articleId, rating.Key];
                numerator += (rating.Value + deviation.Value) * deviation.Cardinality;
                denominator += deviation.Cardinality;
            }

            return numerator / denominator;
        }

        private void ComputeAllDeviations()
        {
            foreach (var articleA in _articles)
            {
                foreach (var articleB in _articles)
                {
                    if (articleA == articleB)
                    {
                        _deviations[articleA, articleB] = new Deviation(0, 0);
                        _deviations[articleB, articleA] = new Deviation(0, 0);
                        break;
                    }
                    
                    var deviation = ComputeDeviation(articleA, articleB);
                    _deviations[articleA, articleB] = deviation;
                    _deviations[articleB, articleA] = new Deviation(-deviation.Value, deviation.Cardinality);
                }
            }
        }

        private Deviation ComputeDeviation(int articleA, int articleB)
        {
            var deviation = new Deviation(0, 0);
            
            var usersRatedA = _ratedBy[articleA];
            var usersRatedB = _ratedBy[articleB];

            foreach (var user in usersRatedA)
            {
                if (!usersRatedB.Contains(user)) continue;
                
                deviation.Update(_ratings[user, articleA] - _ratings[user, articleB]);    
            }

            return deviation;
        }
    }
}