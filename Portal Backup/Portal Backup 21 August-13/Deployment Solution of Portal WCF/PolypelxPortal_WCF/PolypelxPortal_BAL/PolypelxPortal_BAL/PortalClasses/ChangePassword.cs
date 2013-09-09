using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace PolypelxPortal_BAL.PortalClasses
{
    [DataContract]
    public class ChangePassword
    {
        string _OldPassowrd, _NewPassword, _ActiveStatus, _SaveMessage, _Type;
        int _UserId;

        [DataMember]
        public int UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }
        [DataMember]
        public string OldPassowrd
        {
            get { return _OldPassowrd; }
            set { _OldPassowrd = value; }
        }
        [DataMember]
        public string NewPassword
        {
            get { return _NewPassword; }
            set { _NewPassword = value; }
        }
        [DataMember]
        public string ActiveStatus
        {
            get { return _ActiveStatus; }
            set { _ActiveStatus = value; }
        }
        [DataMember]
        public string SaveMessage
        {
            get { return _SaveMessage; }
            set { _SaveMessage = value; }
        }
        [DataMember]
        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }
    }
}
