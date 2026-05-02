using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPP_Schedule_Builder
{
    internal class ScheduleMinCommute : Schedule
    {
        private new List<Lecture> BuildMinCommuteSchedule()
        {
            TimeSpan MorningTrafficStart = new TimeSpan(7, 0, 0);
            TimeSpan MorningTrafficEnd = new TimeSpan(9, 0, 0);
            TimeSpan EveningTrafficStart = new TimeSpan(16, 0, 0);
            TimeSpan EveningTrafficEnd = new TimeSpan(18, 0, 0);
            List<(Lecture lecture, int score)> scoredLectures = new List<(Lecture, int)>();
            foreach (Lecture lecture in Lectures)
            {
                TimeSpan start = ConvertTo24Hour(lecture.StartTime, lecture.StartAM_PM);
                TimeSpan end = ConvertTo24Hour(lecture.EndTime, lecture.EndAM_PM);
                int score = 0;
                if ((start >= MorningTrafficStart && start < MorningTrafficEnd) || (end > MorningTrafficStart && end <= MorningTrafficEnd))
                {
                    score += 2; // High traffic during morning rush hour
                }
                if ((start >= EveningTrafficStart && start < EveningTrafficEnd) || (end > EveningTrafficStart && end <= EveningTrafficEnd))
                {
                    score += 2; // High traffic during evening rush hour
                }
                scoredLectures.Add((lecture, score));
            }
            scoredLectures = scoredLectures.OrderBy(x => x.score).ToList();

            List<Lecture> minCommuteSchedule = new List<Lecture>();
            foreach ((Lecture lecture, int score) in scoredLectures)
            {
                bool hasConflict = false;
                foreach (Lecture added in minCommuteSchedule)
                {
                    if (AreConflicting(lecture, added) || lecture.ClassCode == added.ClassCode)
                    {
                        hasConflict = true;
                        break;
                    }
                }
                if (!hasConflict)
                {
                    minCommuteSchedule.Add(lecture);
                }
            }
            return minCommuteSchedule;
        }
    }
}