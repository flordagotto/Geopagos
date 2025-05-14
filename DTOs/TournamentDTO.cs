using Common.Enums;

namespace DTOs
{
    public class TournamentDTO
    {
        public Gender Type { get; set; }

        public DateTime Created { get; set; }

        public bool IsFinished { get; set; }

        public Guid WinnerId { get; set; }

        public PlayerDTO? Winner { get; set; }

        public List<PlayerDTO>? Players { get; set; }

        public List<MatchDTO>? Matches { get; set; }
    }
}
