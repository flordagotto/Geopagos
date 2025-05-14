using Common.Enums;

namespace Domain.Entities
{
    public abstract class Player
    {
        protected Player(string name, int skill, Gender gender)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.");

            if (skill < 0 || skill > 100)
                throw new ArgumentException("Skill must be between 0 and 100.");

            Id = Guid.NewGuid();
            Name = name;
            Skill = skill;
            Gender = gender;
        }

        public Guid Id { get; }

        public string Name { get; }

        public int Skill { get; }

        public Gender Gender { get; }
    }

    public class FemalePlayer : Player
    {
        private FemalePlayer(Guid id, string name, int skill, int reactionTime) : base(name, skill, Gender.Female)
        {
            ReactionTime = reactionTime;
        }

        public static FemalePlayer Create(string name, int skill, int reactionTime)
        {
            if (reactionTime < 0)
                throw new ArgumentException("Reaction time must be greater than 0.");

            return new FemalePlayer(Guid.NewGuid(), name, skill, reactionTime);
        }

        public int ReactionTime { get; }
    }


    public class MalePlayer : Player
    {
        private MalePlayer(string name, int skill, int strength, int speed) : base(name, skill, Gender.Male)
        {
            Strength = strength;
            Speed = speed;
        }

        public static MalePlayer Create(string name, int skill, int strength, int speed)
        {
            if (strength < 0)
                throw new ArgumentException("Strength must be greater than 0.");

            if (speed < 0)
                throw new ArgumentException("Speed must be greater than 0.");

            return new MalePlayer(name, skill, strength, speed);
        }

        public int Strength { get; }

        public int Speed { get; }
    }
}
