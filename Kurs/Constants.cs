using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurs
{
    /// <summary>
    /// Константы, описывающие параметры вычислительной системы
    /// </summary>
    public static class Constants
    {
        public const int ServersCount = 1;
        public const int BufferSize = 3;

        public static class ProgrammPopupTime
        {
            public const double LinearMinTime = 1.0/2.0;
            public const double LinearMaxTime = 5.0/6.0;

            public const double ExpLambda = 1.5;
        }

        public static class ProgrammExecutionTime
        {
            public const double LinearMinTime = 1;
            public const double LinearMaxTime = 5;

            public const double ExpAvgExecutionTime = 1 / 2.0; // 1 / * (where * is given)
        }



        public const int SimulationTime = 60 * 60;
        public const double SimulationStep = 0.001;
    }
}
