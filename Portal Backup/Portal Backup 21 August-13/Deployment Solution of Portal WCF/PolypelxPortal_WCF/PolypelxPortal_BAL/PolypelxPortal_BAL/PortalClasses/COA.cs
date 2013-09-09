using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.IO;


namespace PolypelxPortal_BAL.PortalClasses
{
    [DataContract]
    public class COA
    {
        #region Private member
        string _COANo;
        #endregion

        #region Properties
        [DataMember]
        public string COANo
        {
            get { return _COANo; }
            set { _COANo = value; }
        }
        #endregion
    }
}
