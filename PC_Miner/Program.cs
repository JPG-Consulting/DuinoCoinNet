using DuinoCoin;
using PCMiner.Resources;
using System;
using System.Configuration;
using System.Threading;

namespace PCMiner
{
    class Program
    {
        private static DucoMinerSettings GetSettings()
        {
            DucoMinerSettings ducoMinerSettings = DucoMinerSettings.Create();

            if (!String.IsNullOrWhiteSpace(ducoMinerSettings.Username))
                return ducoMinerSettings;

            string tempString = null;

            //
            // Username
            //
            while (String.IsNullOrEmpty(tempString))
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write(GetTranslation("ask_username"));
                Console.ResetColor();
                tempString = Console.ReadLine().Trim();
            }

            ducoMinerSettings.Username = tempString;
            tempString = null;

            //
            // Rig name
            //
            string szYesNo = string.Empty;

            while ((!szYesNo.Equals("Y", StringComparison.InvariantCultureIgnoreCase)) && (!szYesNo.Equals("N", StringComparison.InvariantCultureIgnoreCase)))
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write(GetTranslation("ask_rig_identifier"));
                Console.ResetColor();
                szYesNo = Console.ReadLine();

                if (String.IsNullOrEmpty(szYesNo))
                    szYesNo = "N";

                if (szYesNo.Equals("Y", StringComparison.InvariantCultureIgnoreCase))
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write(GetTranslation("ask_rig_name"));
                    Console.ResetColor();
                    ducoMinerSettings.RigName = Console.ReadLine().Trim();
                }
            }

            ducoMinerSettings.Save();

            return ducoMinerSettings;
        }

        static void Main(string[] args)
        {
            DucoMinerSettings ducoMinerSettings = GetSettings();

            DucoClient client = new DucoClient();
            client.Settings = ducoMinerSettings;
            client.PoolInformationRetrieved += Client_PoolInformationRetrieved;
            client.Connected += Client_Connected;
            client.JobReceived += Client_JobReceived;
            client.JobCompleted += Client_JobCompleted;
            client.NodeMessageReceived += Client_NodeMessageReceived;
            client.StartMining();

            while (1 == 1)
            {
                Thread.Sleep(10);
            }
        }

        private static string GetTranslation(string name)
        {
            return StringResources.ResourceManager.GetString(name, System.Globalization.CultureInfo.CurrentUICulture);
        }

        private static string GetTranslation(string name, params string[] args)
        {
            return String.Format(System.Globalization.CultureInfo.CurrentUICulture, GetTranslation(name), args);
        }

        private static void Client_NodeMessageReceived(object sender, NodeMessageEventArgs e)
        {
            WriteConsolePrefix(e.Source);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(GetTranslation("node_message"));
            Console.ResetColor();
            Console.WriteLine(e.Message);
        }

        private static void Client_JobCompleted(object sender, JobCompletedEventArgs e)
        {
            WriteConsolePrefix(e.Source);

            switch (e.Response.Trim().ToUpperInvariant())
            {
                case "BAD":
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(GetTranslation("rejected"));
                        Console.ResetColor();
                    }
                    break;
                case "GOOD":
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(GetTranslation("accepted"));
                        Console.ResetColor();
                    }
                    break;
                case "BLOCK":
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(GetTranslation("block_found"));
                        Console.ResetColor();
                    }
                    break;
                default:
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(GetTranslation("node_message"));
                        Console.ResetColor();

                        Console.WriteLine(e.Response);
                        return;
                    }
            }

            Console.ResetColor();
            Console.Write(String.Format("{0}/{1}  ", e.JobStatistics.Accepted, e.JobStatistics.Accepted + e.JobStatistics.Rejected));

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(String.Format("({0}%) ", Math.Round((Convert.ToDouble(e.JobStatistics.Accepted) / Convert.ToDouble(e.JobStatistics.Accepted + e.JobStatistics.Rejected)) * 100, 0)));
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(String.Concat(Math.Round(e.Ellapsed.TotalMilliseconds / 1000, 2).ToString(), "s"));
            Console.ResetColor();

            Console.Write(" ");

            Console.Write(String.Concat(Math.Round(e.HashRate / 1000, 2).ToString(), " kH/s"));

            Console.WriteLine();
        }

        private static void Client_JobReceived(object sender, JobEventArgs e)
        {
            WriteConsolePrefix(e.Source);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(GetTranslation("job_received"));
            Console.ResetColor();

            Console.Write(e.Job.LastBlockHash.ToString());
            Console.Write(" ");

            Console.Write(e.Job.ExpectedHash.ToString());
            Console.Write(" ");

            Console.Write(e.Job.Difficulty.ToString());
            Console.Write(" ");

            Console.WriteLine();
        }

        private static void WriteConsolePrefix(string source)
        {
            Console.ResetColor();
            Console.Write(String.Concat(DateTime.Now.ToString("HH:mm:ss"), " "));

            string who = source.Substring(0, 3).ToUpperInvariant();
            switch (who)
            {
                case "SYS":
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    break;
                case "NET":
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    break;
                case "CPU":
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    break;
            }

            Console.Write(String.Concat(" ", source, " "));
            Console.ResetColor();
        }

        private static void Client_Connected(object sender, ConnectedEventArgs e)
        {
            Version ver;

            DucoClient client = sender as DucoClient;

            if ((client == null) || (String.IsNullOrEmpty(e.ServerVersion)) || (!Version.TryParse(e.ServerVersion, out ver)))
                return;

            
            if (ver.CompareTo(client.ProtocolVersion) <= 0)
                return;

            WriteConsolePrefix(e.Source);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(GetTranslation("outdated_miner", client.ProtocolVersion.ToString(), e.ServerVersion.ToString()));
            Console.ResetColor();

            Console.WriteLine();
        }

        private static void Client_PoolInformationRetrieved(object sender, PoolInformationRetrievedEventArgs e)
        {
            WriteConsolePrefix(e.Source);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(GetTranslation("mining_node_retrieved"));
            Console.ResetColor();

            Console.Write(e.PoolInformation.Name);

            Console.WriteLine();
        }
    }
}
