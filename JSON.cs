namespace W3Tools {
	using System.Text.Json;

	public class JSON {
		private File file;

		public JSON() { }

		public JSON(string path) {
			this.Path = path;
		}

		public JSON(File file) {
			this.file = file;
		}

		public string Path { get; set; }

		public static string Serialize<T>(T instance) {
			return JsonSerializer.Serialize(instance);
		}

		public static object Deserialize<T>(string path) {
			File file = new File(path);
			return JsonSerializer.Deserialize<T>(file.Read());
		}

		public object Deserialize<T>() {
			return Deserialize<T>(this.Path);
		}

		public void Write<T>(T instance) {
			this.file = this.Path != null
				? new File(this.Path)
				: new File($"{instance.GetType().Name}.json");

			JsonSerializerOptions options = new JsonSerializerOptions {WriteIndented = true};
			this.file.WriteLine(JsonSerializer.Serialize(instance, options));
		}
	}
}