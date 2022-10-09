using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Synonims.DataLayer.SynonimRepositories;
using Synonims.DataLayer.SynonimRepositories.Models;
using Synonims.Services.Synonims;

namespace Synonims.Services.test
{
    public class SynonimServiceTests
    {
        private ISynonimService _service;
        private ISynonimRepository _repository;

        [SetUp]
        public void Setup()
        {
            _repository = Substitute.For<ISynonimRepository>();
            _service = new SynonimService(_repository);
        }

        [Test]
        public async Task TestEmptyDb()
        {
            // Arrange
            _repository.GetSynonimsForWord("test").ReturnsNull();

            // Act
            var result = await _service.GetSynonimsForWord("test");

            // Assert
            Assert.Null(result);
        }

        [Test]
        public async Task GetSynonimsForWord()
        {
            // Arrange
            var dbResponse = new SynonimEntity()
            {
                Keyword = "test",
                Synonims = new List<String>() { "one", "two" }
            };
            _repository.GetSynonimsForWord("test").Returns(dbResponse);

            // Act
            var result = await _service.GetSynonimsForWord("test");

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Synonims.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetSynonimsForWordTransientTest()
        {
            // Arrange
            var dbResponseTest = new SynonimEntity()
            {
                Keyword = "test",
                Synonims = new List<string>() { "one", "two" }
            };
            var dbResponseOne = new SynonimEntity()
            {
                Keyword = "one",
                Synonims = new List<String>() { "test", "two" }
            };
            var dbResponseTwo = new SynonimEntity()
            {
                Keyword = "two",
                Synonims = new List<String>() { "one", "test" }
            };
            _repository.GetSynonimsForWord("test").Returns(dbResponseTest);
            _repository.GetSynonimsForWord("one").Returns(dbResponseOne);
            _repository.GetSynonimsForWord("two").Returns(dbResponseTwo);

            // Act
            var result = await _service.GetSynonimsForWord("test");

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Synonims.Count(), Is.EqualTo(2));
            Assert.That(result.TransientSynoninms.Count(), Is.EqualTo(2));
        }
       

        [Test]
        public async Task AddNewSynonim()
        {
            // Arrange
            _repository.GetSynonimsForWord("test").ReturnsNull();

            // Act
            await _service.AddSynonim("test","one");

            // Assert
            await _repository.Received(1).GetSynonimsForWord("test");
            await _repository.Received(1).InsertAsync(Arg.Is<SynonimEntity>(x => x.Keyword == "test" && x.Synonims.First() == "one"));
        }

        [Test]
        public async Task AddSynonimForExisting()
        {
            // Arrange
            var dbResponse = new SynonimEntity()
            {
                Keyword = "test",
                Synonims = new List<String>() { "one", "two" },
                Version = 1
            };
            _repository.GetSynonimsForWord("test").Returns(dbResponse);

            // Act
            await _service.AddSynonim("test", "three");

            // Assert
            await _repository.Received(1).GetSynonimsForWord("test");
            await _repository.Received(1).UpdateAsync(Arg.Is<SynonimEntity>(x => x.Keyword == "test" && x.Synonims.Last() == "three" && x.Version > 1));
        }

        [Test]
        public async Task AddNewSynonimTransiently()
        {
            // Arrange
            _repository.GetSynonimsForWord("test").ReturnsNull();

            // Act
            await _service.AddSynonim("test", "one");

            // Assert
            await _repository.Received(1).GetSynonimsForWord("test");
            await _repository.Received(1).InsertAsync(Arg.Is<SynonimEntity>(x => x.Keyword == "test" && x.Synonims.First() == "one"));
            await _repository.Received(1).InsertAsync(Arg.Is<SynonimEntity>(x => x.Keyword == "one" && x.Synonims.First() == "test"));
        }

        [Test]
        public async Task AddSynonimForExistingTransiently()
        {
            // Arrange
            var dbResponse = new SynonimEntity()
            {
                Keyword = "test",
                Synonims = new List<String>() { "one", "two" }
            };
            var dbResponseThree = new SynonimEntity()
            {
                Keyword = "three",
                Synonims = new List<String>() { "one", "two", "four", "five" }
            };
            _repository.GetSynonimsForWord("test").Returns(dbResponse);
            _repository.GetSynonimsForWord("three").Returns(dbResponseThree);

            // Act
            await _service.AddSynonim("test", "three");

            // Assert
            await _repository.Received(1).GetSynonimsForWord("test");
            await _repository.Received(1).UpdateAsync(Arg.Is<SynonimEntity>(x => x.Keyword == "test" && x.Synonims.Last() == "three"));
            await _repository.Received(1).GetSynonimsForWord("three");
            await _repository.Received(1).UpdateAsync(Arg.Is<SynonimEntity>(x => x.Keyword == "three" && x.Synonims.Last() == "test" && x.Synonims.Count() == 5));
        }
    }
}