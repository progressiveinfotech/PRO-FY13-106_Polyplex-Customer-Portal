using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.IO;

namespace PolypelxPortal_BAL.PortalClasses
{
    [DataContract]
    public class Invoice
    {
        #region Private member
        private string _InvoiceNo;
        private string _Unit;
        private string _PrintType;
        #endregion

        #region Properties
        [DataMember]
        public string InvoiceNo
        {
            get { return _InvoiceNo; }
            set { _InvoiceNo = value; }
        }
        [DataMember]
        public string Unit
        {
            get { return _Unit; }
            set { _Unit = value; }
        }

        [DataMember]
        public string PrintType
        {
            get { return _PrintType; }
            set { _PrintType = value; }
        }

        #endregion
    }
}