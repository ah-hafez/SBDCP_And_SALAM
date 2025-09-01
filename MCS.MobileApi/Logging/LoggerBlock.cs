using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace MobileAPIs.Logging
{
    public sealed class LoggerBlock
    {
        private static readonly object padlock = new object();
        private static LogWriter loggerBlock = null;
        public static LogWriter LoggerBlockValue
        {
            get
            {
                if (loggerBlock == null)
                {
                    lock (padlock)
                    {
                        if (loggerBlock == null)
                        {
                            var logWriter = new LogWriterFactory().Create();
                            Logger.SetLogWriter(logWriter, false);
                            loggerBlock = logWriter;
                        }
                    }
                }
                return loggerBlock;
            }
        }
    }
}