namespace TheGreatC.Domain.Models
{
    public class CommandArgumentsDetail
    {
        public int RequiredArgs { get; set; }
        public int OptionalArgs { get; set; }
        public int ProvidedArgs { get; set; }
    }
}