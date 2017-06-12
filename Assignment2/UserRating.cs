namespace Assignment2
{
    public class UserRating
    {
        

        public int UserId { get; }
        public int ArticleId { get;  }
        public double Rating  { get; }
        
        public UserRating(int id, int articleId, double rating)
        {
            UserId = id;
            ArticleId = articleId;
            Rating = rating;
        }

        public static UserRating Map(string[] columns)
        {
            var id = int.Parse(columns[0]);
            var articleId = int.Parse(columns[1]);
            var rating = double.Parse(columns[2]);
            
            return new UserRating(id, articleId, rating);
        }
    }
}