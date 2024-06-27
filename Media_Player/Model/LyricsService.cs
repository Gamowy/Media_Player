using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

public class LyricsService
{
    private readonly Uri baseAddress = new Uri("https://api.lyrics.ovh/v1/");
    private readonly HttpClient httpClient;

    public LyricsService()
    {
        httpClient = new HttpClient { BaseAddress = baseAddress };
    }

    public async Task<string> FetchLyricsAsync(string artist, string title)
    {
        try
        {
            string requestUri = $"{Uri.EscapeDataString(artist)}/{Uri.EscapeDataString(title)}";
            using (var response = await httpClient.GetAsync(requestUri))
            {
                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    // Parse the JSON to extract the "lyrics" field
                    var jsonResponse = JObject.Parse(responseData);
                    string lyrics = jsonResponse["lyrics"].ToString();

                    // Check if the lyrics start with "Paroles de la chanson" and remove it
                    if (lyrics.StartsWith("Paroles de la chanson"))
                    {
                        int firstLineEndIndex = lyrics.IndexOf("\n") + 1; // Find the end of the first line
                        lyrics = lyrics.Substring(firstLineEndIndex); // Remove the first line
                    }

                    return lyrics;
                }
                else
                {
                    // Handle non-success status codes
                    return "Lyrics not found.";
                }
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions
            return $"Error fetching lyrics: {ex.Message}";
        }
    }
}