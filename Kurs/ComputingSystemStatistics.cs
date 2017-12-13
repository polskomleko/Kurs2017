using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kurs
{
    /// <summary>
    /// Статистика, когторая собирается по ходу работы ВС
    /// </summary>
    public class ComputingSystemStatistics
    {
        public List<SnapShot> SnapShots = new List<SnapShot>();
        public List<ServerProgramm> programms;

        public SortedDictionary<int, double> ProgrammsCountProbability;

        public int ExecutedProgrammsCount = 0;
        public int DiscardedProgrammsCount = 0;
        public int TotalProgrammsAdded = 0;

        public void AnalizeSnapShots()
        {
            SortedDictionary<int, int> programmsStepsDictionary = new SortedDictionary<int, int>();
            ProgrammsCountProbability = new SortedDictionary<int, double>();

            for (int i = 0; i < SnapShots.Count; ++i)
            {
                var programmsInSystem = SnapShots[i].ExecutingCount + SnapShots[i].BufferItemsCount;
                if (!programmsStepsDictionary.ContainsKey(programmsInSystem))
                    programmsStepsDictionary.Add(programmsInSystem, 0);

                programmsStepsDictionary[programmsInSystem]++;
            }

            foreach (var keyValuePair in programmsStepsDictionary)
            {
                ProgrammsCountProbability.Add(keyValuePair.Key, CountProbability(keyValuePair.Value, SnapShots.Count));
            }
        }

        public static double CountProbability(int cases, int maxCases)
        {
            var prob = Math.Round((double)cases / (double)maxCases, 4);
            return prob <= 1 ? prob : 1;
        }
    }

    public class SnapShot
    {
        public int ExecutingCount = 0;
        public int BufferItemsCount = 0;
    }
}
