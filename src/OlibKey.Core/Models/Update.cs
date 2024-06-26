using System.Text.Json;

namespace OlibKey.Core.Models;

/// <summary>
/// Represents an update with the details of the latest software version from a GitHub repository.
/// This class provides functionality to asynchronously fetch update data such as the version number,
/// download link, publication date, and release notes body.
/// </summary>
public class Update
{
	private const string UrlForGetUpdates = "https://api.github.com/repos/Onebeld/OlibKey/releases/latest";
	
	/// <summary>
	/// Version of the new update
	/// </summary>
	public Version Version { get; }
	
	/// <summary>
	/// Link to download the new update
	/// </summary>
	public string Link { get; }
	
	/// <summary>
	/// Time when a new update is published
	/// </summary>
	public DateTime PublishedAt { get; }
	
	/// <summary>
	/// Information about the new update (e.g. list of changes)
	/// </summary>
	public string? Body { get; }

	private Update(Version version, string link, DateTime publishedAt, string? body)
	{
		Version = version;
		Link = link;
		PublishedAt = publishedAt;
		Body = body;
	}
	
	/// <summary>
	/// Asynchronously retrieves the latest software update information from a specified GitHub repository. It parses the JSON response and constructs an <see cref="Update"/> object with the retrieved information.
	/// </summary>
	/// <returns>A Task that resolves to an <see cref="Update"/> object containing the version, download link, release date, and release notes of the latest version if successful. If the request fails or if any of the required properties are null, it returns null.</returns>
	public static async Task<Update?> Get()
	{
		using HttpClient client = new();
		client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; AcmeInc/1.0)");
		
		HttpResponseMessage response = await client.GetAsync(UrlForGetUpdates);

		if (!response.IsSuccessStatusCode) 
			return null;

		string json = await response.Content.ReadAsStringAsync();

		using JsonDocument jsonDoc = JsonDocument.Parse(json);

		JsonElement tagNameElement = jsonDoc.RootElement.GetProperty("tag_name");
		JsonElement htmlUrlElement = jsonDoc.RootElement.GetProperty("html_url");
		JsonElement bodyElement = jsonDoc.RootElement.GetProperty("body");
		JsonElement publishedAtElement = jsonDoc.RootElement.GetProperty("published_at");

		Version? version = GetVersion(tagNameElement.GetString());
		string? link = htmlUrlElement.GetString();
		string? body = bodyElement.GetString();
		DateTime? publishedAt = GetDate(publishedAtElement.GetString());
			
		if (version is null || link is null || publishedAt is null) 
			return null;

		return new Update(version, link, publishedAt.Value, body);
	}
	
	private static Version? GetVersion(string? tagName)
	{
		if (tagName is null) return null;
		
		tagName = tagName.Replace("v", "");
		
		if (Version.TryParse(tagName, out Version? version))
			return version;
		return null;
	}

	private static DateTime? GetDate(string? date)
	{
		if (DateTime.TryParse(date, out DateTime dateTime)) 
			return dateTime;
		return null;
	}
}