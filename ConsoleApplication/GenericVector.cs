using System;

namespace ConsoleApplication
{
    public class GenericVector
    {
        private float[] _points;

        public int Dimensions => _points.Length;
        public float this[int index] => _points[index];

        public GenericVector(params float[] args)
        {
            _points = args;
        }

        public static double CosineSimilarity(GenericVector a, GenericVector b)
        {
            if (a.Dimensions != b.Dimensions)
                throw new Exception("Distance can't be computed of vectors with different dimensions");

            var sigmaATimesB = 0.0;
            var lengthA = 0.0;
            var lengthB = 0.0;

            for (var i = 0; i < a.Dimensions; i++)
            {
                var A = a[i];
                var B = b[i];

                if (float.IsNaN(A))
                    A = 0;
                if (float.IsNaN(B))
                    B = 0;

                sigmaATimesB += A * B;
                lengthA += Math.Pow(A, 2);
                lengthB += Math.Pow(B, 2);
            }

            return sigmaATimesB / (Math.Sqrt(lengthA) * Math.Sqrt(lengthB));
        }

        public static double PearsonCoefficient(GenericVector a, GenericVector b)
        {
            if (a.Dimensions != b.Dimensions)
                throw new Exception("Distance can't be computed of vectors with different dimensions");

            var n = a.Dimensions;
            var sigmaA = 0.0;
            var sigmaB = 0.0;
            var sigmaATimesB = 0.0;
            var sigmaASquared = 0.0;
            var sigmaBSquared = 0.0;

            for (var i = 0; i < a.Dimensions; i++)
            {
                if (float.IsNaN(a[i]) || float.IsNaN(b[i]))
                {
                    n--;
                    continue;
                }

                sigmaA += a[i];
                sigmaB += b[i];
                sigmaATimesB += a[i] * b[i];
                sigmaASquared += Math.Pow(a[i], 2);
                sigmaBSquared += Math.Pow(b[i], 2);
            }

            return (sigmaATimesB - (sigmaA * sigmaB) / n) /
                   (Math.Sqrt(sigmaASquared - Math.Pow(sigmaA, 2) / n) *
                    Math.Sqrt(sigmaBSquared - Math.Pow(sigmaB, 2) / n));
        }

        public static double EucledianDistanceSimilarity(GenericVector a, GenericVector b)
        {
            if (a.Dimensions != b.Dimensions)
                throw new Exception("Distance can't be computed of vectors with different dimensions");

            var sigmaAb = 0.0;
            for (var i = 0; i < a.Dimensions; i++)
            {
                if (!float.IsNaN(a[i]) && !float.IsNaN(b[i]))
                    sigmaAb += Math.Pow(a[i] - b[i], 2);
            }
            return  1 / (1 + Math.Sqrt(sigmaAb));
        }

        public static double ManhattanDistanceSimilarity(GenericVector a, GenericVector b)
        {
            if (a.Dimensions != b.Dimensions)
                throw new Exception("Distance can't be computed of vectors with different dimensions");
            var sigmaAb = 0.0;
            for (var i = 0; i < a.Dimensions; i++)
            {
                if (!float.IsNaN(a[i]) && !float.IsNaN(b[i]))
                    sigmaAb += Math.Abs(a[i] - b[i]);
            }
            return  1 / (1 + sigmaAb);
        }
    }
}