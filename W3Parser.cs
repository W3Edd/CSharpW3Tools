namespace W3Tools {
	using System.Collections.Generic;

	public class W3Parser {
		private Dictionary<string, string[]> _variables = new Dictionary<string, string[]>();
		private string Path { get; set; }

		public W3Parser() { }

		public W3Parser(string path) {
			this.Path = path;
		}

		public W3Parser(File file) {
			this.Path = file.Path;
		}

		public void Load() {
			if (this.Path != null && File.Exists(this.Path) && !File.IsEmpty(this.Path)) {
				foreach (string setting in new File(this.Path).ReadAsArray()) {
					string varName;
					string[] configs;
					int beginning, ending;
					if (setting.Contains(" ")) {
						varName = setting.Split(" ")[0];
						beginning = setting.IndexOf("[") + 1;
						ending = setting.IndexOf("]");

						configs = setting.Substring(beginning, ending - beginning).Split(",");
						string[] varResults = new string[configs.Length];

						for (int i = 0; i < configs.Length; i++) {
							string config = configs[i];
							varResults[i] = Util.RemoveCharOutside(config, ' ', '"');
							varResults[i] = Util.RemoveChar(varResults[i], '"');
						}
						this._variables.Add(varName, varResults);
					}
				}
			}
		}

		public string[] Get(string key) => this._variables[key];

		public string Get(string key, int index) => this._variables[key][index];
	}
}