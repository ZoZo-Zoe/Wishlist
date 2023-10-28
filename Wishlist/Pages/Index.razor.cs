using WindowsFolderPicker = Windows.Storage.Pickers.FolderPicker;
using Microsoft.AspNetCore.Components;

namespace Wishlist.Pages;
public partial class Index
{
	[Inject] public NavigationManager NavigationManager { get; set; }

	protected override async Task OnInitializedAsync()
	{
		string dbLocation = Preferences.Default.Get<string>("DatabaseLocation", null);

		if(dbLocation is null)
		{
			while(true)
			{
				string path = await PickFolder();

				if (Path.Exists(path))
				{
					Preferences.Default.Set<string>("DatabaseLocation", path);
					break;
				}
			}
		}

        NavigationManager.NavigateTo("/Wishlist");
        await base.OnInitializedAsync();
	}

	public async Task<string> PickFolder()
	{
		var folderPicker = new WindowsFolderPicker();
		// Might be needed to make it work on Windows 10
		folderPicker.FileTypeFilter.Add("*");

		// Get the current window's HWND by passing in the Window object
		var hwnd = ((MauiWinUIWindow)App.Current.Windows[0].Handler.PlatformView).WindowHandle;

		// Associate the HWND with the file picker
		WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, hwnd);

		var result = await folderPicker.PickSingleFolderAsync();

		return result?.Path;
	}
}
