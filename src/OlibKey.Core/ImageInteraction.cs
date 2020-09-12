using System;
using System.Net;
using System.Threading.Tasks;

namespace OlibKey.Core
{
	public static class ImageInteraction
	{
		public static async Task<string> DownloadImage(string url)
		{
			using WebClient client = new WebClient();

			return Convert.ToBase64String(await client.DownloadDataTaskAsync("https://www.google.com/s2/favicons?domain=" + url));
		}
	}
}
