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

namespace Tests.Services
{
    [TestFixture]
    public class PlayerServiceTests
    {
        AutoMocker _mocker;

        IPlayerService _service;


        [SetUp]
        public void Setup()
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

        [Test]
        public async Task Create_HappyPath()
        {
            // Arrange
            var newPlayerDTO = new NewPlayerDTO { Gender = Common.Enums.Gender.Male, Name = RandomGenerator.GenerateRandomName(), Skill = 88, Speed = 50, Strength = 100};
            var newPlayer = MalePlayer.Create(newPlayerDTO.Name, newPlayerDTO.Skill, newPlayerDTO.Strength.Value, newPlayerDTO.Speed.Value);
            _mocker.GetMock<IPlayerRepository>().Setup(x => x.Add(newPlayer));

            // Act
            await _service.Create(newPlayerDTO);

            // Assert
            _mocker.GetMock<IPlayerRepository>().Verify(x => x.Add(It.IsAny<Player>()), Times.Once());
        }

        [Test]
        public async Task Create_WhenGenderIsNotDefined_ShouldThrowArgumentException()
        {
            // Arrange
            var newPlayerDTO = new NewPlayerDTO { Gender = (Gender)2 };

            // Act && assert
            var act = async () => await _service.Create(newPlayerDTO);

            await act.Should().ThrowAsync<ArgumentException>("The gender must be 0 (male) or 1 (female)");

            _mocker.GetMock<IPlayerRepository>().Verify(x => x.Add(It.IsAny<Player>()), Times.Never());
        }

        [Test]
        public async Task Create_WhenHasIncorrectDataForGender_ShouldThrowArgumentException()
        {
            // Arrange
            var newPlayerDTO = new NewPlayerDTO { Gender = Gender.Male, Name = RandomGenerator.GenerateRandomName(), Skill = 88, ReactionTime = 55, Speed = 50, Strength = 100 };

            // Act && assert
            var act = async () => await _service.Create(newPlayerDTO);

            await act.Should().ThrowAsync<ArgumentException>();

            _mocker.GetMock<IPlayerRepository>().Verify(x => x.Add(It.IsAny<Player>()), Times.Never());
        }

        [Test]
        public async Task Create_WhenDataIsMissing_ShouldThrowArgumentException()
        {
            // Arrange
            var newPlayerDTO = new NewPlayerDTO { Gender = Gender.Male, Skill = 88, Speed = 50, Strength = 100 };

            // Act && assert
            var act = async () => await _service.Create(newPlayerDTO);

            await act.Should().ThrowAsync<ArgumentException>("Name is required.");

            _mocker.GetMock<IPlayerRepository>().Verify(x => x.Add(It.IsAny<Player>()), Times.Never());
        }

        [Test]
        public async Task Create_WhenDataIsWrong_ShouldThrowArgumentException()
        {
            // Arrange
            var newPlayerDTO = new NewPlayerDTO { Gender = Gender.Male, Name = RandomGenerator.GenerateRandomName(), Skill = 88, Speed = 50, Strength = -5 };

            // Act && assert
            var act = async () => await _service.Create(newPlayerDTO);

            await act.Should().ThrowAsync<ArgumentException>("Strength must be greater than 0.");

            _mocker.GetMock<IPlayerRepository>().Verify(x => x.Add(It.IsAny<Player>()), Times.Never());
        }

        private Player CreatePlayerAndSetupMapper()
        {
            var player = FemalePlayer.Create(RandomGenerator.GenerateRandomName(), 80, 75);

            var playerDTO = new FemalePlayerDTO { Name = player.Name, Skill = 80, ReactionTime = 75, Gender = Gender.Female };
             // TODO: mejorar esto

            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<PlayerDTO>(player))
                .Returns((Player p) => playerDTO);

            return player;
        }
    }
}