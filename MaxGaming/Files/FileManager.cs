using MaxGaming.Properties;
using System.IO;
using System.Net;

namespace MaxGaming.Files
{
	public static class FileManager
	{
		public static void DownloadFile(string uri, string fileName)
		{
			using (var client = new WebClient())
			{
				client.DownloadFile(uri, fileName);
			}
		}

		public static void DeleteFile(string fileName)
		{
			if (File.Exists(fileName))
			{
				File.Delete(fileName);
			}
		}

		public static void PrepareTempFiles(string imgUri, string imgFileName, string logoFileName)
		{
			DownloadFile(imgUri, imgFileName);

			Resources.avatar.Save(logoFileName);
		}

		public static void DeleteTempFiles(string imgFileName, string logoFileName)
		{
			DeleteFile(imgFileName);
			DeleteFile(logoFileName);
		}
	}
}