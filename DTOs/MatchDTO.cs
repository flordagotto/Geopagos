namespace DTOs
{
    public class MatchDTO
    {
        public Guid Id {  get; set; }

        public int Round { get; set; }

        public PlayerDTO Player1 { get; set; }

        public PlayerDTO Player2 { get; set; }

        public PlayerDTO? Winner { get; set; }

        public TournamentDTO Tournament { get; set; }
    }
}
