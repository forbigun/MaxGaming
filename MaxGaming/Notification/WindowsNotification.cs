using Microsoft.Toolkit.Uwp.Notifications;
using System;

namespace MaxGaming.Notification
{
	internal class WindowsNotification : INotification
	{
		private readonly string _imgPath;
		private readonly string _logoPath;

		public WindowsNotification(string imgPath, string logoPath)
		{
			_imgPath = imgPath;
			_logoPath = logoPath;
		}

		public void ShowInfo(string message, InformationType informationType)
		{
			switch (informationType)
			{
				case InformationType.Success:
					new ToastContentBuilder()
						.AddInlineImage(new Uri($@"{Environment.CurrentDirectory}\{_imgPath}"))
						.AddAppLogoOverride(new Uri($@"{Environment.CurrentDirectory}\{_logoPath}"), ToastGenericAppLogoCrop.Circle)
						.AddText("НАКОНЕЦ-ТО!")
						.AddText("Кажется, тут появилась скидка на интересный тебе товар!")
						.AddText(message)
						.Show();
					break;

				default:
					throw new InvalidOperationException("Неизвестный тип уведомления");
			}
		}
	}
}