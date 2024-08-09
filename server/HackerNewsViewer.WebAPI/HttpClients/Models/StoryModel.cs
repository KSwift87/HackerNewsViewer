namespace HackerNewsViewer.WebAPI.HttpClients.Models
{
    public record StoryModel
    {
        public int Id { get; init; }
        public int Score { get; init; }
        public string Title { get; init; }
        public string Text { get; init; }
        public string Url { get; init; }
    }
}
