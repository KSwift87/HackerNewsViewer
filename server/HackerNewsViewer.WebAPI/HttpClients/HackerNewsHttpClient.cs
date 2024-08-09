using HackerNewsViewer.WebAPI.HttpClients.Models;
using HackerNewsViewer.WebAPI.Services;

namespace HackerNewsViewer.WebAPI.HttpClients
{
    public interface IHackerNewsHttpClient
    {
        public Task<IEnumerable<StoryModel>> GetStoriesAsync(StoryType type, int page = 1, int storiesPerPage = 30);
        Task<IEnumerable<StoryModel>> GetStoryDetailsAsync(int[] storyIds);
    }

    internal class HackerNewsHttpClient(IHttpClientFactory httpClientFactory, ICacheService cacheService) 
        : IHackerNewsHttpClient
    {
        private HttpClient _httpClient = httpClientFactory.CreateClient(Constants.HackerNewsHttpClient);

        public async Task<IEnumerable<StoryModel>> GetStoriesAsync(StoryType type, int page = 1, int storiesPerPage = 30)
        {
            string storyType = type.ToString().ToLower();
            int[] newStoryIds = cacheService.GetItem<int[]>(storyType) ?? Array.Empty<int>();

            if (newStoryIds.Length == 0)
            {
                newStoryIds = await _httpClient.GetFromJsonAsync<int[]>($"{Constants.HackerNewsBaseURL}{storyType}.json") ?? Array.Empty<int>();

                if (newStoryIds.Length > 0)
                {
                    cacheService.AddItem(storyType, newStoryIds);
                }
            }                

            if (newStoryIds.Length > 0)
            {
                var stories = await GetStoryDetailsAsync(
                    newStoryIds.Skip(page * storiesPerPage).Take(storiesPerPage).Select(r => r).ToArray()
                );

                return stories;
            }

            return new List<StoryModel>();
        }

        public async Task<IEnumerable<StoryModel>> GetStoryDetailsAsync(int[] storyIds)
        {
            var stories = new List<StoryModel>();

            foreach (var storyId in storyIds)
            {
                var story = cacheService.GetItem<StoryModel>($"{storyId}");

                if (story == null)
                {
                    story = await _httpClient.GetFromJsonAsync<StoryModel>($"{Constants.HackerNewsBaseURL}item/{storyId}.json");
                    
                    if (story != null)
                    {
                        cacheService.AddItem($"{storyId}", story);
                    }
                }

                stories.Add(story);
            }

            return stories;
        }
    }
}
