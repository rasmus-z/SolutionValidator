﻿#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="CSharpFormattingOptionsExtension.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.CodeInspection
{
	#region using...
	using System;
	using System.Text;
	using Catel.Text;
	using ICSharpCode.NRefactory.CSharp;

	#endregion

	public static class StringExtension
	{
		public static string Replace(this string @this, string oldValue, string newValue, StringComparison comparison)
		{
			var result = new StringBuilder();

			var previousIndex = 0;
			var index = @this.IndexOf(oldValue, comparison);
			while (index != -1)
			{
				result.Append(@this.Substring(previousIndex, index - previousIndex));
				result.Append(newValue);
				index += oldValue.Length;

				previousIndex = index;
				index = @this.IndexOf(oldValue, index, comparison);
			}
			result.Append(@this.Substring(previousIndex));

			return result.ToString();
		}
	}
}