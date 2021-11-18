using System;
using System.Threading.Tasks;
using Catalog.Controllers;
using Catalog.Entities;
using Catalog.DTO;
using Catalog.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using FluentAssertions;

namespace Catalog.Testing
{
  public class ItemControllerTests
  {
    private readonly Mock<IItemsRepository> repoStub = new Mock<IItemsRepository>();

    private readonly Random random = new Random();

    [Fact]
    public async Task GetItemAsync_WithUnexistingItem_ReturnsNotFound()
    {
      //ARRANGE setup
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
      //ARRANGE
      var expectedItem = CreateRandomItem();

      repoStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
        .ReturnsAsync(expectedItem);
      var controller = new ItemsController(repoStub.Object);
      //ACT
      var result = await controller.GetItemAsync(Guid.NewGuid());

      //ASSERT
      result.Value.Should().BeEquivalentTo(
          expectedItem, 
          options => options.ComparingByMembers<Item>()
        );
      Assert.IsType<ItemDto>(result.Value);
      var dto = (result as ActionResult<ItemDto>).Value;
      Assert.Equal(expectedItem.Id, dto.Id);
    }


    private Item CreateRandomItem()
    {
      return new Item()
      {
        Id = Guid.NewGuid(),
        Name = Guid.NewGuid().ToString(),
        Price = random.Next(1000),
        CreatedDate = DateTimeOffset.UtcNow
      };
    }

  }
}
