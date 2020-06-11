using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PokerDL.Models
{
    [DataContract]
    public class BaseEntity
    {
    }
    [DataContract]
    public class BaseIdentityEntity : BaseEntity
    {
        [DataMember]
        public int Id { get; set; }

    }
}
