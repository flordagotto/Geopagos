namespace DTOs
{
    public abstract class PlayerDTO
    {
        public string Name { get; set; }

        public int Skill { get; set; }
    }

    public class FemalePlayerDTO : PlayerDTO
    {
        public int ReactionTime { get; set; }
    }

    public class MalePlayerDTO : PlayerDTO
    {
        public int Strength { get; set; }

        public int Speed { get; set; }
    }
}
