using Common.Enums;

namespace DTOs
{
    public class NewPlayerDTO
    {
        public string Name { get; set; }

        public int Skill { get; set; }

        public Gender Gender { get; set; }

        public int? ReactionTime { get; set; } 

        public int? Strength { get; set; }   

        public int? Speed { get; set; }       

    }
}
