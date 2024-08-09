using System.Text.Json.Serialization;

namespace HackerNewsViewer.WebAPI.HttpClients
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum StoryType
    {
        TopStories,
        NewStories,
        BestStories,
        AskStories,
        ShowStories,
        JobStories
    }
}
