using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Wishlist.Models;

namespace Wishlist.Components;
public partial class WishListItemComponent {
	[Inject] public NavigationManager NavigationManager { get; set; } = default!;
	[Parameter] public WishListItem WishListItem { get; set; } = default!;

	[Parameter] public EventCallback<WishListItem> OnDeleted { get; set; }

	private bool _shouldUnfold = false;

	private void Edit(MouseEventArgs args) => NavigationManager.NavigateTo($"/Item/{WishListItem.Id}");
	private void Delete(MouseEventArgs args) => OnDeleted.InvokeAsync(WishListItem);

	private void MarkBought(MouseEventArgs args) => NavigationManager.NavigateTo($"/Bought/New/{WishListItem.Id}");
	private static async Task LinkClicked(WishListItem item) => await Browser.Default.OpenAsync(item.Link);
}
