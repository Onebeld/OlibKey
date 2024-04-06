namespace OlibKey.Core.Helpers;

public static class HttpClientUtils
{
	/// <summary>
	/// Downloads the content from the specified link
	/// </summary>
	/// <param name="client"><see cref="HttpClient"/></param>
	/// <param name="uri">Content link</param>
	/// <returns>Byte array of downloaded data</returns>
	public static async Task<byte[]> DownloadDataTaskAsync(this HttpClient client, Uri uri)
	{
		await using Stream stream = await client.GetStreamAsync(uri);
		using MemoryStream memoryStream = new();

		await stream.CopyToAsync(memoryStream);

		return memoryStream.ToArray();
	}
}