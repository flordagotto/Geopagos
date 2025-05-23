using AutoMapper;
using Common.Enums;
using Common.Helpers;
using DAL.Repositories;
using Domain.Entities;
using DTOs;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;
using Services.Services;
using Tournament = Domain.Entities.Tournament;

namespace Tests.Services
{
    [TestFixture]
    public class TournamentServiceTests
    {
        AutoMocker _mocker;

        ITournamentService _service;


        [SetUp]
        public void Setup()
        {
            _mocker = new AutoMocker();

            _service = _mocker.CreateInstance<TournamentService>();
        }

        [Test]
        public async Task GetByFilter_WhenThereAreNoFilters_ShouldCallGetFilteredWithoutFilters()
        {
            // Arrange
            var tournament1 = CreateFemaleTournamentAndSetupMapper();
            var tournament2 = CreateFemaleTournamentAndSetupMapper();

            var tournaments = new List<Tournament>();
            tournaments.Add(tournament1);
            tournaments.Add(tournament2);

            _mocker.GetMock<ITournamentRepository>().Setup(x => x.GetFiltered(null, null, null, null)).ReturnsAsync(tournaments);

            // Act
            var result = await _service.GetByFilter(new TournamentFilterDto());

            // Assert
            _mocker.GetMock<ITournamentRepository>().Verify(x => x.GetFiltered(null, null, null, null), Times.Once);
        }

        [Test]
        public async Task GetByFilter_WhenFilteringByIsFinished_ShouldCallGetFilteredWithFilter()
        {
            // Arrange
            var tournament1 = CreateFemaleTournamentAndSetupMapper();
            var tournament2 = CreateFemaleTournamentAndSetupMapper();
            var tournament3 = CreateFemaleTournamentAndSetupMapper(isFinished: true);

            var tournaments = new List<Tournament>();
            tournaments.Add(tournament3);

            _mocker.GetMock<ITournamentRepository>().Setup(x => x.GetFiltered(null, null, null, true)).ReturnsAsync(tournaments);

            // Act
            var result = await _service.GetByFilter(new TournamentFilterDto() { IsFinished = true });

            // Assert
            _mocker.GetMock<ITournamentRepository>().Verify(x => x.GetFiltered(null, null, null, true), Times.Once);
        }

        [Test]
        public async Task GetAll_WhenThereAreNoTournaments_ShouldReturnAnEmptyList()
        {
            // Act
            var result = await _service.GetByFilter(new TournamentFilterDto());

            // Assert
            result.Should().BeEmpty();
        }

        [Test]
        public async Task Create_HappyPath()
        {
            // Arrange
            var player1 = CreatePlayer(Gender.Female);
            var player2 = CreatePlayer(Gender.Female);

            var playersList = new List<Player> { player1, player2 };

            var playersDTOsList = MapToPlayerDTOs(playersList);

            var playersIds = playersDTOsList.Select(x => x.Id).ToList();

            var newTournament = new NewTournamentDTO { Type = Common.Enums.Gender.Female, Players = playersIds };

            _mocker.GetMock<IPlayerRepository>().Setup(x => x.GetByIds(playersIds)).ReturnsAsync(playersList);

            // Act
            await _service.Create(newTournament);

            // Assert
            _mocker.GetMock<ITournamentRepository>().Verify(x => x.Add(It.IsAny<Tournament>()), Times.Once);
        }

        [Test]
        public async Task Create_WhenGenderIsNotDefined_ShouldThrowArgumentException()
        {
            // Arrange
            var newTournament = new NewTournamentDTO { Type = (Gender)2, Players = new List<Guid>() };

            // Act && assert
            var act = async () => await _service.Create(newTournament);

            await act.Should().ThrowAsync<ArgumentException>("The gender must be 0 (male) or 1 (female)");

            _mocker.GetMock<ITournamentRepository>().Verify(x => x.Add(It.IsAny<Tournament>()), Times.Never());
        }

        [Test]
        public async Task Create_WhenPlayersAreRepeated_ShouldThrowArgumentException()
        {
            // Arrange
            var playerId = Guid.NewGuid();
            var playersIds = new List<Guid> { playerId, playerId };

            var newTournament = new NewTournamentDTO { Type = Gender.Female, Players = playersIds };

            // Act && assert
            var act = async () => await _service.Create(newTournament);

            await act.Should().ThrowAsync<ArgumentException>("The players can not be repeated.");

            _mocker.GetMock<ITournamentRepository>().Verify(x => x.Add(It.IsAny<Tournament>()), Times.Never());
        }

        [Test]
        public async Task Create_WhenAmountOfPlayersIsLessThanTwo_ShouldThrowArgumentNullException()
        {
            // Arrange
            var newTournament = new NewTournamentDTO { Type = Gender.Female };

            // Act && assert
            var act = async () => await _service.Create(newTournament);

            await act.Should().ThrowAsync<ArgumentNullException>();

            _mocker.GetMock<ITournamentRepository>().Verify(x => x.Add(It.IsAny<Tournament>()), Times.Never());
        }

        [Test]
        public async Task Create_WhenAmountOfPlayersIsWrong_ShouldThrowArgumentException()
        {
            // Arrange
            var player1 = CreatePlayer(Gender.Female);
            var playersList = new List<Player> { player1 };

            var playersDTOsList = MapToPlayerDTOs(playersList);

            var playersIds = playersDTOsList.Select(x => x.Id).ToList();

            var newTournament = new NewTournamentDTO { Type = Gender.Female, Players = playersIds };

            // Act && assert
            var act = async () => await _service.Create(newTournament);

            await act.Should().ThrowAsync<ArgumentException>("The amount of players in a tournament should be power of two");

            _mocker.GetMock<ITournamentRepository>().Verify(x => x.Add(It.IsAny<Tournament>()), Times.Never());
        }

        [Test]
        public async Task Create_WhenListOfPlayersIsWrong_ShouldThrowArgumentException()
        {
            // Arrange
            var player1 = CreatePlayer(Gender.Female);
            var player2 = CreatePlayer(Gender.Male);
            var playersList = new List<Player> { player1, player2 };

            var playersDTOsList = MapToPlayerDTOs(playersList);

            var playersIds = playersDTOsList.Select(x => x.Id).ToList();

            var newTournament = new NewTournamentDTO { Type = Gender.Female, Players = playersIds };

            // Act && assert
            var act = async () => await _service.Create(newTournament);

            await act.Should().ThrowAsync<ArgumentException>("All players must have the same gender as the tournament.");

            _mocker.GetMock<ITournamentRepository>().Verify(x => x.Add(It.IsAny<Tournament>()), Times.Never());
        }

        [Test]
        public async Task Create_WhenPlayersAreMissing_ShouldThrowArgumentException()
        {
            // Arrange
            var player1 = CreatePlayer(Gender.Female);
            var player2 = CreatePlayer(Gender.Female);

            var playersList = new List<Player> { player1, player2 };

            var playersDTOsList = MapToPlayerDTOs(playersList);

            var playersIds = playersDTOsList.Select(x => x.Id).ToList();
            playersIds.Add(Guid.NewGuid());

            _mocker.GetMock<IPlayerRepository>().Setup(x => x.GetByIds(playersIds)).ReturnsAsync(playersList);

            var newTournament = new NewTournamentDTO { Type = Gender.Female, Players = playersIds };

            // Act && assert
            var act = async () => await _service.Create(newTournament);

            var result = await act.Should().ThrowAsync<ArgumentException>();
            result.Which.Message.Should().StartWith("Some players not found:");

            _mocker.GetMock<ITournamentRepository>().Verify(x => x.Add(It.IsAny<Tournament>()), Times.Never());
        }

        [Test]
        public async Task Create_WhenPlayerDoesNotExist_ShouldThrowArgumentException()
        {
            // Arrange
            var playersIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

            var newTournament = new NewTournamentDTO { Type = Gender.Female, Players = playersIds };

            // Act && assert
            var act = async () => await _service.Create(newTournament);

            await act.Should().ThrowAsync<ArgumentException>("No valid players found for the tournament.");

            _mocker.GetMock<ITournamentRepository>().Verify(x => x.Add(It.IsAny<Tournament>()), Times.Never());
        }

        [Test]
        public async Task Start_HappyPath()
        {
            // Arrange
            var tournament = CreateFemaleTournamentAndSetupMapper();

            _mocker.GetMock<ITournamentRepository>()
                .Setup(x => x.GetById(tournament.Id))
                .ReturnsAsync(tournament);

            _mocker.GetMock<IMatchRepository>()
                .Setup(x => x.Add(It.IsAny<Domain.Entities.Match>()));

            _mocker.GetMock<ITournamentRepository>()
                .Setup(x => x.SetWinner(It.IsAny<Guid>(), It.IsAny<Guid>()));

            // Act 
            var winner = await _service.StartTournament(tournament.Id);

            // Assert

            _mocker.GetMock<ITournamentRepository>()
                .Verify(x => x.GetById(tournament.Id), Times.Once);

            _mocker.GetMock<IMatchRepository>()
                .Verify(x => x.Add(It.IsAny<Domain.Entities.Match>()), Times.Exactly(3));

            _mocker.GetMock<ITournamentRepository>()
                .Verify(x => x.SetWinner(tournament.Id, winner.Id), Times.Once);

            _mocker.GetMock<IUnitOfWork>()
                .Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task Start_WhenTournamentDoesNotExist_ShouldThrowException()
        {
            // Act && assert
            var act = async () => await _service.StartTournament(Guid.NewGuid());

            await act.Should().ThrowAsync<Exception>("Tournament not found");

            _mocker.GetMock<IMatchRepository>()
                .Verify(x => x.Add(It.IsAny<Domain.Entities.Match>()), Times.Never);

            _mocker.GetMock<ITournamentRepository>()
                .Verify(x => x.SetWinner(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);

            _mocker.GetMock<IUnitOfWork>()
                .Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task Start_WhenTournamentIsFinished_ShouldThrowException()
        {
            // Arrange
            var tournament = TournamentFactory.LoadFromPersistance(Guid.NewGuid(), Gender.Female, DateTime.Now, true, null, new List<Player>(), new List<Domain.Entities.Match>());

            _mocker.GetMock<ITournamentRepository>()
                .Setup(x => x.GetById(tournament.Id))
                .ReturnsAsync(tournament);

            // Act && assert
            var act = async () => await _service.StartTournament(tournament.Id);

            await act.Should().ThrowAsync<Exception>("The tournament is already finished.");

            _mocker.GetMock<IMatchRepository>()
                .Verify(x => x.Add(It.IsAny<Domain.Entities.Match>()), Times.Never);

            _mocker.GetMock<ITournamentRepository>()
                .Verify(x => x.SetWinner(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);

            _mocker.GetMock<IUnitOfWork>()
                .Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        private Tournament CreateFemaleTournamentAndSetupMapper(bool isFinished = false)
        {
            var player1 = CreatePlayer(Gender.Female);
            var player2 = CreatePlayer(Gender.Female);
            var player3 = CreatePlayer(Gender.Female);
            var player4 = CreatePlayer(Gender.Female);
            var playersList = new List<Player> { player1, player2, player3, player4 };

            var tournament = Tournament.Create(Gender.Female, playersList);

            var tournamentDTO = new TournamentDTO
            {
                Created = tournament.Created,
                IsFinished = isFinished,
                Players = MapToPlayerDTOs(playersList),
                Type = Gender.Female
            };

            _mocker.GetMock<IMapper>()
                .Setup(t => t.Map<TournamentDTO>(tournament))
                .Returns((Tournament t) => tournamentDTO);

            return tournament;
        }

        private static Player CreatePlayer(Gender gender)
        {
            if (gender == Gender.Male)
                return MalePlayer.Create(RandomGenerator.GenerateRandomName(), 85, 60, 99);

            return FemalePlayer.Create(RandomGenerator.GenerateRandomName(), 80, 75);
        }

        private static List<PlayerDTO> MapToPlayerDTOs(List<Player> players)
        {
            return players.Select<Player, PlayerDTO>(p =>
            {
                if (p is MalePlayer male)
                {
                    return new MalePlayerDTO
                    {
                        Id = male.Id,
                        Name = male.Name,
                        Skill = male.Skill,
                        Strength = male.Strength,
                        Speed = male.Speed,
                        Gender = Gender.Male
                    };
                }

                if (p is FemalePlayer female)
                {
                    return new FemalePlayerDTO
                    {
                        Id = female.Id,
                        Name = female.Name,
                        Skill = female.Skill,
                        ReactionTime = female.ReactionTime,
                        Gender = Gender.Female
                    };
                }

                throw new InvalidOperationException("Unknown player type.");
            }).Cast<PlayerDTO>().ToList();
        }
    }
}