namespace DAL.Entities
{
    public class PlayersByTournament
    {
        public Guid TournamentId { get; set; }

        public Guid PlayerId { get; set; }

        public Tournament Tournament { get; set; }

        public Player Player { get; set; }
    }
}
