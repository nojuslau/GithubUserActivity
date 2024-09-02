using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace GithubUserActivity;

class Program
{
    public static async Task Main(string[] args)
    {
        using HttpClient client = new();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
        client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
        await ProcessRepositoriesAsync(client, args[0]);
    }


    static async Task ProcessRepositoriesAsync(HttpClient client, string username)
    {
        var json = await client.GetStringAsync(
             $"https://api.github.com/users/{username}/events");

        var eventsArray = JArray.Parse(json);
        Dictionary<string, int> events = new Dictionary<string, int>();

        foreach (var eventObj in eventsArray)
        {
            string eventType = eventObj["type"]?.ToString();
            if (eventType == "PushEvent")
            {
                if (!events.ContainsKey(eventType))
                        events.Add(eventType, 0);
                events[eventType]++;
            }
            else if (eventType == "PullRequestEvent")
            {
                if (!events.ContainsKey(eventType))
                    events.Add(eventType, 0);
                events[eventType]++;
            }
            else if (eventType == "CreateEvent")
            {
                if (!events.ContainsKey(eventType))
                    events.Add(eventType, 0);
                events[eventType]++;
            }
        }

        Console.WriteLine("Output:");
        foreach (var item in events)
        {
            Console.WriteLine(item.Key + ": " + item.Value);
        }
    }
}