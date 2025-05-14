using Common.Enums;

namespace DAL.Entities
{
    public abstract class Player
    {
        public string Name { get; set; }

        public int Skill { get; set; }

        public Gender Gender { get; set; }
    }


    public class FemalePlayer : Player { 
        public int ReactionTime { get; set; }
    }


    public class MalePlayer : Player
    {
        public int Strength { get; set; }

        public int Speed { get; set; }
    }
}
