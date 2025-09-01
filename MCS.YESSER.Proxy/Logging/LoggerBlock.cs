using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.YESSER.Proxy
{
    public class LoggerBlock
    {
        protected LogWriter logWriter;

        public LoggerBlock()
        {
            InitLogging();
        }

        private void InitLogging()
        {
            logWriter = new LogWriterFactory().Create();
            Logger.SetLogWriter(logWriter, false);
        }

        public LogWriter LogWriter
        {
            get
            {
                return logWriter;
            }
        }
    }
}