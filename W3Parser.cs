namespace W3Tools {
	using System;
	using System.Collections.Generic;

	public class W3Parser {
		private Dictionary<string, string[]> _variables = new Dictionary<string, string[]>();
		private string Path { get; set; }
		public char LeftLimiter { get; set; }
		public char RightLimiter { get; set; }

		public W3Parser() {
			this.LeftLimiter = '[';
			this.RightLimiter = ']';
		}

		public W3Parser(string path) {
			this.Path = path;
			this.LeftLimiter = '[';
			this.RightLimiter = ']';
			this.Load();
		}

		public W3Parser(File file) {
			this.Path = file.Path;
			this.Load();
		}

		public void Load() {
			if (this.Path != null && File.Exists(this.Path) && !File.IsEmpty(this.Path)) {
				foreach (string setting in new File(this.Path).ReadAsArray()) {
					string varName;
					string[] configs;
					int beginning, ending;
					if (setting.Contains(" ")) {
						varName = setting.Split(" ")[0];
						beginning = setting.IndexOf(LeftLimiter) + 1;
						ending = setting.IndexOf(RightLimiter);

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

		public void Write(string variableName, string value, bool append = true) {
			if (variableName == null) throw new ArgumentNullException(nameof(variableName));
			if (value == null) throw new ArgumentNullException(nameof(value));
			if (this.Path == null) return;
			File configFile = new File(this.Path);
			if (configFile.IsEmpty()) {
				configFile.WriteLine("");
			}
			configFile.Write($"{variableName} => [\"{value}\"];");
		}

		public string[] Get(string key) => this._variables[key];

		public string Get(string key, int index) => this._variables[key][index];
	}
}
