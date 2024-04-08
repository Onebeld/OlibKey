using System.Text.Json;

namespace OlibKey.Core.Other;

public struct Update
{
	public Version Version;
	public string Link;
	public DateTime PublishedAt;
	public string? Body;
}

public static class Updater
{
	private const string Url = "https://api.github.com/repos/Onebeld/OlibKey/releases/latest";

	public static async Task<Update?> GetUpdate()
	{
		using HttpClient client = new();
		client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; AcmeInc/1.0)");
		
		HttpResponseMessage response = await client.GetAsync(Url);

		if (!response.IsSuccessStatusCode) return null;

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
			
		if (version is null || link is null || publishedAt is null) return null;

		return new Update
		{
			Version = version,
			Link = link,
			PublishedAt = publishedAt.Value,
			Body = body
		};
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