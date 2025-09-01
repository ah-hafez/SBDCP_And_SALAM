using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace MobileApi.Models
{
    [DataContract]
    public class Person
    {
        [DataMember]
        public int Id;

        [DataMember]
        public string Name;

        [DataMember]
        public int EntityId;
    }
}