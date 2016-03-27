using System.Runtime.Serialization;

namespace LoremIpsum
{

    public class LoremIpsum
    {
        public Feed Feed { get; set; }
    }

    [DataContract]
    public class Feed
    {
        [DataMember(Name ="lipsum")]
        public string Lipsum { get; set; }

        [DataMember(Name = "generated")]
        public string Generated { get; set; }

        [DataMember(Name = "donatelink")]
        public string DonateLink { get; set; }

        [DataMember(Name = "creditlink")]
        public string CreditLink { get; set; }

        [DataMember(Name = "creditname")]
        public string CreditName { get; set; }
    }

}
