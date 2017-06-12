namespace Assignment2
{
    public class Deviation
    {
        public double Value { get; private set; }
        public int Cardinality { get; private set; }

        public Deviation(double deviation, int cardinality = 1)
        {
            Value = deviation;
            Cardinality = cardinality;
        }

        public void Update(double dev)
        {
            Value = ((Value * Cardinality) + dev) / (Cardinality + 1);
            Cardinality++;
        }

    }
}