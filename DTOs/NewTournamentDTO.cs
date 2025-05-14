using Common.Enums;

namespace DTOs
{
    public class NewTournamentDTO
    {
        public Gender Type { get; set; }

        public bool IsFinished { get; set; }

        public List<Guid> Players { get; set; }
    }
}
