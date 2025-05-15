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

namespace Tests.Services
{
    [TestFixture]
    public class PlayerServiceTests
    {
        AutoMocker _mocker;

        IPlayerService _service;


        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _mocker = new AutoMocker();

            _service = _mocker.CreateInstance<PlayerService>();
        }

        [Test]
        public async Task GetAll_WhenThereArePlayers_ShouldReturnListOfPlayers()
        {
            // Arrange
            var player1 = CreatePlayerAndSetupMapper();
            var player2 = CreatePlayerAndSetupMapper();

            var players = new List<Player>();
            players.Add(player1);
            players.Add(player2);

            _mocker.GetMock<IPlayerRepository>().Setup(x => x.GetAll()).ReturnsAsync(players);

            // Act
            var result = await _service.GetAll();

            // Assert
            var playersResult = result.ToList();

            playersResult[0].Name.Should().Be(player1.Name);
            playersResult[0].Skill.Should().Be(player1.Skill);
            playersResult[0].Gender.Should().Be(player1.Gender);

            playersResult[1].Name.Should().Be(player2.Name);
            playersResult[1].Skill.Should().Be(player2.Skill);
            playersResult[1].Gender.Should().Be(player2.Gender);
        }

        [Test]
        public async Task GetAll_WhenThereAreNoPlayers_ShouldReturnAnEmptyList()
        {
            // Act
            var result = await _service.GetAll();

            // Assert
            result.Should().BeEmpty();
        }

        private Player CreatePlayerAndSetupMapper()
        {
            var player = FemalePlayer.Create(RandomGenerator.GenerateRandomName(), 80, 75);

            var playerDTO = new FemalePlayerDTO { Name = player.Name, Skill = 80, ReactionTime = 75, Gender = Common.Enums.Gender.Female };
             // TODO: mejorar esto

            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<PlayerDTO>(player))
                .Returns((Player p) => playerDTO);

            return player;
        }
    }
}