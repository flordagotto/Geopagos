using AutoMapper;
using Common.Helpers;
using DAL.Repositories;
using Domain.Entities;
using DTOs;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;
using Services.Services;
using Match = Domain.Entities.Match;

namespace Tests.Services
{
    [TestFixture]
    public class MatchServiceTests
    {
        AutoMocker _mocker;

        IMatchService _service;


        [SetUp]
        public void Setup()
        {
            _mocker = new AutoMocker();

            _service = _mocker.CreateInstance<MatchService>();
        }

        [Test]
        public async Task GetAll_WhenThereAreMatches_ShouldReturnListOfMatches()
        {
            // Arrange
            var match1 = CreateMatchesAndSetupMapper();
            var match2 = CreateMatchesAndSetupMapper();

            var matches = new List<Match>();
            matches.Add(match1);
            matches.Add(match2);

            _mocker.GetMock<IMatchRepository>().Setup(x => x.GetAll()).ReturnsAsync(matches);

            // Act
            var result = await _service.GetAll();

            // Assert
            var matchesResult = result.ToList();

            matchesResult[0].Player1.Name.Should().Be(match1.Player1.Name);
            matchesResult[0].Player2.Name.Should().Be(match1.Player2.Name);
            matchesResult[0].Player1.Skill.Should().Be(match1.Player1.Skill);
            matchesResult[0].Player2.Skill.Should().Be(match1.Player2.Skill);
            matchesResult[0].Player1.Gender.Should().Be(match1.Player1.Gender);
            matchesResult[0].Player2.Gender.Should().Be(match1.Player2.Gender);
            matchesResult[0].Tournament.Type.Should().Be(match2.Tournament.Type);

            matchesResult[1].Player1.Name.Should().Be(match2.Player1.Name);
            matchesResult[1].Player2.Name.Should().Be(match2.Player2.Name);
            matchesResult[1].Player1.Skill.Should().Be(match2.Player1.Skill);
            matchesResult[1].Player2.Skill.Should().Be(match2.Player2.Skill);
            matchesResult[1].Player1.Gender.Should().Be(match2.Player1.Gender);
            matchesResult[1].Player2.Gender.Should().Be(match2.Player2.Gender);
            matchesResult[1].Tournament.Type.Should().Be(match2.Tournament.Type);
        }

        [Test]
        public async Task GetAll_WhenThereAreNoMatches_ShouldReturnAnEmptyList()
        {
            // Act
            var result = await _service.GetAll();

            // Assert
            result.Should().BeEmpty();
        }

        private Match CreateMatchesAndSetupMapper()
        {
            var player1Name = RandomGenerator.GenerateRandomName();
            var player2Name = RandomGenerator.GenerateRandomName();

            var player1 = FemalePlayer.Create(player1Name, 80, 75);
            var player2 = FemalePlayer.Create(player2Name, 90, 65);

            var player1DTO = new FemalePlayerDTO { Name = player1Name, Skill = 80, ReactionTime = 75, Gender = Common.Enums.Gender.Female };
            var player2DTO = new FemalePlayerDTO { Name = player2Name, Skill = 90, ReactionTime = 65, Gender = Common.Enums.Gender.Female };
            // TODO: mejorar esto

            var playersList = new List<Player>();
            playersList.Add(player1);
            playersList.Add(player2);

            var tournament = Tournament.Create(Common.Enums.Gender.Female, playersList);

            var tournamentDTO = new TournamentDTO
            {
                Created = tournament.Created,
                IsFinished = false,
                Players = new List<PlayerDTO> { player1DTO, player2DTO },
                Type = Common.Enums.Gender.Female
            };

            var match = Match.Create(1, player1, player2, tournament);

            var dto = new MatchDTO
            {
                Round = match.Round,
                Player1 = player1DTO,
                Player2 = player2DTO,
                Tournament = tournamentDTO
            };

            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<MatchDTO>(match))
                .Returns((Match m) => dto);

            return match;
        }
    }
}