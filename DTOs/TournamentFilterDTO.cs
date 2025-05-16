using Common.Enums;

namespace DTOs
{
    public class TournamentFilterDto
    {
        public Gender? Type { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool? IsFinished { get; set; }
    }
}
