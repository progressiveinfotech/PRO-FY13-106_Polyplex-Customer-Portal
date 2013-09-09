using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace PolypelxPortal_BAL.PortalClasses
{
    [DataContract]
    public class CustomerGroup
    {
        string _CustomerGroupId, _UserName;
        
        [DataMember]
        public string CustomerGroupId
        {
            get { return _CustomerGroupId; }
            set { _CustomerGroupId = value; }
        }
        [DataMember]
        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }
    }
}
