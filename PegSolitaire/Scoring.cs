namespace PegSolitaire
{
    class Scoring
    {
        public Scoring(string name, double gameTime)
        {
            Name = name;
            GameTime = gameTime;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public double GameTime { get; set; }
    }
}
