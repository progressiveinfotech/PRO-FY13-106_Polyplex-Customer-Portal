using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace PolypelxPortal_BAL.PortalClasses
{
    [DataContract]
    public class WarehouseLocation
    {
        string _WareHouseId, _WareHouseName, _CustomerId;

        [DataMember]
        public string WareHouseId
        {
            get { return _WareHouseId; }
            set { _WareHouseId = value; }
        }
        [DataMember]
        public string WareHouseName
        {
            get { return _WareHouseName; }
            set { _WareHouseName = value; }
        }
        [DataMember]
        public string CustomerId
        {
            get { return _CustomerId; }
            set { _CustomerId = value; }
        }
    }
}
