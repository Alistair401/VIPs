namespace Ground.Map
{
    public class District
    {
        public int MinX { get; set; }
        public int MinY { get; set; }
        public int MaxX { get; set; }
        public int MaxY { get; set; }

        public int Probability
        {
            get { return ((MaxX - MinX) * (MaxY - MinY)) ^ 2; }
        }
    }
}