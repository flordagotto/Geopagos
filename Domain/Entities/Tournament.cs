using Common.Enums;

namespace Domain.Entities
{
    public class Tournament : Entity
    {
        public Gender Type { get; }

        public DateTime Created { get; }

        public bool IsFinished { get; private set; }

        public Guid WinnerId { get; private set; }

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
    }
}
