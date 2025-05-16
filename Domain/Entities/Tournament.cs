using Common.Enums;

namespace Domain.Entities
{
    public class Tournament : Entity
    {
        public Gender Type { get; }

        public DateTime Created { get; }

        public bool IsFinished { get; private set; }

        public Guid? WinnerId { get; private set; }

        public Player? Winner { get; private set; }

        public List<Match>? Matches { get; set; }

        private readonly List<Player> _players;
        public IReadOnlyCollection<Player> Players => _players.AsReadOnly();


        private Tournament(Gender type, IEnumerable<Player> players)
        {
            Type = type;
            Created = DateTime.Now;
            IsFinished = false;
            _players = [.. players];
        }

        internal Tournament(Guid id, Gender type, DateTime created, bool isFinished, Guid? winnerId, List<Player> players, List<Match> matches)
        {
            Id = id;
            Type = type;
            Created = created;
            IsFinished = isFinished;
            WinnerId = winnerId;
            _players = players;
            Matches = matches;
        }

        public static Tournament Create(Gender type, IEnumerable<Player> players)
        {
            if(players == null || !players.Any()) 
                throw new ArgumentNullException("A tournament should have at least 2 players.");

            if (!IsPowerOfTwo(players.Count()) || players.Count() < 2)
                throw new ArgumentException("The amount of players in a tournament should be power of two");

            if (players.Any(p => p.Gender != type))
                throw new ArgumentException("All players must have the same gender as the tournament.");

            return new Tournament(type, players);
        }

        public bool HasPlayer(Player player) => _players.Contains(player);

        private static bool IsPowerOfTwo(int number)
        {
            return number > 0 && (number & (number - 1)) == 0;
        }

        public Player Start()
        {
            Matches = new List<Match>();
            var currentRoundPlayers = new List<Player>(Players);
            var currentRound = 1;

            while (currentRoundPlayers.Count > 1)
            {
                var nextRoundPlayers = new List<Player>();

                for (int i = 0; i < currentRoundPlayers.Count; i += 2)
                {
                    var p1 = currentRoundPlayers[i];
                    var p2 = currentRoundPlayers[i + 1];

                    var match = Match.Create(currentRound, p1, p2, this);

                    var winner = match.PlayMatch();

                    Matches.Add(match);

                    nextRoundPlayers.Add(winner);
                }

                currentRoundPlayers = nextRoundPlayers;
                currentRound++;
            }

            return currentRoundPlayers.Single();
        }
    }

    public static class TournamentFactory
    { //todo: mejorar?
        public static Tournament LoadFromPersistance(Guid id, Gender type, DateTime created, bool isFinished, Guid? winnerId, IEnumerable<Player> players, IEnumerable<Match> matches)
        {
            return new Tournament(id, type, created, isFinished, winnerId, players.ToList(), matches.ToList());
        }
    }
}
