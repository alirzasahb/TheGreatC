namespace TheGreatC.Domain.Models
{
    public class CommandArgumentDetail
    {
        public int RequiredArgs { get; set; }
        public int OptionalArgs { get; set; }
        public int ProvidedArgs { get; set; }
    }
}