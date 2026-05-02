using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text.Json;

namespace CPP_Schedule_Builder
{
    public class ProfessorRating
    {
        public bool Found { get; set; }
        public string? Name { get; set; }
        public string? Department { get; set; }
        public double? Rating { get; set; }
        public double? Difficulty { get; set; }
        public int? NumRatings { get; set; }
        public double? WouldTakeAgain { get; set; }
        public string? Error { get; set; }


        public static ProfessorRating GetProfessorRating(string professorName)
        {
            string schoolName = "Cal Poly Pomona";

            string scriptPath = Path.GetFullPath(Path.Combine(
                AppContext.BaseDirectory,
                "..", "..", "..",
                "Python",
                "rmp_lookup.py"));

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = $"\"{scriptPath}\" \"{schoolName}\" \"{professorName}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using Process process = Process.Start(startInfo)!;

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            process.WaitForExit();

            if (!string.IsNullOrWhiteSpace(error))
            {
                return new ProfessorRating
                {
                    Found = false,
                    Error = error
                };
            }

            return JsonSerializer.Deserialize<ProfessorRating>(
                output,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new ProfessorRating
                {
                    Found = false,
                    Error = "Could not read Python response."
                };
        }

    }


}
