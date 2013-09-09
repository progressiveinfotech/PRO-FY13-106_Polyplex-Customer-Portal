using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PolypelxPortal_BAL.BasicClasses
{
    public class Common
    {
        #region *****************************Variables**************************************

        private string _DateFormat = "MM/dd/yyyy";
        private IFormatProvider _FormatProvider = new System.Globalization.CultureInfo("en-US", true);

        #endregion

        #region *****************************Functions**************************************

        public Common()
        { }

        public Int32 STRToInt(string sData)
        {
            try
            {
                return Convert.ToInt32(sData);
            }
            catch
            {
                return 0;
            }
        }

        public Int64 STRToIntBig(string sData)
        {
            try
            {
                return Convert.ToInt64(sData);
            }
            catch
            {
                return 0;
            }
        }

        public Double STRToDBL(string sData)
        {
            try
            {
                return Convert.ToDouble(sData);
            }
            catch
            {
                return 0.0;
            }
        }

        public DateTime STRToDate(string sData)
        {
            try
            {
                return Convert.ToDateTime(sData);
            }
            catch
            {
                return DateTime.Now;
            }

        }

        public decimal STRToDCML(string sData)
        {
            try
            {
                return Convert.ToDecimal(sData);
            }
            catch
            {
                return 0.0M;
            }
        }

        public bool STRToBool(string sData)
        {
            try
            {
                return Convert.ToBoolean(sData);
            }
            catch
            {
                return false;
            }
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime ();
            return dtDateTime;
         }

        public static double DateTimeToUnixTimestamp(DateTime dateTime)
        {
            return (dateTime - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds;
        }

        public string FindDate(string param)
        {
            Log.LogMessage(param);
            int SIndex = param.IndexOf("(");
            int EIndex = param.IndexOf(")");
            Log.LogMessage(SIndex.ToString());
            Log.LogMessage(EIndex.ToString());
            string result = param.Substring(SIndex + 1, EIndex - SIndex - 1);
            return result;
        }

        #endregion
    }
}
