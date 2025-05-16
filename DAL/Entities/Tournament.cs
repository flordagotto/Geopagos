using Common.Enums;

namespace DAL.Entities
{
    public class Tournament
    {
        public Tournament() { }

        public Guid Id { get; set; }

        public Gender Type { get; set; }

        public DateTime Created { get; set; }

        public bool IsFinished { get; set; }

        public Guid? WinnerId { get; set; }

        public Player? Winner { get; set; }

        public ICollection<PlayersByTournament> Players { get; set; } = [];
        public ICollection<Match> Matches { get; set; } = [];
    }
}
