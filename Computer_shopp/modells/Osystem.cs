using Org.BouncyCastle.Bcpg.OpenPgp;

namespace Computer_shopp.modells
{
    public class Osystem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int version { get; set; }
        public DateTime RegisterTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
