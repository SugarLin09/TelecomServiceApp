using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TelecomService.Core.Entities;
using TelecomService.Infrastructure.Data;
using TelecomService.Infrastructure.Repositories;

namespace TelecomService.UnitTesting
{
    public class PhoneNumberRepositoryTests
    {
        private readonly Mock<ILogger<PhoneNumberRepository>> mockLogger;
        private AppDbContext mockAppDbContext;

        public PhoneNumberRepositoryTests()
        {
            mockLogger = new Mock<ILogger<PhoneNumberRepository>>();
            var _dbContextOptionsMock = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
            mockAppDbContext = new AppDbContext(_dbContextOptionsMock);
        }

        private void DisposeDBCOntext()
        {
            mockAppDbContext.Database.EnsureCreated();
            mockAppDbContext.Dispose();
        }

        [Fact]
        public async Task GetAllPhoneNumbersAsync_ReturnAllPhoneNumbers()
        {
            // Arrange
            var phoneNumbers = new List<PhoneNumber>
            {
                new PhoneNumber { Id = "1", PhoneNumberValue = "0123456789", CustomerId = "1", Active = true },
                new PhoneNumber { Id = "2", PhoneNumberValue = "0198765432", CustomerId = "2", Active = false }
            };

            mockAppDbContext.PhoneNumbers.AddRange(phoneNumbers);
            await mockAppDbContext.SaveChangesAsync();

            var userRepository = new PhoneNumberRepository(mockLogger.Object, mockAppDbContext);

            // Act
            var result = await userRepository.GetAllPhoneNumbersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());

            DisposeDBCOntext();
        }

        [Fact]
        public async Task GetPhoneNumberByIdAsync_ReturnCorrectPhoneNumber()
        {
            // Arrange
            var phoneNumberId = "1";
            var phoneNumbers = new List<PhoneNumber>
            {
                new PhoneNumber { Id = "1", PhoneNumberValue = "0123456789", CustomerId = "1", Active = true },
                new PhoneNumber { Id = "2", PhoneNumberValue = "0198765432", CustomerId = "2", Active = false }
            };

            mockAppDbContext.PhoneNumbers.AddRange(phoneNumbers);
            await mockAppDbContext.SaveChangesAsync();

            var userRepository = new PhoneNumberRepository(mockLogger.Object, mockAppDbContext);

            // Act
            var result = await userRepository.GetPhoneNumberByIdAsync(phoneNumberId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(phoneNumberId, result.Id);

            DisposeDBCOntext();
        }

        [Fact]
        public async Task GetPhoneNumbersByCustomerIdAsync_ReturnPhoneNumbersByCustomerId()
        {
            // Arrange
            var customerId = "1";
            var phoneNumbers = new List<PhoneNumber>
            {
                new PhoneNumber { Id = "1", PhoneNumberValue = "0123456789", CustomerId = "1", Active = true },
                new PhoneNumber { Id = "2", PhoneNumberValue = "0198765432", CustomerId = "2", Active = false },
                new PhoneNumber { Id = "3", PhoneNumberValue = "0112223333", CustomerId = "1", Active = false }
            };

            mockAppDbContext.PhoneNumbers.AddRange(phoneNumbers);
            await mockAppDbContext.SaveChangesAsync();

            var userRepository = new PhoneNumberRepository(mockLogger.Object, mockAppDbContext);

            // Act
            var result = await userRepository.GetPhoneNumbersByCustomerIdAsync(customerId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());

            DisposeDBCOntext();
        }

        [Fact]
        public async Task AddPhoneNumberAsync_AddPhoneNumberCorrrectly()
        {
            // Arrange
            var newPhoneNumber = new PhoneNumber { Id = "3", PhoneNumberValue = "0112223333", CustomerId = "1", Active = false };
            var userRepository = new PhoneNumberRepository(mockLogger.Object, mockAppDbContext);

            // Act
            var result = await userRepository.AddPhoneNumberAsync(newPhoneNumber);

            // Assert
            var phoneNumberFromDb = mockAppDbContext.PhoneNumbers.Find("3");
            Assert.NotNull(phoneNumberFromDb);
            Assert.Equal("0112223333", phoneNumberFromDb.PhoneNumberValue);

            DisposeDBCOntext();
        }

        [Fact]
        public async Task ActivatePhoneNumberAsync_ActivatePhoneNumberCorrrectly()
        {
            // Arrange
            var phoneNumberId = "4";
            var activatedPhoneNumber = new PhoneNumber { Id = phoneNumberId, PhoneNumberValue = "0123334444", CustomerId = "2", Active = true };
            mockAppDbContext.PhoneNumbers.Add(activatedPhoneNumber);
            await mockAppDbContext.SaveChangesAsync();

            var userRepository = new PhoneNumberRepository(mockLogger.Object, mockAppDbContext);

            // Act
            var result = await userRepository.ActivatePhoneNumberAsync(activatedPhoneNumber);

            // Assert
            var phoneNumberFromDb = mockAppDbContext.PhoneNumbers.Find(phoneNumberId);
            Assert.NotNull(phoneNumberFromDb);
            Assert.Equal("0123334444", phoneNumberFromDb.PhoneNumberValue);
            Assert.True(phoneNumberFromDb.Active);

            DisposeDBCOntext();
        }
    }
}
