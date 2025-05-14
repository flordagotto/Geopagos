using Common.Enums;

namespace Domain.Entities
{
    public class Tournament
    {
        public Guid Id { get;}

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
            Id = Guid.NewGuid();
            Type = type;
            Created = DateTime.Now;
            IsFinished = false;
            _players = [.. players];
        }

        public Tournament Create(Gender type, IEnumerable<Player> players)
        {
            if(players == null || !players.Any()) 
                throw new ArgumentNullException("A tournament should have at least 2 players.");

            return new Tournament(type, players);
        }
    }
}
