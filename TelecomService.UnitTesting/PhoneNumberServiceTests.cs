using Microsoft.Extensions.Logging;
using TelecomService.Application.Interfaces;
using TelecomService.Application.Models;
using TelecomService.Application.Services;
using TelecomService.Core.Entities;

namespace TelecomService.UnitTesting
{
    public class PhoneNumberServiceTests
    {
        private PhoneNumberService GetPhoneNumberServiceTests(Mock<IPhoneNumberRepository> mockPhoneNumberRepository,
            Mock<ICustomerRepository> mockCustomerRepository)
        {
            var mockLogger = new Mock<ILogger<PhoneNumberService>>();
            return new PhoneNumberService(mockLogger.Object, mockPhoneNumberRepository.Object, mockCustomerRepository.Object);
        }

        [Fact]
        public async Task GetAllPhoneNumbersAsync_ShouldReturnAllPhoneNumbers()
        {
            // Arrange
            var mockPhoneNumberRepository = new Mock<IPhoneNumberRepository>();
            var mockCustomerRepository = new Mock<ICustomerRepository>();
            var phoneNumbers = new List<PhoneNumber>
            {
                new PhoneNumber { Id = "1", PhoneNumberValue = "0123456789", CustomerId = "1", Active = true },
                new PhoneNumber { Id = "2", PhoneNumberValue = "0198765432", CustomerId = "2", Active = false }
            };
            var customers = new List<Customer>
            {
                new Customer { Id = "1", Name = "John Doe" },
                new Customer { Id = "2", Name = "Jane Doe" }
            };
            mockPhoneNumberRepository.Setup(repo => repo.GetAllPhoneNumbersAsync()).ReturnsAsync(phoneNumbers);
            mockCustomerRepository.Setup(repo => repo.GetAllCustomersAsync()).ReturnsAsync(customers);

            var service = GetPhoneNumberServiceTests(mockPhoneNumberRepository, mockCustomerRepository);

            // Act
            var result = await service.GetAllPhoneNumbersAsync();

            // Assert
            var firstPhoneNumber = result.FirstOrDefault();
            Assert.Equal(2, result.Count());
            Assert.Equal(phoneNumbers.First().PhoneNumberValue, firstPhoneNumber?.PhoneNumber);
        }

        [Fact]
        public async Task GetPhoneNumberByIdAsync_ShouldReturnPhoneNumber_WhenPhoneNumberExists()
        {
            // Arrange
            var mockPhoneNumberRepository = new Mock<IPhoneNumberRepository>();
            var mockCustomerRepository = new Mock<ICustomerRepository>();
            var phoneNumberId = "1";
            var phoneNumber = new PhoneNumber { Id = "1", PhoneNumberValue = "0123456789", CustomerId = "1", Active = true };
            mockPhoneNumberRepository.Setup(repo => repo.GetPhoneNumberByIdAsync(phoneNumberId)).ReturnsAsync(phoneNumber);

            var service = GetPhoneNumberServiceTests(mockPhoneNumberRepository, mockCustomerRepository);

            // Act
            var result = await service.GetPhoneNumberByIdAsync(phoneNumberId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(phoneNumberId, result.Id);
        }

        [Fact]
        public async Task GetPhoneNumbersByCustomerIdAsync_ShouldReturnPhoneNumbersByCustomerIds()
        {
            // Arrange
            var mockPhoneNumberRepository = new Mock<IPhoneNumberRepository>();
            var mockCustomerRepository = new Mock<ICustomerRepository>();
            var customerId = "1";
            var phoneNumbers = new List<PhoneNumber>
            {
                new PhoneNumber { Id = "1", PhoneNumberValue = "0123456789", CustomerId = "1", Active = true },
                new PhoneNumber { Id = "2", PhoneNumberValue = "0198765432", CustomerId = "2", Active = false },
                new PhoneNumber { Id = "3", PhoneNumberValue = "0112223333", CustomerId = "1", Active = false }
            };
            var customer = new Customer { Id = customerId, Name = "John Doe" };
            var filteredPhoneNumbers = phoneNumbers.Where(phoneNumber => phoneNumber.CustomerId == customerId);
            mockPhoneNumberRepository.Setup(repo => repo.GetPhoneNumbersByCustomerIdAsync(customerId)).ReturnsAsync(filteredPhoneNumbers);
            mockCustomerRepository.Setup(repo => repo.GetCustomerByIdAsync(customerId)).ReturnsAsync(customer);

            var service = GetPhoneNumberServiceTests(mockPhoneNumberRepository, mockCustomerRepository);

            // Act
            var result = await service.GetPhoneNumbersByCustomerIdAsync(customerId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());

            var firstPhoneNumber = result.FirstOrDefault();
            Assert.Equal(customer.Name, firstPhoneNumber?.CustomerName);
        }

        [Fact]
        public async Task AddPhoneNumberAsync_AddPhoneNumberCorrectly()
        {
            // Arrange
            var mockPhoneNumberRepository = new Mock<IPhoneNumberRepository>();
            var mockCustomerRepository = new Mock<ICustomerRepository>();
            var phoneNumberDto = new PhoneNumberDto { PhoneNumber = "0198765432", CustomerId = "2", Active = false };
            var phoneNumber = new PhoneNumber
            {
                Id = Guid.NewGuid().ToString(),
                PhoneNumberValue = phoneNumberDto.PhoneNumber,
                CustomerId = phoneNumberDto.CustomerId,
                Active = false
            };
            mockPhoneNumberRepository.Setup(repo => repo.AddPhoneNumberAsync(It.IsAny<PhoneNumber>())).ReturnsAsync(phoneNumber);

            var service = GetPhoneNumberServiceTests(mockPhoneNumberRepository, mockCustomerRepository);

            // Act
            var result = await service.AddPhoneNumberAsync(phoneNumberDto);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Id);
            Assert.Equal(phoneNumberDto.PhoneNumber, result.PhoneNumber);
        }

        [Fact]
        public async Task ActivatePhoneNumberAsync_ActivatePhoneNumberCorrectly()
        {
            // Arrange
            var mockPhoneNumberRepository = new Mock<IPhoneNumberRepository>();
            var mockCustomerRepository = new Mock<ICustomerRepository>();
            var phoneNumberId = "1";
            var phoneNumber = new PhoneNumber { Id = phoneNumberId, PhoneNumberValue = "0123334444", CustomerId = "2", Active = false };
            var activatedPhoneNumber = phoneNumber;
            activatedPhoneNumber.Active = true;

            mockPhoneNumberRepository.Setup(repo => repo.GetPhoneNumberByIdAsync(phoneNumberId)).ReturnsAsync(phoneNumber);
            mockPhoneNumberRepository.Setup(repo => repo.ActivatePhoneNumberAsync(activatedPhoneNumber)).ReturnsAsync(activatedPhoneNumber);

            var service = GetPhoneNumberServiceTests(mockPhoneNumberRepository, mockCustomerRepository);

            // Act
            var result = await service.ActivatePhoneNumberAsync(phoneNumberId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("0123334444", result.PhoneNumber);
            Assert.True(result.Active);
        }

    }
}
