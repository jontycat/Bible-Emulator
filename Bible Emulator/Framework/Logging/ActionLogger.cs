using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Config;
using System.Windows.Forms;

namespace Logging
{
    public class ActionLogger
    {
        private static readonly ILog mLogger = LogManager.GetLogger("ActionLogger");

        public static void writeError(String txt)
        {
            mLogger.Error(txt);
        }

        public static void writeDebug(String txt)
        {
            mLogger.Debug(txt);
        }

        public static void writeFatal(String txt)
        {
            mLogger.Fatal(txt);
        }

        public static void writeInfo(String txt)
        {
            mLogger.Info(txt);
        }
    }
}
