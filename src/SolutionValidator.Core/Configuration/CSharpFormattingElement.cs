﻿#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="CSharpFormattingElement.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.Configuration
{
	#region using...
	using System;
	using System.Configuration;
	using System.IO;
	using Catel.Logging;
	using Properties;

	#endregion

	[UsedImplicitly]
	public class CSharpFormattingElement : ConfigurationElement
	{
		#region Constants
		private const string OptionsFilePathAttributeName = "optionsFilePath";
		private const string DefaultFormattingOptionSetAttributeName = "DefaultFormattingOptionSetName";
		private const string OptionsFilePathDefaultValue = "csharpformatting.xml";
		private static readonly ILog Logger = LogManager.GetCurrentClassLogger();
		#endregion

		#region Properties
		[ConfigurationProperty(OptionsFilePathAttributeName, DefaultValue = OptionsFilePathDefaultValue)]
		public string OptionsFilePath
		{
			get { return (string) base[OptionsFilePathAttributeName]; }
		}

		[ConfigurationProperty(DefaultFormattingOptionSetAttributeName, DefaultValue = "Orcomp")]
		public string DefaultFormattingOptionSetName
		{
			get { return (string) base[OptionsFilePathAttributeName]; }
		}

		public FormattingOptionSet DefaultFormattingOptionSet
		{
			get
			{
				FormattingOptionSet result;
				if (Enum.TryParse(DefaultFormattingOptionSetName, true, out result))
				{
					return result;
				}
				return FormattingOptionSet.VisualStudio;
			}
		}

		[ConfigurationProperty(SolutionValidatorConfigurationSection.CheckAttributeName, DefaultValue = "true")]
		public bool Check
		{
			get { return (bool) base[SolutionValidatorConfigurationSection.CheckAttributeName]; }
		}

		public bool IsDefaultOptionsFilePath
		{
			get { return OptionsFilePathDefaultValue.Equals(OptionsFilePath); }
		}

		public string EvaluatedOptionsFilePath()
		{
			try
			{
				var folder = Path.GetDirectoryName(SolutionValidatorConfigurationSection.ConfigFilePath);
				var combine = Path.Combine(folder, OptionsFilePath);
				return Path.GetFullPath(combine);
			}
			catch (Exception e)
			{
				Logger.Error(e, Resources.FolderStructureElement_EvaluatedDefinitionFilePath_Error_getting_EvaluatedDefinitionFilePath);
				return Path.GetFullPath(OptionsFilePath);
			}
		}
		#endregion
	}
}