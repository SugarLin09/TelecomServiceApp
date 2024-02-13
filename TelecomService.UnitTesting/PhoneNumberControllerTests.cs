using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelecomService.Api.Controllers;
using TelecomService.Application.Interfaces;
using TelecomService.Application.Models;
using TelecomService.Application.Services;
using TelecomService.Core.Entities;

namespace TelecomService.UnitTesting
{
    public class PhoneNumberControllerTests
    {
        private PhoneNumberController GetPhoneNumberControllerTests(Mock<IPhoneNumberService> mockPhoneNumberService)
        {
            var mockLogger = new Mock<ILogger<PhoneNumberController>>();
            return new PhoneNumberController(mockLogger.Object, mockPhoneNumberService.Object);
        }

        [Fact]
        public async Task GetAllPhoneNumbers_ShouldReturnAllPhoneNumbers()
        {
            // Arrange
            var mockPhoneNumberService = new Mock<IPhoneNumberService>();
            var phoneNumbers = new List<PhoneNumberDto>
            {
                new PhoneNumberDto { Id = "1", PhoneNumber = "0123456789", CustomerId = "1", Active = true },
                new PhoneNumberDto { Id = "2", PhoneNumber = "0198765432", CustomerId = "2", Active = false }
            };
            mockPhoneNumberService.Setup(service => service.GetAllPhoneNumbersAsync()).ReturnsAsync(phoneNumbers);

            var controller = GetPhoneNumberControllerTests(mockPhoneNumberService);

            // Act
            var result = await controller.GetAllPhoneNumbers();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result.Result);

            var dataFromResult = (OkObjectResult)result.Result;

            var firstPhoneNumber = (dataFromResult.Value as IEnumerable<PhoneNumberDto>)?.FirstOrDefault();
            Assert.Equal(2, (dataFromResult.Value as IEnumerable<PhoneNumberDto>)?.Count());
            Assert.Equal(phoneNumbers.First().PhoneNumber, firstPhoneNumber?.PhoneNumber);
        }

        [Fact]
        public async Task GetAllPhoneNumbersByCustomerId_ShouldReturnAllPhoneNumbersByCustomerId()
        {
            // Arrange
            var mockPhoneNumberService = new Mock<IPhoneNumberService>();
            var customerId = "1";
            var phoneNumbers = new List<PhoneNumberDto>
            {
                new PhoneNumberDto { Id = "1", PhoneNumber = "0123456789", CustomerId = "1", Active = true },
                new PhoneNumberDto { Id = "2", PhoneNumber = "0198765432", CustomerId = "2", Active = false },
                new PhoneNumberDto { Id = "3", PhoneNumber = "0112223333", CustomerId = "1", Active = false }
            };
            var filteredPhoneNumbers = phoneNumbers.Where(phoneNumber => phoneNumber.CustomerId == customerId);
            mockPhoneNumberService.Setup(service => service.GetPhoneNumbersByCustomerIdAsync(customerId)).ReturnsAsync(filteredPhoneNumbers);

            var controller = GetPhoneNumberControllerTests(mockPhoneNumberService);

            // Act
            var result = await controller.GetPhoneNumbersByCustomerId(customerId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result.Result);

            var dataFromResult = (OkObjectResult)result.Result;

            var firstPhoneNumber = (dataFromResult.Value as IEnumerable<PhoneNumberDto>)?.FirstOrDefault();
            Assert.Equal(2, (dataFromResult.Value as IEnumerable<PhoneNumberDto>)?.Count());
            Assert.Equal(customerId, firstPhoneNumber?.CustomerId);
        }

        [Fact]
        public async Task AddPhoneNumber_ShouldAddPhoneNumberCorrectly()
        {
            // Arrange
            var mockPhoneNumberService = new Mock<IPhoneNumberService>();
            var phoneNumberDto = new PhoneNumberDto { PhoneNumber = "0198765432", CustomerId = "2", Active = false };
            var phoneNumber = new PhoneNumberDto
            {
                Id = Guid.NewGuid().ToString(),
                PhoneNumber = phoneNumberDto.PhoneNumber,
                CustomerId = phoneNumberDto.CustomerId,
                Active = false
            };
            mockPhoneNumberService.Setup(service => service.AddPhoneNumberAsync(It.IsAny<PhoneNumberDto>())).ReturnsAsync(phoneNumber);

            var controller = GetPhoneNumberControllerTests(mockPhoneNumberService);

            // Act
            var result = await controller.AddPhoneNumber(phoneNumberDto);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result.Result);

            var dataFromResult = (OkObjectResult)result.Result;

            var phoneNumberResult = dataFromResult.Value as PhoneNumberDto;
            Assert.NotNull(phoneNumberResult?.Id);
            Assert.Equal(phoneNumberDto.PhoneNumber, phoneNumberResult?.PhoneNumber);
        }

        [Fact]
        public async Task ActivatePhoneNumber_ShouldActivatePhoneNumberCorrectly_WhenPhoneNumberExists()
        {
            // Arrange
            var mockPhoneNumberService = new Mock<IPhoneNumberService>();
            var phoneNumberId = "1";
            var phoneNumber = new PhoneNumberDto { Id = phoneNumberId, PhoneNumber = "0123334444", CustomerId = "2", Active = false };
            var activatedPhoneNumber = phoneNumber;
            activatedPhoneNumber.Active = true;
            mockPhoneNumberService.Setup(service => service.GetPhoneNumberByIdAsync(phoneNumberId)).ReturnsAsync(phoneNumber);
            mockPhoneNumberService.Setup(service => service.ActivatePhoneNumberAsync(phoneNumberId)).ReturnsAsync(activatedPhoneNumber);

            var controller = GetPhoneNumberControllerTests(mockPhoneNumberService);

            // Act
            var result = await controller.ActivatePhoneNumber(phoneNumberId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result.Result);

            var dataFromResult = (OkObjectResult)result.Result;

            var phoneNumberResult = dataFromResult.Value as PhoneNumberDto;
            Assert.NotNull(phoneNumberResult?.Id);
            Assert.Equal(phoneNumber.Id, phoneNumberResult?.Id);
            Assert.True(phoneNumberResult?.Active);
        }

        [Fact]
        public async Task ActivatePhoneNumber_ShouldReturnNotFound_WhenPhoneNumberNotExists()
        {
            // Arrange
            var mockPhoneNumberService = new Mock<IPhoneNumberService>();
            var phoneNumberId = "2";
            mockPhoneNumberService.Setup(service => service.GetPhoneNumberByIdAsync(phoneNumberId)).ReturnsAsync(() => null);
            mockPhoneNumberService.Setup(service => service.ActivatePhoneNumberAsync(phoneNumberId));

            var controller = GetPhoneNumberControllerTests(mockPhoneNumberService);

            // Act
            var result = await controller.ActivatePhoneNumber(phoneNumberId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}
