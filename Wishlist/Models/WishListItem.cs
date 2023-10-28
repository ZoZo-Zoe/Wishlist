namespace Wishlist.Models;
public partial class WishListItem
{
	public Guid Id { get; set; } = Guid.NewGuid();
	public DateTime AddedDate { get; set; } = DateTime.Now;
	public string Name { get; set; } = string.Empty;
	public string Link { get; set; } = string.Empty;
	public decimal Price { get; set; } = 0M;
	public string Notes { get; set; } = string.Empty;

	public string Cost
	{
		get => $"€ {Price}";
		set => _ = value;
	}

	public string StoreName
	{ 
		get => StoreNameCalculation();
		set => _ = value; 
	}

	private string StoreNameCalculation()
	{
		try
		{
			ReadOnlySpan<char> chars = Link;
			int startIndex = chars.IndexOf("://") + 3;
			int endIndex;

			if (chars[startIndex..].StartsWith("www."))
			{
				startIndex += 4;
			}

			endIndex = startIndex + chars[startIndex..].IndexOf('.');

			Span<char> finalString = stackalloc char[endIndex - startIndex];
			chars[startIndex..endIndex].CopyTo(finalString);

			finalString[0] = char.ToUpper(finalString[0]);
			return finalString.ToString();
		}
		catch
		{
			return string.Empty;
		}
	}
}