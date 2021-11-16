using System;
using System.Threading.Tasks;
using Catalog.Controllers;
using Catalog.Entities;
using Catalog.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Catalog.Testing
{
    public class ItemControllerTests
    {
        private readonly Mock<IItemsRepository> repoStub = new Mock<IItemsRepository>();
           
        [Fact]
        public async Task GetItemAsync_WithUnexistingItem_ReturnsNotFound()
        {
            //ARRANGE setup
            var repoStub = new Mock<IItemsRepository>();
            repoStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Item)null);

            var controller = new ItemsController(repoStub.Object);
            //ACT executing test 
            var result = await controller.GetItemAsync(Guid.NewGuid());
            //ASSERT
            Assert.IsType<NotFoundResult>(result.Result);
        }


        [Fact]
        public async Task GetItemAsync_WithExistingItem_ReturnsExpectedItem() 
        {

        }
    }
}
