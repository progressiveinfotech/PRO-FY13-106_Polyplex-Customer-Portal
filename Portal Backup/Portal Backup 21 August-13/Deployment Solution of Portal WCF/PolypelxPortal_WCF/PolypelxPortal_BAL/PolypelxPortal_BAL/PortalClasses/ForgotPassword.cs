using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace PolypelxPortal_BAL.PortalClasses
{
    [DataContract]
    public class ForgotPassword
    {
        string _LoginId, _EmailId, _Password, _ActiveStatus, _UserName;

        [DataMember]
        public string LoginId
        {
            get { return _LoginId; }
            set { _LoginId = value; }
        }
        [DataMember]
        public string EmailId
        {
            get { return _EmailId; }
            set { _EmailId = value; }
        }
        [DataMember]
        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }
        [DataMember]
        public string ActiveStatus
        {
            get { return _ActiveStatus; }
            set { _ActiveStatus = value; }
        }
        [DataMember]
        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }
    }
}
