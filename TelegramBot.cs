namespace W3Tools {
	using System.Net;
	using System.Web;

	public class TelegramBot {
		public TelegramBot(string groupId) {
			this.URL =
				"https://api.telegram.org/bot1098401798:AAEycvrpsUUIUb0oOcUO-_tGsvlfJEK8dVg/" +
				"sendMessage?chat_id=@" + groupId;
		}

		public string URL { get; set; }
		public string Message { get; set; }

		public void AddMessage(string message) {
			this.Message += message + "\n";
		}

		public void Send() {
			string completeUrl = this.URL + "&text=" + HttpUtility.UrlEncode(this.Message);
			WebClient client = new WebClient();
			client.DownloadData(completeUrl);
		}
	}
}