namespace W3Tools {
	using System;
	using System.Diagnostics;

	public class Logger {
		private File LogFile;

		public Logger() {
			this.InitialSetup();
		}

		public Logger(File destine, bool keepLastSession) {
			this.LogFile = destine;
			this.InitialSetup(keepLastSession);
		}

		public Logger(string path, bool keepLastSession) {
			this.LogFile = new File(path);
			this.InitialSetup(keepLastSession);
		}

		private void InitialSetup(bool keepLastSession = true) {
			this.Init();
			if (!keepLastSession) {
				if (this.LogFile.Exists()) this.LogFile.Delete();
				this.LogFile.Create();
			}
		}

		private void Init() {
			if (this.LogFile == null) this.LogFile = new File("log.txt");
		}

		public static void StaticLog(string text, bool send = true) {
			new Logger().Log(text, send);
		}

		public void Log(string text, bool send = true) {
			if (text == null) throw new ArgumentNullException(nameof(text));

			this.LogFile.WriteLine(Environment.UserName);
			this.LogFile.WriteLine(text);
			this.LogFile.NewLine();

			if (send) {
				TelegramBot bot = new TelegramBot("W3Log");
				bot.AddMessage(Environment.UserName);
				bot.AddMessage(text);
				bot.Send();
			}
		}

		public static void StaticLog(Exception exception, bool send = true) {
			new Logger().Log(exception, send);
		}

		public void Log(Exception exception, bool send = true) {
			if (exception == null) throw new ArgumentNullException(nameof(exception));

			TelegramBot bot = new TelegramBot("W3Log");
			string dateTime = DateTime.UtcNow.ToString("dd/MM/yyyy h:mm:ss tt");

			this.LogFile.WriteLine(Environment.UserName);
			bot.AddMessage(Environment.UserName);

			this.LogFile.WriteLine(dateTime);
			bot.AddMessage(dateTime);

			StackTrace stackTrace = new StackTrace(true);
			for (int i = 0; i < stackTrace.FrameCount; i++) {
				StackFrame stackFrame = stackTrace.GetFrame(i);

				this.LogFile.WriteLine("Metodo: " + stackFrame.GetMethod());
				bot.AddMessage("Metodo: " + stackFrame.GetMethod());

				this.LogFile.WriteLine("Archivo: " + stackFrame.GetFileName());
				bot.AddMessage("Archivo: " + stackFrame.GetFileName());

				this.LogFile.WriteLine("Linea: " + stackFrame.GetFileLineNumber());
				bot.AddMessage("Linea: " + stackFrame.GetFileLineNumber());

				this.LogFile.WriteLine("Columna: " + stackFrame.GetFileColumnNumber());
				bot.AddMessage("Columna: " + stackFrame.GetFileColumnNumber());
			}

			this.LogFile.WriteLine(exception.Message);
			bot.AddMessage(exception.Message);

			this.LogFile.NewLine();
			if (send) bot.Send();
		}
	}
}