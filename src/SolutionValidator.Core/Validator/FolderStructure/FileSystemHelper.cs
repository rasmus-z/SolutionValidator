﻿#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="FileSystemHelper.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.FolderStructure
{
	#region using...
	using System.Collections.Generic;
	using System.Globalization;
	using System.IO;
	using System.Linq;
	using System.Text.RegularExpressions;

	#endregion

	public class FileSystemHelper : IFileSystemHelper
	{

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
			var folders = Directory.GetDirectories(root, "*", SearchOption.AllDirectories);

			var regexPattern = EscapeRegexSpecial(pattern);
			regexPattern = regexPattern.Replace(FileSystemRule.RecursionToken, @".+");
			// Not used currently: .Replace(FileSystemRule.OneLevelWildCardToken, @"[^\\]+")

			regexPattern = string.Format(@"^{0}$", regexPattern);

			foreach (var folder in folders)
			{
				var relativePart = folder.ToLower().Replace(root, "").Trim('\\') + '\\';
				if (Regex.IsMatch(relativePart, regexPattern, RegexOptions.IgnoreCase))
				{
					result.Add(folder);
				}
			}
			return result;
		}

		public IEnumerable<string> GetFiles(string root, string pattern)
		{
			// TODO: Implement exclude filters (for example to let exclude autogenerated files
			root = root.Trim().ToLower().Replace('/', '\\').Trim('\\') + '\\';
			return Directory.GetFiles(root, pattern, SearchOption.AllDirectories);
		}


		private string EscapeRegexSpecial(string pattern)
		{
			var result = pattern;
			//const string regexSpecials = @".$^{}[]()|*+?\";
			const string regexSpecials = @".$^{}[]()|+?\";
			foreach (var regexSpecial in regexSpecials)
			{
				result = result.Replace(regexSpecial.ToString(CultureInfo.InvariantCulture), @"\" + regexSpecial);
			}
			return result;
		}
	}
}