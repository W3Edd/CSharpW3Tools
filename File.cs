namespace W3Tools {
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	public class File {
		public string Path { get; set; }
		public StreamReader Reader { get; set; }

		public File() {
			this.Path = null;
			this.Reader = null;
		}

		public File(string path) {
			this.Path = path;
			this.Reader = null;
		}

		public File(File file, bool copyReader = false) {
			this.Path = file.Path;
			this.Reader = copyReader ? file.Reader : null;
		}

		public bool Create() {
			return Create(this.Path);
		}

		public static bool Create(string path) {
			bool created = false;
			try {
				if (System.IO.File.Create(path) != null) created = true;
			}
			catch (IOException exception) {
				P.Err(exception);
			}
			return created;
		}

		public bool Delete() {
			return Delete(this.Path);
		}

		public static bool Delete(string path) {
			bool deleted = false;
			try {
				System.IO.File.Delete(path);
				deleted = true;
			}
			catch (Exception e) {
				P.Err(e);
			}
			return deleted;
		}

		public string GetName() {
			return GetName(this.Path);
		}

		public static string GetName(string path) {
			return System.IO.Path.GetFileName(path);
		}

		public string GetNameNoExtension() {
			return GetNameNoExtension(this.Path);
		}

		public static string GetNameNoExtension(string path) {
			return System.IO.Path.GetFileNameWithoutExtension(path);
		}

		public string GetExtension() {
			return GetExtension(this.Path);
		}

		public static string GetExtension(string path) {
			return System.IO.Path.GetExtension(path);
		}

		public bool Rename(string newPath) {
			return Rename(this.Path, newPath);
		}

		public static bool Rename(string originalPath, string newPath) {
			bool renamed = false;
			try {
				System.IO.File.Move(originalPath, newPath);
				renamed = true;
			}
			catch (Exception e) {
				P.Err(e);
			}
			return renamed;
		}

		public bool Exists() {
			return Exists(this.Path);
		}

		public static bool Exists(string path) {
			return System.IO.File.Exists(path);
		}

		public bool InitLineReader() {
			bool initialized = false;
			if (this.Path != null) {
				this.Reader = new StreamReader(this.Path);
				initialized = true;
			}
			return initialized;
		}

		public string Read() {
			string text = null;
			try {
				text = System.IO.File.ReadAllText(this.Path);
			}
			catch (Exception e) {
				P.Err(e);
			}
			return text;
		}

		public string[] ReadAsArray() {
			List<string> lines = new List<string>();
			string line;
			while ((line = this.ReadLine()) != null) lines.Add(line);
			return lines.ToArray();
		}

		public string ReadLine() {
			string line = null;
			if (this.Reader == null) this.InitLineReader();
			try {
				if (this.Reader.Peek() != -1) line = this.Reader.ReadLine();
			}
			catch (Exception e) when (e is IOException || e is OutOfMemoryException) {
				P.Err(e);
			}
			return line;
		}

		public bool Write(string line, bool append = true) {
			bool wrote = false;
			if (this.Path != null)
				try {
					StreamWriter writer = new StreamWriter(this.Path, append);
					writer.Write(line);
					writer.Close();
					wrote = true;
				}
				catch (Exception e) {
					P.Err(e);
				}
			return wrote;
		}

		public bool WriteLine(string line, bool append = true) {
			bool wrote = false;
			if (this.Path != null)
				try {
					StreamWriter writer = new StreamWriter(this.Path, append);
					writer.WriteLine(line);
					writer.Close();
					wrote = true;
				}
				catch (Exception exception) {
					P.Err(exception);
				}
			return wrote;
		}

		public bool NewLine() {
			bool inserted = false;
			if (this.Path != null)
				try {
					StreamWriter writer = new StreamWriter(this.Path, true);
					writer.WriteLine();
					writer.Close();
					inserted = true;
				}
				catch (Exception exception) {
					P.Err(exception);
				}
			return inserted;
		}

		public bool Modify(string originalLine, string newLine, bool allOccurrences = false) {
			bool modified = false;
			if (originalLine != null && newLine != null) {
				string line;

				File temporalFile = new File("TemporalFile");
				StreamReader reader = new StreamReader(this.Path);

				bool modifyingOnAllOccurrences = false;

				while ((line = reader.ReadLine()) != null)
					if (!modifyingOnAllOccurrences && line.Equals(originalLine)) {
						temporalFile.WriteLine(newLine);
						modified = true;
						if (!allOccurrences) modifyingOnAllOccurrences = true;
					} else {
						temporalFile.WriteLine(line);
					}

				this.Delete();
				temporalFile.Rename(this.Path);
			}
			return modified;
		}

		public int GetSizeInLines() {
			return GetSizeInLines(this.Path);
		}

		public static int GetSizeInLines(string path) {
			return System.IO.File.ReadLines(path).Count();
		}

		public bool IsEmpty() {
			return IsEmpty(this.Path);
		}

		public static bool IsEmpty(string path) {
			return new FileInfo(path).Length == 0;
		}
	}
}