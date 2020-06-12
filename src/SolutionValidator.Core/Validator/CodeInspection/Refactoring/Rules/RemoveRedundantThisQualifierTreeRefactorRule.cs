﻿#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="RemoveRedundantThisQualifierRule.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.CodeInspection.Refactoring
{
	#region using...
	using Catel.Logging;
	using Configuration;
	using FolderStructure;

	#endregion

	public class RemoveRedundantThisQualifierTreeRefactorRule : TreeRefactorRule<RemoveRedundantThisQualifierRewriter>
	{
		private static readonly ILog Logger = LogManager.GetCurrentClassLogger();

		public RemoveRedundantThisQualifierTreeRefactorRule(IncludeExcludeCollection sourceFileFilters, IFileSystemHelper fileSystemHelper, string fileNamePattern = "*.cs", bool isBackupEnabled = true) : base(sourceFileFilters, fileSystemHelper, fileNamePattern, isBackupEnabled)
		{
		}

		#region Message overrides
		protected override string TransformedMessage
		{
			get { return "Removed redundant 'this' qualifiers"; }
		}

		protected override string TransformingMessage
		{
			get { return "Removing redundant 'this' qualifiers"; }
		}

		protected override string TransformerMessage
		{
			get { return "Redundant 'this' qualifier remover"; }
		}

		protected override string TransformMessage
		{
			get { return "Remove redundant 'this' qualifiers"; }
		}
		#endregion

	}
}