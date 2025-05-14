namespace Domain.Entities
{
    public class Match : Entity
    {
        public int Round { get; set; }

        public Player Player1 { get; set; }

        public Player Player2 { get; set; }

        public Player? Winner { get; set; }

        public Tournament Tournament { get; set; }

        private Match(int round, Player player1, Player player2, Tournament tournament)
        {
            Round = round;
            Player1 = player1;
            Player2 = player2;
            Tournament = tournament;
        }

        public Match Create(int round, Player player1, Player player2, Tournament tournament)
        {
            if (round <= 0)
                throw new ArgumentException("Round must be greater than 0.");

            if (player1.Id == player2.Id)
                throw new ArgumentException("A player cannot play against themselves.");

            if (!tournament.HasPlayer(player1) || !tournament.HasPlayer(player2))
                throw new InvalidOperationException("Players must belong to the tournament.");

            return new Match(round, player1, player2, tournament);
        }
    }
}
