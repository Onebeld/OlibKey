using Avalonia.Platform.Storage;

namespace OlibKey.Core.Helpers;

public static class StorageProvider
{
    public static async Task<string?> SelectFolder(string? directory = null)
    {
        IReadOnlyList<IStorageFolder> result = await OlibKeyApp.TopLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            SuggestedStartLocation = directory is null ? null : await OlibKeyApp.TopLevel.StorageProvider.TryGetFolderFromPathAsync(new Uri(directory))
        });
        
        return result.Count == 0 ? null : result[0].Path.LocalPath;
    }

    public static async Task<string?> SelectFile(
        string? title = null,
        IReadOnlyList<FilePickerFileType>? pickerFileTypes = null)
    {
        IReadOnlyList<IStorageFile> result = await OlibKeyApp.TopLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            Title = title,
            FileTypeFilter = pickerFileTypes,
            AllowMultiple = false
        });
        
        return result.Count == 0 ? null : result[0].Path.LocalPath;
    }

    public static async Task<string?> SaveFile(
        string? defaultExtension = null,
        string? title = null,
        bool showOverwritePrompt = true,
        string? suggestedFileName = null,
        IReadOnlyList<FilePickerFileType>? pickerFileTypes = null
        )
    {
        IStorageFile? result = await OlibKeyApp.TopLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions()
        {
            Title = title,
            FileTypeChoices = pickerFileTypes,
            DefaultExtension = defaultExtension,
            ShowOverwritePrompt = showOverwritePrompt,
            SuggestedFileName = suggestedFileName
        });

        return result?.Path.LocalPath;
    }
}