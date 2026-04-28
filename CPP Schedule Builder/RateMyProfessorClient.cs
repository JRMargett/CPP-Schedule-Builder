using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;

namespace CPP_Schedule_Builder
{
    internal sealed class RateMyProfessorClient
    {
        private const int CalPolyPomonaSchoolId = 13914;
        private const string BaseUrl = "https://www.ratemyprofessors.com";

        private static readonly Regex TeacherCardRegex = new(
            @"<a\b[^>]*class=""[^""]*TeacherCard__StyledTeacherCard[^""]*""[^>]*href=""/professor/(?<id>\d+)""[^>]*>(?<card>.*?)</a>",
            RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private static readonly Regex RatingRegex = new(
            @"CardNumRating__CardNumRatingNumber[^""]*""[^>]*>(?<value>.*?)</div>",
            RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private static readonly Regex RatingsCountRegex = new(
            @"CardNumRating__CardNumRatingCount[^""]*""[^>]*>(?<value>.*?)</div>",
            RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private static readonly Regex NameRegex = new(
            @"CardName__StyledCardName[^""]*""[^>]*>(?<value>.*?)</div>",
            RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private static readonly HttpClient HttpClient = CreateHttpClient();

        private readonly Dictionary<string, RateMyProfessorRating?> cache = new(StringComparer.OrdinalIgnoreCase);

        public async Task<RateMyProfessorRating?> GetProfessorRatingAsync(string professorName)
        {
            string cleanedName = professorName.Trim();

            if (string.IsNullOrWhiteSpace(cleanedName))
            {
                return null;
            }

            if (cache.TryGetValue(cleanedName, out RateMyProfessorRating? cachedRating))
            {
                return cachedRating;
            }

            string url = $"{BaseUrl}/search/professors/{CalPolyPomonaSchoolId}?did=*&q={Uri.EscapeDataString(cleanedName)}";

            try
            {
                string html = await HttpClient.GetStringAsync(url);
                RateMyProfessorRating? rating = FindBestMatch(ParseRatings(html), cleanedName);
                cache[cleanedName] = rating;
                return rating;
            }
            catch (HttpRequestException)
            {
                cache[cleanedName] = null;
                return null;
            }
            catch (TaskCanceledException)
            {
                cache[cleanedName] = null;
                return null;
            }
        }

        private static HttpClient CreateHttpClient()
        {
            HttpClient client = new()
            {
                Timeout = TimeSpan.FromSeconds(15)
            };

            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0");
            client.DefaultRequestHeaders.Accept.ParseAdd("text/html");
            return client;
        }

        private static List<RateMyProfessorRating> ParseRatings(string html)
        {
            List<RateMyProfessorRating> ratings = new();

            foreach (Match match in TeacherCardRegex.Matches(html))
            {
                string cardHtml = match.Groups["card"].Value;
                string name = ExtractCleanValue(NameRegex, cardHtml);

                if (string.IsNullOrWhiteSpace(name))
                {
                    continue;
                }

                string ratingText = ExtractCleanValue(RatingRegex, cardHtml);
                double? score = double.TryParse(ratingText, NumberStyles.Number, CultureInfo.InvariantCulture, out double parsedScore)
                    ? parsedScore
                    : null;

                string countText = ExtractCleanValue(RatingsCountRegex, cardHtml);
                int ratingsCount = 0;
                Match countMatch = Regex.Match(countText, @"\d+");

                if (countMatch.Success)
                {
                    ratingsCount = int.Parse(countMatch.Value, CultureInfo.InvariantCulture);
                }

                ratings.Add(new RateMyProfessorRating(
                    name,
                    score,
                    ratingsCount,
                    $"{BaseUrl}/professor/{match.Groups["id"].Value}"));
            }

            return ratings;
        }

        private static RateMyProfessorRating? FindBestMatch(IEnumerable<RateMyProfessorRating> ratings, string professorName)
        {
            string normalizedSearchName = NormalizeName(professorName);
            string[] searchTokens = normalizedSearchName.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            return ratings
                .Select(rating => new
                {
                    Rating = rating,
                    Score = GetNameMatchScore(searchTokens, NormalizeName(rating.Name))
                })
                .Where(result => result.Score > 0)
                .OrderByDescending(result => result.Score)
                .ThenByDescending(result => result.Rating.Score ?? -1)
                .ThenByDescending(result => result.Rating.RatingsCount)
                .Select(result => result.Rating)
                .FirstOrDefault();
        }

        private static int GetNameMatchScore(IReadOnlyCollection<string> searchTokens, string normalizedRatingName)
        {
            if (searchTokens.Count == 0)
            {
                return 0;
            }

            string[] ratingTokens = normalizedRatingName.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (searchTokens.SequenceEqual(ratingTokens))
            {
                return 100;
            }

            int tokenMatches = searchTokens.Count(token => ratingTokens.Contains(token));

            if (tokenMatches == searchTokens.Count)
            {
                return 50 + tokenMatches;
            }

            return tokenMatches;
        }

        private static string ExtractCleanValue(Regex regex, string html)
        {
            Match match = regex.Match(html);
            return match.Success ? CleanHtml(match.Groups["value"].Value) : string.Empty;
        }

        private static string CleanHtml(string value)
        {
            string withoutComments = Regex.Replace(value, @"<!--.*?-->", " ", RegexOptions.Singleline);
            string withoutTags = Regex.Replace(withoutComments, "<.*?>", " ", RegexOptions.Singleline);
            string decoded = WebUtility.HtmlDecode(withoutTags);
            return Regex.Replace(decoded, @"\s+", " ").Trim();
        }

        private static string NormalizeName(string name)
        {
            string normalized = name.ToLowerInvariant();
            normalized = Regex.Replace(normalized, @"\b(professor|prof|dr|doctor|phd)\b\.?", " ");
            normalized = Regex.Replace(normalized, @"[^a-z\s]", " ");
            return Regex.Replace(normalized, @"\s+", " ").Trim();
        }
    }
}
