using LiteDB;
using Microsoft.AspNetCore.Components;
using Microsoft.UI.Xaml.Automation.Peers;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Wishlist.Models;

#nullable enable

namespace Wishlist.Pages;
public partial class Bought {
	[Inject] public NavigationManager NavigationManager { get; set; } = default!;
	[Inject] public ILiteDatabase Database { get; set; } = default!;
	[Inject] public ILiteCollection<BoughtItem> Collection { get; init; } = default!;

	private List<BoughtItem> SourceData => Collection.FindAll().OrderByDescending(item => item.BoughtDate).ToList();
	private ICollection<BoughtItem> FilteredSource => SourceData.Where(ApplyFilter).ToArray();
	private List<string> Chips => SourceData.Select(e => e.StoreName).Distinct().ToList();

	private MudChip? _selectedChip = null;
	private string? _searchQuery = null;

	private bool ApplyFilter(BoughtItem item) {
		if (_selectedChip is not null && item.StoreName != _selectedChip.Text) {
			return false;
		}

		if (!string.IsNullOrEmpty(_searchQuery) && !item.Name.Contains(_searchQuery, StringComparison.CurrentCultureIgnoreCase)) {
			return false;
		}

		return true;
	}

	//Item editing
	private void Delete(BoughtItem item) {
		foreach(string attachment in item.Attachments) {
			Database.FileStorage.Delete($"{item.Id}/{attachment}");
		}

		Collection.Delete(item.Id);
	}
	
}
