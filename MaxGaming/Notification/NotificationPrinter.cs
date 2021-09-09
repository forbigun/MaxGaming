using System;

namespace MaxGaming.Notification
{
	public class NotificationPrinter : INotification
	{
		public void ShowInfo(string message, InformationType informationType)
		{
			var showInfoType = true;

			switch (informationType)
			{
				case InformationType.Error:
					Console.ForegroundColor = ConsoleColor.Red;
					break;

				case InformationType.Success:
					Console.ForegroundColor = ConsoleColor.Green;
					break;

				case InformationType.NotFound:
					Console.ForegroundColor = ConsoleColor.DarkYellow;
					break;

				case InformationType.Warning:
					Console.ForegroundColor = ConsoleColor.Yellow;
					break;

				case InformationType.info:
					showInfoType = false;
					Console.ForegroundColor = ConsoleColor.Gray;
					break;
			}

			if (showInfoType)
			{
				Console.WriteLine($"{informationType}! {message}");
			}
			else
			{
				Console.WriteLine(message);
			}
			Console.ResetColor();
		}
	}
}