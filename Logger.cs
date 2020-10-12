namespace W3Tools {
	using System;
	using System.Diagnostics;

	public class Logger {
		private File LogFile;

		public Logger() {
			this.InitialSetup();
		}

		public Logger(File destine, bool keepLastSession = true) {
			this.LogFile = destine;
			this.InitialSetup(keepLastSession);
		}

		public Logger(string path, bool keepLastSession = true) {
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

		public static void StaticLog(string text) {
			new Logger().Log(text);
		}

		public void Log(string text) {
			if (text == null) throw new ArgumentNullException(nameof(text));

			this.LogFile.WriteLine(Environment.UserName);
			this.LogFile.WriteLine(text);
			this.LogFile.NewLine();
		}

		public static void StaticLog(Exception exception, bool send = true) {
			new Logger().Log(exception, send);
		}

		public void Log(Exception exception, bool send = true) {
			if (exception == null) throw new ArgumentNullException(nameof(exception));
			string dateTime = DateTime.UtcNow.ToString("dd/MM/yyyy h:mm:ss tt");

			this.LogFile.WriteLine(Environment.UserName);
			this.LogFile.WriteLine(dateTime);

			StackTrace stackTrace = new StackTrace(true);
			for (int i = 0; i < stackTrace.FrameCount; i++) {
				StackFrame stackFrame = stackTrace.GetFrame(i);

				if (i == stackTrace.FrameCount - 1) {
					this.LogFile.WriteLine("Method: " + stackFrame.GetMethod());
					this.LogFile.WriteLine("File: " + stackFrame.GetFileName());
					this.LogFile.WriteLine("Line: " + stackFrame.GetFileLineNumber());
					this.LogFile.WriteLine("Column: " + stackFrame.GetFileColumnNumber());
				}
			}
			this.LogFile.WriteLine(exception.Message);
			this.LogFile.NewLine();
		}
	}
}