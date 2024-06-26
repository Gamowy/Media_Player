using System;
using System.Net.Http;
using System.Threading.Tasks;

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
                    // Assuming the API returns a JSON object with a "lyrics" field
                    // You might need to parse this JSON to extract the lyrics
                    return responseData; // Adjust this line based on actual API response structure
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