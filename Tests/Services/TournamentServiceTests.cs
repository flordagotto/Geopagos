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
            var tournament1 = CreateTournamentsAndSetupMapper();
            var tournament2 = CreateTournamentsAndSetupMapper();

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
            var tournament1 = CreateTournamentsAndSetupMapper();
            var tournament2 = CreateTournamentsAndSetupMapper();
            var tournament3 = CreateTournamentsAndSetupMapper(isFinished: true);

            var tournaments = new List<Tournament>();
            tournaments.Add(tournament3);

            _mocker.GetMock<ITournamentRepository>().Setup(x => x.GetFiltered(null, null, null, true)).ReturnsAsync(tournaments);

            // Act
            var result = await _service.GetByFilter(new TournamentFilterDto() { IsFinished = true});

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

        private Tournament CreateTournamentsAndSetupMapper(bool isFinished = false)
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
                IsFinished = isFinished,
                Players = new List<PlayerDTO> { player1DTO, player2DTO },
                Type = Common.Enums.Gender.Female
            };

            _mocker.GetMock<IMapper>()
                .Setup(t => t.Map<TournamentDTO>(tournament))
                .Returns((Tournament t) => tournamentDTO);

            return tournament;
        }
    }
}