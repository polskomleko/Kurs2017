using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kurs
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            textBox_inputInfo.Text =
                "Количество серверов: " + Constants.ServersCount + Environment.NewLine +
                "Размер буфера: " + Constants.BufferSize + Environment.NewLine +
                "(Линейный закон) Программы поступают в промежутке от " + Constants.ProgrammPopupTime.LinearMinTime + " до " +
                Math.Round(Constants.ProgrammPopupTime.LinearMaxTime, 2) + " сек" + Environment.NewLine +
                "(Линейный закон) Время обработки одной программы: от " + Constants.ProgrammExecutionTime.LinearMinTime +
                " до " + Constants.ProgrammExecutionTime.LinearMaxTime + " сек" + Environment.NewLine +
                "(Экспоненц. закон) Частота: " + Constants.ProgrammPopupTime.ExpLambda + Environment.NewLine +
                "(Экспоненц. закон) сред. время обработки: " + Constants.ProgrammExecutionTime.ExpAvgExecutionTime + Environment.NewLine +
                "Время симуляции: " + Constants.SimulationTime + " сек";

            button_simulate.Focus();
            CheckForIllegalCrossThreadCalls = false;
        }

        private void button_simulate_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(delegate
            {
                simulateAndShowResults(ComputingSystem.DistributionType.Liniar);
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(delegate
            {
                simulateAndShowResults(ComputingSystem.DistributionType.Exponential);
            });
        }

        private void simulateAndShowResults(ComputingSystem.DistributionType distrType)
        {
            textBox_output.Text = "";

            ComputingSystem cs = new ComputingSystem();

            var stats = cs.Simulate(delegate (double done)
            {
                progressBar1.Value = (int)Math.Round(done);
            }, distrType);

            stats.AnalizeSnapShots();

            string output = "";

            foreach (var programmsProb in stats.ProgrammsCountProbability)
            {
                if (programmsProb.Key == 0)
                {
                    output += $"{programmsProb.Value} - вероятность того, что ВС не загружена" + Environment.NewLine;
                }
                else if (programmsProb.Key < Constants.ServersCount)
                {
                    output += $"{programmsProb.Value} - вероятность того, что загружено: {programmsProb.Key} \\ {Constants.ServersCount} серверов" + Environment.NewLine;
                }
                else if (programmsProb.Key == Constants.ServersCount)
                {
                    double probabilityAllServersBusy = 0;
                    stats.ProgrammsCountProbability.ToList().ForEach(pair =>
                    {
                        if (pair.Key >= programmsProb.Key)
                            probabilityAllServersBusy += pair.Value;
                    });

                    output += $"{probabilityAllServersBusy} - вероятность того, что загружено:  {Constants.ServersCount} \\ {Constants.ServersCount} серверов" + Environment.NewLine;
                }
                else if (programmsProb.Key > Constants.ServersCount)
                {
                    var inBufferCount = programmsProb.Key - Constants.ServersCount;

                    output += $"{programmsProb.Value} - вероятность того, что в буфере {inBufferCount} программа(ы)" + Environment.NewLine;
                }
            }

            var executedPercentage = countPercents(stats.TotalProgrammsAdded, stats.ExecutedProgrammsCount);
            output += $"{executedPercentage}% - Q (относит. пропускная способность- процент программ, обработанных ВС)" + Environment.NewLine;

            var programmsExecutedPerSecond = Math.Round((double)stats.ExecutedProgrammsCount / (double)Constants.SimulationTime, 2);
            output += $"{programmsExecutedPerSecond} - S (среднее число программ, обработанных в секунду)" + Environment.NewLine;

            var discardProbability = ComputingSystemStatistics.CountProbability(stats.DiscardedProgrammsCount, stats.TotalProgrammsAdded);
            output += $"{discardProbability} - Pоткл (вероятность отказа, т.е. того, что программа будет не обработанной)" + Environment.NewLine;

            var totalServersBusy = 0;
            int totalProgrammsInSystem = 0;
            int totalProgrammsInBuffer = 0;
            foreach (var snapshot in stats.SnapShots)
            {
                totalProgrammsInSystem += snapshot.ExecutingCount + snapshot.BufferItemsCount;
                totalProgrammsInBuffer += snapshot.BufferItemsCount;
                int serversBusy = snapshot.ExecutingCount;
                if (serversBusy > Constants.ServersCount)
                    serversBusy = Constants.ServersCount;
                totalServersBusy += serversBusy;
            }
            double avgServersBusy = Math.Round((double)totalServersBusy / (double)stats.SnapShots.Count, 2);
            output += $"{avgServersBusy} - K (среднее число занятых серверов)" + Environment.NewLine;

            double avgProgrammsInSystem = Math.Round((double)totalProgrammsInSystem / (double)stats.SnapShots.Count, 2);
            output += $"{avgProgrammsInSystem} - K (среднее число программ в ВС)" + Environment.NewLine;

            var spentTimeTotal = 0.0;
            var spentTimeInBuffer = 0.0;
            foreach (var programm in stats.programms)
            {
                spentTimeTotal += programm.ExecutionAwaitingTime + programm.ExecutionTime;
                spentTimeInBuffer += programm.ExecutionAwaitingTime;
            }
            var averageTimeInSystem = Math.Round(spentTimeTotal / (double)stats.programms.Count, 2);
            output += $"{averageTimeInSystem} сек - Tпрог (среднее время нахождения программы в ВС)" + Environment.NewLine;

            double avgProgrammsInBuffer = Math.Round((double)totalProgrammsInBuffer / (double)stats.SnapShots.Count, 2);
            output += $"{avgProgrammsInBuffer} - Nбуф (среднее число программ в буфере)" + Environment.NewLine;

            var averageTimeInBuffer = Math.Round(spentTimeInBuffer / (double)stats.programms.Count, 2);
            output += $"{averageTimeInBuffer} сек - Tбуф (среднее время нахождения программы в буфере)" + Environment.NewLine;

            textBox_output.Text = output;
        }

        private double countPercents(double total, double value)
        {
            return Math.Round(value / (total / 100.0), 2);
        }
    }
}
