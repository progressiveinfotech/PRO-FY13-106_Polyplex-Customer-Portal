using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;

namespace PolypelxPortal_BAL.BasicClasses
{
    public class Log
    {
        public static void LogMessage(string sMsg)
        {
            File.AppendAllText(@"C:\PolypelxPortal_WCF.log", sMsg + Environment.NewLine);
        }
    }
}
