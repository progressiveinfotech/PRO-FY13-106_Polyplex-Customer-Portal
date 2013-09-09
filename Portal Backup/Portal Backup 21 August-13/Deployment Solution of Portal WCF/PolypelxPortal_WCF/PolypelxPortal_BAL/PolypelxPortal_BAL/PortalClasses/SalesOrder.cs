using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.IO;

namespace PolypelxPortal_BAL.PortalClasses
{
    [DataContract]
    public class SalesOrder
    {
        #region Private member
        string _SalesOrderNo;
        #endregion

        #region Properties
        [DataMember]
        public string SalesOrderNo
        {
            get { return _SalesOrderNo; }
            set { _SalesOrderNo = value; }
        }
        #endregion
    }
}