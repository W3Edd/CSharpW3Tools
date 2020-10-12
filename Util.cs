namespace W3Tools {
	using System;

	public abstract class Util {
		public static string[] Explode(string line, char divisor) {
			return line.Split(divisor);
		}

		public static string RemoveChar(string original, char toRemove) {
			string final = "";
			foreach (char character in original) {
				if (!character.Equals(toRemove)) {
					final += character;
				}
			}
			return final;
		}

		public static string RemoveFirstChar(string original, char toRemove) {
			string final = "";
			bool removed = false;
			foreach (char character in original) {
				if (!character.Equals(toRemove)) {
					final += character;
					removed = true;
				}
				if (removed) {
					break;
				}
			}
			return final;
		}

		public static string RemoveCharOutside(string original, char toRemove, char limit) {
			string final = "";
			bool outside = false;
			foreach (char character in original) {
				if (character.Equals(limit)) {
					outside = !outside;
				}
				if (!character.Equals(toRemove)) {
					final += character;
				} else if (character.Equals(toRemove) && outside) {
					final += character;
				}
			}
			return final;
		}

		public static string RemoveString(string original, string toRemove) {
			bool removed = true;
			string result = "";

			while (removed) {
				if (original.Contains(toRemove)) {
					result = original.Remove(original.IndexOf(toRemove, StringComparison.Ordinal),
						toRemove.Length);
				}
				if (result != original) {
					original = result;
					removed = true;
				} else {
					removed = false;
				}
			}
			return original;
		}

		public static string RemoveStringOutside(string original, string toRemove, char limit) {
			string result = "";

			string[] parts = original.Split(limit);
			for (int i = 0; i < parts.Length; i++) {
				if (i % 2 == 0) {
					result += RemoveString(parts[i], toRemove);
				} else {
					result += parts[i];
				}
			}
			return result;
		}
	}
}