namespace W3Tools {
	using System;

	public abstract class P {
		public static void Print(object toPrint) {
			Console.Write(toPrint.ToString());
		}

		public static void PrintLine(object toPrint) {
			Console.WriteLine(toPrint.ToString());
		}

		public static void Print(File file) {
			if (file != null && file.Path != null) {
				if (file.Exists()) {
					string line;
					while ((line = file.ReadLine()) != null) {
						P.PrintLine(line);
					}
				}
				else {
					P.Err("File does not exists");
				}
			}
			else {
				P.Err("File's path is not defined");
			}
		}

		public static void Err(object error) {
			Console.Error.WriteLine(error.ToString());
		}

		public static void Err(Exception exception) {
			P.Err(exception.Message);
		}
	}
}