using log4net;

[assembly: log4net.Config.XmlConfigurator(Watch = true, ConfigFile = "Configs/log4net.config")]

namespace LotterySpider.Common.Utils
{
    public class LogHelper
    {
        private static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Debug(string message)
        {
            _log.Debug(message);
        }

        public static void Fatal(string message)
        {
            _log.Fatal(message);
        }

        public static void Info(string message)
        {
            _log.Info(message);
        }

        public static void Error(string message)
        {
            _log.Error(message);
        }

        public static void Warn(string message)
        {
            _log.Warn(message);
        }
    }
}