using HtmlAgilityPack;
using MaxGaming.Model;
using MaxGaming.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MaxGaming.Parse
{
	public class MaxGaming
	{
		private readonly List<Product> _products;
		public string TitleId { get; }
		public string[] Url { get; }

		public MaxGaming(string titleId, params string[] url)
		{
			var validatedUrlList = ValidateUrlList(url);
			if (url.Length == 0 || url is null || validatedUrlList is null
				|| validatedUrlList.Count == 0 || validatedUrlList.Count() < url.Length)
			{
				var printer = new NotificationPrinter();
				printer.ShowInfo("Один или более адресов были пустыми (null), " +
					"поэтому данные с них отображены не будут", InformationType.Warning);
			}
			this.TitleId = titleId;
			_products = new List<Product>(url.Length);
			this.Url = validatedUrlList.ToArray();
		}

		private List<string> ValidateUrlList(string[] url)
		{
			var notNullUrls = new List<string>(url.Length);

			foreach (var item in url)
			{
				if (item != null)
				{
					notNullUrls.Add(item);
				}
			}
			return notNullUrls;
		}

		public async Task<List<HtmlDocument>> GetDocumentsAsync()
		{
			try
			{
				var documents = new List<HtmlDocument>(Url.Length);
				var httpClient = new HttpClient();
				foreach (var url in Url)
				{
					var html = await httpClient.GetStringAsync(url);
					var htmlDocument = new HtmlDocument();
					htmlDocument.LoadHtml(html);
					documents.Add(htmlDocument);
				}
				return documents;
			}
			catch (InvalidOperationException)
			{
				var printer = new NotificationPrinter();

				printer.ShowInfo("Не удалось получить данные со страницы.", InformationType.Error);

				return null;
			}
		}

		private void MakeDiscountList(HtmlDocument htmlDocument)
		{
			decimal price = 0;

			var productName = htmlDocument.DocumentNode
				.Descendants("h1")
				.Where(node => node.Id == TitleId)
				.Select(node => node.InnerText)
				.FirstOrDefault();

			if (GetPrice(htmlDocument, "PrisREA", out var discountPrice))
			{
				GetPrice(htmlDocument, "PrisORD", out price);
			}
			else
			{
				GetPrice(htmlDocument, "PrisBOLD", out price);
			}

			var image = htmlDocument.DocumentNode
				.SelectSingleNode(@"//*[@id=""Zoomer""]")
				.GetAttributeValue("href", "img");

			_products.Add(new Product()
			{
				Name = productName,
				Price = price,
				DiscountPrice = discountPrice,
				Url = Url[_products.Count],
				Image = $"https://us.maxgaming.com{image}",
			});
		}

		private bool GetPrice(HtmlDocument htmlDocument, string priceCssClass, out decimal price)
		{
			var priceNodes = htmlDocument.DocumentNode
					.Descendants("span")
					.Where(node => node.HasClass(priceCssClass));

			var priceString = string.Empty;

			if (priceNodes.Any())
			{
				priceString = priceNodes
					.Select(node => node.InnerText)
					.FirstOrDefault()
					.Split(';')[1]
					.Replace('.', ',');
			}

			return decimal.TryParse(priceString, out price);
		}

		public List<Product> GetProducts(IEnumerable<HtmlDocument> docs)
		{
			foreach (var doc in docs)
			{
				MakeDiscountList(doc);
			}
			return _products;
		}
	}
}