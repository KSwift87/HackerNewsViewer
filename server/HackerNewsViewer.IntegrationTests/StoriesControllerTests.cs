using HackerNewsViewer.WebAPI.Controllers;
using HackerNewsViewer.WebAPI.HttpClients;
using HackerNewsViewer.WebAPI.HttpClients.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HackerNewsViewer.IntegrationTests
{
    public class StoriesControllerTests
    {
        public StoriesControllerTests()
        {
            ServicesProvider.WireUpDependencyInjection();
        }

        [Fact]
        public async Task GetStoriesAsync_UnusedNumberOfStories_Returns30Stories()
        {
            var controller = new StoriesController(
                ServicesProvider.GetRequiredService<IHackerNewsHttpClient>(),
                ServicesProvider.GetRequiredService<ILogger<StoriesController>>()
            );

            IActionResult result = await controller.GetStoriesAsync(StoryType.NewStories, 1);

            Assert.NotNull(result);
        }

        [Theory]
        [InlineData(StoryType.NewStories, 1, 30)]
        [InlineData(StoryType.NewStories, 2, 10)]
        [InlineData(StoryType.NewStories, 6, 20)]
        public async Task GetStoriesAsync_XNumberOfStories_ReturnsXStories(StoryType type, int page, int storiesPerPage)
        {
            var controller = new StoriesController(
                ServicesProvider.GetRequiredService<IHackerNewsHttpClient>(),
                ServicesProvider.GetRequiredService<ILogger<StoriesController>>()
            );

            IActionResult result = await controller.GetStoriesAsync(type, page, storiesPerPage);

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

            var refinedResult = (OkObjectResult)result;
            Assert.IsType<List<StoryModel>>(refinedResult.Value);

            var refinedValue = (List<StoryModel>)refinedResult.Value;
            Assert.True(refinedValue.Count() == storiesPerPage);
        }
    }
}