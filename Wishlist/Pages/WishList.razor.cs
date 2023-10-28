using LiteDB;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.UI.Xaml.Automation.Peers;
using MudBlazor;
using Wishlist.Models;
using static MudBlazor.CategoryTypes;

#nullable enable

namespace Wishlist.Pages;
public partial class WishList {
	[Inject] public ILiteCollection<WishListItem> Collection { get; init; } = default!;
	[Inject] public NavigationManager Navigationmanager { get; init; } = default!;

	private WishListItem[] SourceData => Collection.FindAll().OrderByDescending(item => item.AddedDate).ToArray();
	private ICollection<WishListItem> FilteredSource => SourceData.Where(item => _selectedChip is null ? true : item.StoreName == _selectedChip.Text).ToArray();
	private List<string> Chips => SourceData.Select(e => e.StoreName).Distinct().ToList();

	private MudChip? _selectedChip = null;

	private void Delete(WishListItem item) {
		
	}
}
