namespace LoremIpsum
{

    public class LoremIpsum
    {
        public Feed feed { get; set; }
    }

    public class Feed
    {
        public string lipsum { get; set; }
        public string generated { get; set; }
        public string donatelink { get; set; }
        public string creditlink { get; set; }
        public string creditname { get; set; }
    }

}
