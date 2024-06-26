namespace OlibKey.Core.Extensions;

public static class HttpClientExtensions
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
	
	/// <summary>
	/// Downloads a favicon by specific website and saves it to a base 64-format string
	/// </summary>
	/// <param name="client"><see cref="HttpClient"/></param>
	/// <param name="url">Web site</param>
	/// <param name="size">Size of favicon in pixels</param>
	/// <returns>Favicon in base 64 format</returns>
	public static async Task<string?> DownloadFavicon(this HttpClient client, string url, int size = 32)
	{
		try
		{
			if (url.Contains("http://") || url.Contains("https://"))
			{
				return Convert.ToBase64String(await client.DownloadDataTaskAsync(
					$"https://t3.gstatic.com/faviconV2?client=SOCIAL&type=FAVICON&fallback_opts=TYPE,SIZE,URL&url={url}&size={size}"
						.ToUri()));
			}

			return Convert.ToBase64String(await client.DownloadDataTaskAsync(
				$"https://t3.gstatic.com/faviconV2?client=SOCIAL&type=FAVICON&fallback_opts=TYPE,SIZE,URL&url=https://{url}&size={size}"
					.ToUri()));
		}
		catch (Exception)
		{
			return null;
		}
	}
}