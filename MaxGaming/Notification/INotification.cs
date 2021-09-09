namespace MaxGaming.Notification
{
	internal interface INotification
	{
		void ShowInfo(string message, InformationType informationType);
	}
}