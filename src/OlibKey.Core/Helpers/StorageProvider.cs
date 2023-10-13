using Avalonia.Controls;
using Avalonia.Platform.Storage;
using PleasantUI.Controls;

namespace OlibKey.Core.Helpers;

public static class StorageProvider
{
    public static async Task<string?> SelectFolder(TopLevel window, string? directory = null)
    {
        IReadOnlyList<IStorageFolder> result = await window.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            SuggestedStartLocation = directory is null ? null : await window.StorageProvider.TryGetFolderFromPathAsync(new Uri(directory))
        });
        
        return result.Count == 0 ? null : result[0].Path.LocalPath;
    }

    public static async Task<string?> SelectFile(
        TopLevel window,
        string? title = null,
        IReadOnlyList<FilePickerFileType>? pickerFileTypes = null)
    {
        IReadOnlyList<IStorageFile> result = await window.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            Title = title,
            FileTypeFilter = pickerFileTypes,
            AllowMultiple = false
        });
        
        return result.Count == 0 ? null : result[0].Path.LocalPath;
    }

    public static async Task<string?> SaveFile(
        TopLevel window,
        string? defaultExtension = null,
        string? title = null,
        bool showOverwritePrompt = true,
        string? suggestedFileName = null,
        IReadOnlyList<FilePickerFileType>? pickerFileTypes = null
        )
    {
        IStorageFile? result = await window.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions()
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