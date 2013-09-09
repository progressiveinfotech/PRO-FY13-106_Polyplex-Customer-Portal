using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace PolypelxPortal_BAL.PortalClasses
{
    [DataContract]
    public class UserDetails
    {
        private string _UserId, _UserCode, _UserName, _EmailId, _LocationId, _LocationName, _UserTypeId, _UserType, _ActiveStatus;

        [DataMember]
        public string UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }
        [DataMember]
        public string UserCode
        {
            get { return _UserCode; }
            set { _UserCode = value; }
        }
        [DataMember]
        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }
        [DataMember]
        public string EmailId
        {
            get { return _EmailId; }
            set { _EmailId = value; }
        }
        [DataMember]
        public string LocationId
        {
            get { return _LocationId; }
            set { _LocationId = value; }
        }
        [DataMember]
        public string LocationName
        {
            get { return _LocationName; }
            set { _LocationName = value; }
        }
        [DataMember]
        public string UserTypeId
        {
            get { return _UserTypeId; }
            set { _UserTypeId = value; }
        }
        [DataMember]
        public string UserType
        {
            get { return _UserType; }
            set { _UserType = value; }
        }
        [DataMember]
        public string ActiveStatus
        {
            get { return _ActiveStatus; }
            set { _ActiveStatus = value; }
        }
    }
}
