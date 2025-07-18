using Api.Controllers;
using Azure;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Text;

namespace Tests
{
    [TestClass]
    public class MediaStorageUnitTests
    {
        [TestMethod]
        public async Task ListAllFiles_ReturnsOk()
        {
            // Arrange
            var mockService = new Mock<IMediaStorageService>();
            mockService.Setup(service => service.GetAllFiles()).ReturnsAsync(new List<string>());
            var controller = new MediaStorageController(mockService.Object);

            // Act
            var result = await controller.ListAllFiles();

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task ListAllFiles_ReturnsBadRequest()
        {
            // Arrange
            var mockService = new Mock<IMediaStorageService>();
            mockService.Setup(service => service.GetAllFiles()).ThrowsAsync(new RequestFailedException("test"));
            var controller = new MediaStorageController(mockService.Object);
            // Act
            var result = await controller.ListAllFiles();
            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(result);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }


        [TestMethod]
        public async Task UploadFileMedia_ReturnsOk()
        {
            // Arrange
            // Create and initialize a new IFormFile
            var content = "Hello World from a Fake File";
            var fileName = "test.txt";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            IFormFile formFile = new FormFile(stream, 0, stream.Length, "id_from_form", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "text/plain"
            };

            var mockService = new Mock<IMediaStorageService>();
            mockService.Setup(service => service.UploadFile(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync("test");
            var controller = new MediaStorageController(mockService.Object);

            // Act
            var result = await controller.UploadFileMedia(formFile);

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(result);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task UploadFileMedia_ReturnsBadRequest()
        {
            // Arrange
            var mockService = new Mock<IMediaStorageService>();
            mockService.Setup(service => service.UploadFile(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync("test");
            var controller = new MediaStorageController(mockService.Object);

            // Act
            var result = await controller.UploadFileMedia(It.IsAny<IFormFile>());

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(result);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }



        [TestMethod]
        public async Task DeleteFileMedia_ReturnsNoContent()
        {
            // Arrange
            var mockService = new Mock<IMediaStorageService>();
            mockService.Setup(service => service.DeleteFile(It.IsAny<string>())).ReturnsAsync(true);
            var controller = new MediaStorageController(mockService.Object);

            // Act
            var result = await controller.DeleteFileMedia(It.IsAny<string>());

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public async Task DeleteFileMedia_ReturnsNotFound()
        {
            // Arrange
            var mockService = new Mock<IMediaStorageService>();
            mockService.Setup(service => service.DeleteFile(It.IsAny<string>())).ReturnsAsync(false);
            var controller = new MediaStorageController(mockService.Object);
            // Act
            var result = await controller.DeleteFileMedia(It.IsAny<string>());
            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
    }
}
