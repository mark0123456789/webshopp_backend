namespace Computer_shopp.modells.DTO
{
    public class UpdateNewComputerDTO
    {
        public string? Brand { get; set; }
        public string? Type { get; set; }
        public double Display { get; set; }
        public int memory { get; set; }
        public Osystem? Osys { get; set; }
    }
}
