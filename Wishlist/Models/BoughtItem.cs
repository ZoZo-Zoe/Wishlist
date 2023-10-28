using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wishlist.Models;
public class BoughtItem : WishListItem {

	public DateTime? BoughtDate { get; set; } = DateTime.Now;
	public string OrderNumber { get; set; } = string.Empty;
	public List<string> Attachments { get; set; } = new();

	public BoughtItem(WishListItem item) {
		Id = item.Id;
		AddedDate = item.AddedDate;
		Name = item.Name;
		Link = item.Link;
		Price = item.Price;
		Notes = item.Notes;
	}

    public BoughtItem()
    {
        
    }
}
