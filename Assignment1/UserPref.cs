using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApplication
{
    public class UserPref
    {

        public int Id { get; }
        public int ArticleId { get; }
        public float Rating { get; }

        public UserPref(int id, int articleId, float rating)
        {
            Id = id;
            ArticleId = articleId;
            Rating = rating;
        }

        public override string ToString()
        {
            return $"Id = {Id}, ArticleId = {ArticleId}, Rating = {Rating}";
        }

        public static UserPref Map(string[] columns)
        {
            var id = int.Parse(columns[0]);
            var article = int.Parse(columns[1]);
            var rating = float.Parse(columns[2]);

            return new UserPref(id, article, rating);
        }
    }
}