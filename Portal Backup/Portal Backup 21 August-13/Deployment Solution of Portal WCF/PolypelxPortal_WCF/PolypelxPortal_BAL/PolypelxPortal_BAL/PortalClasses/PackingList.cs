using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.IO;


namespace PolypelxPortal_BAL.PortalClasses
{
    [DataContract]
    public class PackingList
    {
        #region Private member
        string _CustNo;
        string _PckLstNo;
        #endregion

        #region Properties
        [DataMember]
        public string CustomerNo
        {
            get { return _CustNo; }
            set { _CustNo = value; }
        }
        [DataMember]
        public string PackingListNo
        {
            get { return _PckLstNo; }
            set { _PckLstNo = value; }
        }
        #endregion
    }

}