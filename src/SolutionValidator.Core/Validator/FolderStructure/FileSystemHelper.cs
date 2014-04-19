﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckType.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SolutionValidator.Validator.FolderStructure
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text.RegularExpressions;
    using Rules;

    public class FileSystemHelper : IFileSystemHelper
	{
		#region IFileSystemHelper Members

		public bool Exists(string folder, string searchPattern = null)
		{
			if (string.IsNullOrEmpty(searchPattern))
			{
				return Directory.Exists(folder);
			}

			return Directory.GetFiles(folder, searchPattern).Length != 0;
		}

		public IEnumerable<string> GetFolders(string root, string pattern)
		{
			var result = new List<string>();
			root = root.Trim().ToLower();
			pattern = pattern.Replace('/', '\\').Trim('\\') + '\\';
			string[] folders = Directory.GetDirectories(root, "*", SearchOption.AllDirectories);

			string regexPattern = EscapeRegexSpecial(pattern);
		    regexPattern = regexPattern.Replace(FileSystemRule.RecursionToken, @".+");
				// Not used currently: .Replace(FileSystemRule.OneLevelWildCardToken, @"[^\\]+")

			regexPattern = string.Format(@"^{0}$", regexPattern);


			foreach (string folder in folders)
			{
				string relativePart = folder.ToLower().Replace(root, "").Trim('\\') + '\\';
				if (Regex.IsMatch(relativePart, regexPattern, RegexOptions.IgnoreCase))
				{
					result.Add(folder);
				}
			}
			return result;
		}

		#endregion

		private string EscapeRegexSpecial(string pattern)
		{
			string result = pattern;
			//const string regexSpecials = @".$^{}[]()|*+?\";
			const string regexSpecials = @".$^{}[]()|+?\";
			foreach (char regexSpecial in regexSpecials)
			{
				result = result.Replace(regexSpecial.ToString(CultureInfo.InvariantCulture), @"\" + regexSpecial);
			}
			return result;
		}
	}
}