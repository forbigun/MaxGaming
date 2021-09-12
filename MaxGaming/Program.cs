using MaxGaming.Files;
using MaxGaming.Notification;
using MaxGaming.Parse;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MaxGaming
{
	internal class Program
	{
		private static readonly string _titleId = "ArtikelnamnFalt";
		private static readonly NotificationPrinter _printer = new NotificationPrinter();
		private static readonly string _logoPath = "logo.jpg";
		private static readonly string _prodImagePath = "image.jpg";

		private async static Task Main(string[] args)
		{
			try
			{
				var urlList = args is null || args.Length == 0
					? new string[2]
					: args;

				if (args is null || args.Length == 0)
				{
					urlList[0] = "https://us.maxgaming.com/us/wireless-mouses/model-o-wireless-gaming-mouse-white";
					urlList[1] = "https://us.maxgaming.com/us/wireless-mouses/model-o-wireless-gaming-mouse-black";
				}

				do
				{
					var website = new Parse.MaxGaming(_titleId, urlList);

					_printer.ShowInfo("Идет поиск скидок...", InformationType.info);

					var docs = await website.GetDocumentsAsync();
					var products = website.GetProducts(docs);

					if (products.Count > 0)
					{
						foreach (var product in products)
						{
							FileManager.PrepareTempFiles(product.Image, _prodImagePath, _logoPath);
							if (product.DiscountPrice > 0)
							{
								var windowsNotification = new WindowsNotification(_prodImagePath, _logoPath);
								windowsNotification.ShowInfo(product.Name, InformationType.Success);
								urlList = urlList.Where(url => url != product.Url).ToArray();
								await Task.Delay(5000);
							}
							FileManager.DeleteTempFiles(_prodImagePath, _logoPath);
						}
					}
					else
					{
						_printer.ShowInfo("Не удалось найти товар", InformationType.Error);
					}

					if (urlList.Length < 1)
					{
						break;
					}

					await Task.Delay(GetMinutes(30));
				}
				while (true);
			}
			catch (Exception ex)
			{
				_printer.ShowInfo(ex.Message, InformationType.Error);
			}
			finally
			{
				_printer.ShowInfo("Поиск завершен!", InformationType.info);
				Console.ReadKey();
			}
		}

		private static int GetMinutes(int minutes) => 1000 * 60 * minutes;
	}
}