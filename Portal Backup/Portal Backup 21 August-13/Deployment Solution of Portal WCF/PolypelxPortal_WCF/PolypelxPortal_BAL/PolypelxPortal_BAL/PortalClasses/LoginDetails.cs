using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace PolypelxPortal_BAL.PortalClasses
{
    [DataContract]
    public class LoginDetails
    {
        string _LoginId, _Password;

        [DataMember]
        public string LoginId
        {
            get { return _LoginId; }
            set { _LoginId = value; }
        }
        [DataMember]
        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }
    }
}
