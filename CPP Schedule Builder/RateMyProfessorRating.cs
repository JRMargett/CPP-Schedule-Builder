namespace CPP_Schedule_Builder
{
    internal sealed class RateMyProfessorRating
    {
        public RateMyProfessorRating(string name, double? score, int ratingsCount, string profileUrl)
        {
            Name = name;
            Score = score;
            RatingsCount = ratingsCount;
            ProfileUrl = profileUrl;
        }

        public string Name { get; }
        public double? Score { get; }
        public int RatingsCount { get; }
        public string ProfileUrl { get; }
    }
}
