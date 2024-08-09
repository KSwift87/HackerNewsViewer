using HackerNewsViewer.WebAPI.HttpClients;
using Microsoft.AspNetCore.Mvc;

namespace HackerNewsViewer.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StoriesController : ControllerBase
    {
        private readonly IHackerNewsHttpClient _hackerNewsHttpClient;
        private readonly ILogger<StoriesController> _logger;

        public StoriesController(IHackerNewsHttpClient hackerNewsHttpClient,
            ILogger<StoriesController> logger)
        {
            _hackerNewsHttpClient = hackerNewsHttpClient;
            _logger = logger;
        }

        [HttpGet(nameof(GetStoriesAsync))]
        public async Task<IActionResult> GetStoriesAsync(StoryType type, int page = 1, int storiesPerPage = 30)
        {
            try
            {
                var results = await _hackerNewsHttpClient.GetStoriesAsync(type, page, storiesPerPage);

                return new OkObjectResult(results);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
