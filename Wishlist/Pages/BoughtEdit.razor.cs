using LiteDB;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.UI.Xaml.Automation.Peers;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Wishlist.Models;

namespace Wishlist.Pages;
public partial class BoughtEdit {
	[Inject] public ISnackbar Snackbar { get; set; }
	[Inject] public NavigationManager NavigationManager { get; set; }
	[Inject] public ILiteCollection<BoughtItem> Collection { get; set; }
	[Inject] public ILiteCollection<WishListItem> WishListCollection { get; set; }
	[Inject] public ILiteDatabase Database { get; set; }

	[Parameter] public Guid? Id { get; set; } = null;
	[Parameter] public Guid? NewId { get; set; } = null;

	private BoughtItem _item = new();

	private readonly List<string> _deleteAttachments = new();
	private readonly List<FileResult> _addAttachments = new();

	protected override void OnAfterRender(bool firstRender) {
		base.OnAfterRender(firstRender);

		if (firstRender) {
			if (Id is not null) {
				_item = Collection.FindById(Id);
			}
			else if(NewId is not null) {
				_item = new(WishListCollection.FindById(NewId));
			}
			else {
				throw new Exception("Netiher Id's are filled. this should not be possible because you wouldnt be redirected to this page");
			}

			StateHasChanged();
		}
	}

	private void Cancel() => NavigationManager.NavigateTo(Id is null ? "/WishList" : "/Bought");

	private async Task Save() {
		if (!Validate())
			return;

		if (Id is null) {
			Collection.Insert(_item);
			WishListCollection.Delete(_item.Id);
		}
		else {
			Collection.Update(_item);
		}

		foreach(var attachment in _deleteAttachments) {
			Database.FileStorage.Delete($"{_item.Id}/{attachment}");
		}

		foreach(var attachment in _addAttachments) {
			var uploadInfo = Database.FileStorage.Upload($"{_item.Id}/{attachment.FileName}", attachment.FullPath);
		}

		NavigationManager.NavigateTo("/Bought");

		await Task.CompletedTask;
	}

	private bool Validate() {
		bool success = true;

		if (string.IsNullOrWhiteSpace(_item.Name)) {
			success = false;
			Snackbar.Add("Name cannot be empty", Severity.Error);
		}

		if (string.IsNullOrWhiteSpace(_item.Link)) {
			success = false;
			Snackbar.Add("Url cannot be empty", Severity.Error);
		}

		if (_item.Price <= 0M) {
			success = false;
			Snackbar.Add("Price should be above 0", Severity.Error);
		}

		return success;
	}

	private void DeleteAttachment(string attachment) {
		_item.Attachments.Remove(attachment);

		if(_addAttachments.Any(e => e.FileName == attachment)) {
			var removeList =  _addAttachments.Where(e => e.FileName == attachment).ToList();

			foreach (var remove in removeList) {
				_addAttachments.Remove(remove);
			}
		}
		else {
			_deleteAttachments.Add(attachment);
		}

		StateHasChanged();
	}

	private async Task UploadAttachment() {

		var file = await FilePicker.PickAsync();

		if(file == null) {
			return;
		}

		_item.Attachments.Add(file.FileName);
		_addAttachments.Add(file);

		StateHasChanged();
	}
}
