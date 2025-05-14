namespace DAL.Entities
{
    public class Match
    {
        public Guid Id { get; set; }

        public int Round { get; set; }

        public Guid Player1Id { get; set; }

        public Guid Player2Id { get; set; }

        public Guid WinnerId { get; set; }

        public Guid TournamentId { get; set; }


        public Player Player1 { get; set; }

        public Player Player2 { get; set; }

        public Player Winner { get; set; }

        public Tournament Tournament { get; set; }
    }
}
