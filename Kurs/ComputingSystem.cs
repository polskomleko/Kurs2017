using System;
using System.Collections.Generic;
using System.Linq;

namespace Kurs
{
    public class ComputingSystem
    {
        public delegate void simulationProcessCallback(double percentsDone);

        private int bufferItems = 0;
        private List<ServerProgramm> programms;
        private List<ServerProgramm> finishedProgramms;

        private static Random random = new Random();

        public ComputingSystemStatistics Simulate(simulationProcessCallback callcack, DistributionType distributionType = DistributionType.Liniar)
        {
            //сброс переменных
            bufferItems = 0;
            programms = new List<ServerProgramm>();
            finishedProgramms = new List<ServerProgramm>();

            ComputingSystemStatistics statistic = new ComputingSystemStatistics();

            double timeTillNextProgrammLinear = 0;
            var popupProbabilityExp = GetExpProbability(Constants.ProgrammPopupTime.ExpLambda, Constants.SimulationStep);
            double elapsedExp = 0;

            for (double i = 0; i <= Constants.SimulationTime; i += Constants.SimulationStep)
            {
                if ((int)i % 10 == 0)
                    callcack(i / (Constants.SimulationTime / 100.0));

                programms.ForEach(x => x.Update(Constants.SimulationStep));

                //поступление новых программ
                if (distributionType == DistributionType.Liniar)
                {
                    if (timeTillNextProgrammLinear <= 0)
                    {
                        var serverProgramm = new ServerProgramm(DistributionType.Liniar,
                            GetRandomNumber(Constants.ProgrammExecutionTime.LinearMinTime, Constants.ProgrammExecutionTime.LinearMaxTime));
                        programms.Add(serverProgramm);
                        timeTillNextProgrammLinear = GetRandomNumber(Constants.ProgrammPopupTime.LinearMinTime, Constants.ProgrammPopupTime.LinearMaxTime);
                        statistic.TotalProgrammsAdded++;
                    }
                    timeTillNextProgrammLinear -= Constants.SimulationStep;
                }
                else if (distributionType == DistributionType.Exponential)
                {
                    if (popupProbabilityExp > GetRandomNumber(0, 1))
                    {
                        var serverProgramm = new ServerProgramm(DistributionType.Exponential,
                            GetRandomNumber(Constants.ProgrammExecutionTime.LinearMinTime, Constants.ProgrammExecutionTime.LinearMaxTime));
                        programms.Add(serverProgramm);
                        statistic.TotalProgrammsAdded++;
                    }
                }

                //перевод программ из ожидания в выполнение
                int freeServersCount = Constants.ServersCount - programms.Count(x => x.Status == ServerProgramm.ProgrammStatus.Executing);
                programms.Where(x => x.Status == ServerProgramm.ProgrammStatus.AwaitingExecution).Take(freeServersCount).ToList()
                    .ForEach(x => x.Status = ServerProgramm.ProgrammStatus.Executing);

                //считаем переполняющие буфер программы невыполненными
                var overflowCount = programms.Count(x => x.Status == ServerProgramm.ProgrammStatus.AwaitingExecution) - Constants.BufferSize;
                if (overflowCount > 0)
                    programms.Where(x => x.Status == ServerProgramm.ProgrammStatus.AwaitingExecution).Take(overflowCount).ToList()
                        .ForEach(x => x.Status = ServerProgramm.ProgrammStatus.Discarded);

                //сбор статистики
                statistic.SnapShots.Add(new SnapShot()
                {
                    BufferItemsCount = programms.Count(x => x.Status == ServerProgramm.ProgrammStatus.AwaitingExecution),
                    ExecutingCount = programms.Count(x => x.Status == ServerProgramm.ProgrammStatus.Executing)
                });

                //удаляем из листа программ не обработанные и выполненные программы для оптимизации скорости поиска по листу в следующих итерациях
                var hist = programms.Where(x => x.Status == ServerProgramm.ProgrammStatus.Discarded || x.Status == ServerProgramm.ProgrammStatus.Executed).ToList();
                foreach (var programm in hist)
                {
                    if (programm.Status == ServerProgramm.ProgrammStatus.Executed)
                        statistic.ExecutedProgrammsCount++;
                    if (programm.Status == ServerProgramm.ProgrammStatus.Discarded)
                        statistic.DiscardedProgrammsCount++;

                    programms.Remove(programm);
                    finishedProgramms.Add(programm);
                }
            }

            //сбор всех программ в один лист для статистики
            finishedProgramms.AddRange(programms);
            statistic.programms = finishedProgramms;
            return statistic;
        }

        public static double GetExpProbability(double lambda, double time)
        {
            return lambda < 0 ? 0 :
                1 - (Math.Exp(-lambda * time));
        }

        public static double GetRandomNumber(double minimum, double maximum)
        {
            return random.NextDouble() * (maximum - minimum) + minimum;
        }

        //тип распределения
        public enum DistributionType
        {
            Liniar, Exponential
        }
    }

    /// <summary>
    /// Программа, выполняющаяся в ВС
    /// </summary>
    public class ServerProgramm
    {
        public ProgrammStatus Status = ProgrammStatus.AwaitingExecution;

        public double ExecutionTimeLeft { get; private set; }
        public double ExecutionTime { get; private set; }
        public double ExecutionAwaitingTime { get; private set; } = 0;

        private bool isExponential;
        private double expAverage;

        public ServerProgramm(ComputingSystem.DistributionType distrType, double time)
        {
            if (distrType == ComputingSystem.DistributionType.Liniar)
            {
                ExecutionTime = ExecutionTimeLeft = time;
                isExponential = false;
            }
            else
            {
                ExecutionTime = 0;
                expAverage = time;
                isExponential = true;
            }
        }

        public void Update(double timePassed)
        {
            if (Status == ProgrammStatus.AwaitingExecution)
                ExecutionAwaitingTime += timePassed;
            else if (Status == ProgrammStatus.Executing)
            {
                if (isExponential)
                {
                    ExecutionTime += timePassed;

                    var probabilityOfBeingExecuted = ComputingSystem.GetExpProbability(Constants.ProgrammExecutionTime.ExpAvgExecutionTime,
                            Constants.SimulationStep);

                    if (probabilityOfBeingExecuted > ComputingSystem.GetRandomNumber(0, 1))
                        Status = ProgrammStatus.Executed;
                }
                else
                {
                    ExecutionTimeLeft -= timePassed;
                    if (ExecutionTimeLeft <= 0)
                        Status = ProgrammStatus.Executed;
                }
            }
        }

        public enum ProgrammStatus
        {
            AwaitingExecution, Executing, Executed, Discarded
        }
    }
}
