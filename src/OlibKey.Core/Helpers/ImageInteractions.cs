using System.Net;

namespace OlibKey.Core.Helpers;

public static class ImageInteractions
{
    /// <summary>
    /// Downloads a favicon by specific website and saves it to a base 64-format string
    /// </summary>
    /// <param name="url">Web site</param>
    /// <param name="size">Size of favicon in pixels</param>
    /// <returns>Favicon in base 64 format</returns>
    public static async Task<string?> DownloadFavicon(string url, int size = 32)
    {
        using WebClient client = new();
        
        if (url.Contains("http://") || url.Contains("https://"))
        {
            return Convert.ToBase64String(await client.DownloadDataTaskAsync(
                $"https://t3.gstatic.com/faviconV2?client=SOCIAL&type=FAVICON&fallback_opts=TYPE,SIZE,URL&url={url}&size={size}"));
        }
                
        return Convert.ToBase64String(await client.DownloadDataTaskAsync(
            $"https://t3.gstatic.com/faviconV2?client=SOCIAL&type=FAVICON&fallback_opts=TYPE,SIZE,URL&url=https://{url}&size={size}"));
    }
}