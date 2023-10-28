using LiteDB;

namespace Wishlist.Extensions;
internal static class IServiceCollectionExtensions
{
	public static IServiceCollection AddLiteDatabase(this IServiceCollection services, string location) => 
		services.AddSingleton<ILiteDatabase>(e =>  new LiteDatabase(Path.Combine(location, "Wishlist.db")));

	public static IServiceCollection AddCollection<T>(this IServiceCollection services) => services.AddTransient(Configure<T>);
	public static ILiteCollection<T> Configure<T>(IServiceProvider provider) => provider.GetRequiredService<ILiteDatabase>().GetCollection<T>();
}
