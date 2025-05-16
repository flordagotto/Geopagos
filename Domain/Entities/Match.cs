using Common.Helpers;

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

        public static Match Create(int round, Player player1, Player player2, Tournament tournament)
        {
            if (round <= 0)
                throw new ArgumentException("Round must be greater than 0.");

            if (player1.Id == player2.Id)
                throw new ArgumentException("A player cannot play against themselves.");

            return new Match(round, player1, player2, tournament);
        }

        public Player PlayMatch()
        {
            var player1Score = Player1.GetGameValue();
            var player2Score = Player2.GetGameValue();

            if (player1Score > player2Score)
            {
                Winner = Player1;
            }
            else if (player2Score > player1Score)
            {
                Winner = Player2;
            }
            else
            {
                var player1Luck = RandomGenerator.GenerateLuckProbability();
                var player2Luck = RandomGenerator.GenerateLuckProbability();

                Winner = player1Luck >= player2Luck ? Player1 : Player2;
            }

            return Winner;
        }
    }
}
