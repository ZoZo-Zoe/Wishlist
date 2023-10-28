using LiteDB;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wishlist.Models;

namespace Wishlist.Pages;
public partial class ItemEdit
{
	[Inject] public ISnackbar Snackbar { get; set; }
	[Inject] public NavigationManager NavigationManager { get; set; }
	[Inject] public ILiteCollection<WishListItem> Collection { get; set; }

	[Parameter] public Guid? Id { get; set; } = null;

	private WishListItem _item = new();

	protected override void OnAfterRender(bool firstRender)
	{
		base.OnAfterRender(firstRender);

		if(firstRender )
		{
			if (Id is not null)
			{
				_item = Collection.FindById(Id);
			}

			StateHasChanged();
		}
	}

	private void Cancel() => NavigationManager.NavigateTo("/WishList");

	private void Save()
	{
		if (!Validate())
			return;

		if(Id is null)
		{
			Collection.Insert(_item);
		}
		else
		{
			Collection.Update(_item);
		}

		NavigationManager.NavigateTo("/WishList");
	}

	private bool Validate()
	{
		bool success = true;

		if (string.IsNullOrWhiteSpace(_item.Name))
		{
			success = false;
			Snackbar.Add("Name cannot be empty", Severity.Error);
		}

		if (string.IsNullOrWhiteSpace(_item.Link))
		{
			success = false;
			Snackbar.Add("Url cannot be empty", Severity.Error);
		}

		if (_item.Price <= 0M)
		{
			success = false;
			Snackbar.Add("Price should be above 0", Severity.Error);
		}

		return success;
	}
}
