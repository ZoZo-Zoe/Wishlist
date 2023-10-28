using LiteDB;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Wishlist.Models;

namespace Wishlist.Components;
public partial class BoughtItemComponent {
	[Inject] public NavigationManager NavigationManager { get; set; } = default!;
	[Inject] public ILiteDatabase Database { get; set; } = default!;
	[Inject] public ISnackbar Snackbar { get; set; } = default!;

	[Parameter] public BoughtItem BoughtItem { get; set; } = default!;
	[Parameter] public EventCallback<BoughtItem> OnDeleted { get; set; }

	private bool _shouldUnfold = false;

	private readonly Guid DOWNLOAD_FOLDER_GUID = Guid.Parse("374DE290-123F-4565-9164-39C4925E467B");

	private void Edit(MouseEventArgs args) => NavigationManager.NavigateTo($"/Bought/{BoughtItem.Id}");
	private void Delete(MouseEventArgs args) => OnDeleted.InvokeAsync(BoughtItem);

	private async Task DownloadAttachment(BoughtItem item, string attachment) {
		string path = Path.Combine(SHGetKnownFolderPath(DOWNLOAD_FOLDER_GUID, 0), attachment);

		LiteFileInfo<string> fileInfo = Database.FileStorage.FindById($"{item.Id}/{attachment}");
		fileInfo.SaveAs(path);

		Snackbar.Add($"{attachment} downloaded. Click to open.", Severity.Normal, config => config.Onclick = snackbar => OpenWithDefaultProgram(path));

		await Task.CompletedTask;
	}

	public static Task OpenWithDefaultProgram(string path) {
		Process fileopener = new Process() { StartInfo = new() { FileName = "explorer", Arguments = "\"" + path + "\"" } };
		fileopener.Start();
		fileopener.Dispose();

		return Task.CompletedTask;
	}

	[DllImport("shell32", CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = false)]
	private static extern string SHGetKnownFolderPath([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags, nint hToken = 0);
}
