namespace Ground.Map
{
    public class Block
    {
        public int MinX { get; set; }
        public int MinY { get; set; }
        public int MaxX { get; set; }
        public int MaxY { get; set; }

        public int Probability
        {
            get { return Size ^ 2; }
        }

        public int Size
        {
            get { return (MaxX - MinX) * (MaxY - MinY); }
        }

        public int XLength
        {
            get { return MaxX - MinX; }
        }

        public int YLength
        {
            get { return MaxY - MinY; }
        }
    }
}