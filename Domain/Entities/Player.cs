using Common.Enums;

namespace Domain.Entities
{
    public abstract class Player : Entity
    {
        protected Player(string name, int skill, Gender gender)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.");

            if (skill < 0 || skill > 100)
                throw new ArgumentException("Skill must be between 0 and 100.");

            Name = name;
            Skill = skill;
            Gender = gender;
        }

        public string Name { get; }

        public int Skill { get; }

        public Gender Gender { get; }

        public abstract double GetGameValue();
    }

    public class FemalePlayer : Player
    {
        private FemalePlayer(string name, int skill, int reactionTime) : base(name, skill, Gender.Female)
        {
            ReactionTime = reactionTime;
        }

        public static FemalePlayer Create(string name, int skill, int reactionTime)
        {
            if (reactionTime < 0 || reactionTime > 100)
                throw new ArgumentException("All skills must be between 0 and 100.");

            return new FemalePlayer(name, skill, reactionTime);
        }

        public int ReactionTime { get; }

        public override double GetGameValue()
        {
            return Skill * ReactionTime;
        }
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
            if (strength < 0 || strength > 100 || speed < 0 || speed > 100)
                throw new ArgumentException("All skills must be between 0 and 100.");

            return new MalePlayer(name, skill, strength, speed);
        }

        public int Strength { get; }

        public int Speed { get; }

        public override double GetGameValue()
        {
            return Skill * Strength * Speed;
        }
    }
}
