using Avalonia.Media.Imaging;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace OlibKey.Core
{
    public static class ImageInteraction
    {
		public static async Task<Bitmap> DownloadImage(string url)
		{
			using WebClient client = new WebClient();
			byte[] imageBytes = await client.DownloadDataTaskAsync("https://www.google.com/s2/favicons?domain=" + url);

			await using var ms = new MemoryStream(imageBytes);
			return new Bitmap(ms);
		}
	}
}
