using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace PolypelxPortal_BAL.PortalClasses
{
    [DataContract]
    public class CustomerAccount
    {
        string _CustomerId, _CustomerName, _CustomerGroupId;

        [DataMember]
        public string CustomerId
        {
            get { return _CustomerId; }
            set { _CustomerId = value; }
        }
        [DataMember]
        public string CustomerName
        {
            get { return _CustomerName; }
            set { _CustomerName = value; }
        }
        [DataMember]
        public string CustomerGroupId
        {
            get { return _CustomerGroupId; }
            set { _CustomerGroupId = value; }
        }
    }
}
