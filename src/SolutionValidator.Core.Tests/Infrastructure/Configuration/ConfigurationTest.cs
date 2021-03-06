﻿#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigurationTest.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.Tests.Configuration
{
	#region using...
	using System.IO;
	using ICSharpCode.NRefactory.CSharp;
	using NUnit.Framework;
	using SolutionValidator.Configuration;

	#endregion

	[TestFixture]
	public class ConfigurationTest
	{
		private const string TestFolder = @"TestData\TestConfigurations";

		private void CheckDefaultConfiguration(SolutionValidatorConfigurationSection configuration)
		{
			// Folder structure:
			Assert.AreEqual(".folderStructure", configuration.FolderStructure.DefinitionFilePath);
			Assert.IsTrue(configuration.FolderStructure.Check);

			// C# formatting:
			Assert.AreEqual("csharpformatting.xml", configuration.CSharpFormatting.OptionsFilePath);
			Assert.IsTrue(configuration.CSharpFormatting.Check);
			Assert.AreEqual(1, configuration.CSharpFormatting.SourceFileFilters.Count);
			Assert.AreEqual(@"^src\\.*", configuration.CSharpFormatting.SourceFileFilters[0].Include);
			Assert.AreEqual(@".*\\obj\\(Debug|Release)\\.*", configuration.CSharpFormatting.SourceFileFilters[0].Exclude);

			// C# memeber reordering
			//Assert.AreEqual(0, configuration.CSharpFormatting.CodeMemberOrder.Count);
			//Assert.AreEqual(GeneratedCodeMember.StaticFields, configuration.CSharpFormatting.CodeMemberOrder[0]);

			// C# private field rename
			Assert.AreEqual("^([a-zA-Z][a-zA-Z0-9_]*$)", configuration.CSharpFormatting.PrivateFieldRename.Find);
			Assert.AreEqual("_$1", configuration.CSharpFormatting.PrivateFieldRename.Replace);
			Assert.IsTrue(configuration.ProjectFile.OutputPath.Check);

			// C# Remove redundant this qualifier
			Assert.IsTrue(configuration.ProjectFile.OutputPath.Check);

			// Project file / Output path:
			Assert.AreEqual("output", configuration.ProjectFile.OutputPath.Value);
			Assert.IsTrue(configuration.ProjectFile.OutputPath.Check);

			// Project file / required configurations:
			Assert.IsTrue(configuration.ProjectFile.RequiredConfigurations.Check);
			Assert.AreEqual(2, configuration.ProjectFile.RequiredConfigurations.Count);
			Assert.AreEqual("Debug", configuration.ProjectFile.RequiredConfigurations[0].Name);
			Assert.AreEqual("Release", configuration.ProjectFile.RequiredConfigurations[1].Name);

			//// Project file / check identical:
			Assert.IsTrue(configuration.ProjectFile.CheckIdentical.Check);
			Assert.AreEqual(2, configuration.ProjectFile.CheckIdentical.Count);
			Assert.AreEqual("AssemblyName", configuration.ProjectFile.CheckIdentical[0].PropertyName);
			Assert.AreEqual("RootNamespace", configuration.ProjectFile.CheckIdentical[0].OtherPropertyName);
			Assert.AreEqual("AssemblyName", configuration.ProjectFile.CheckIdentical[1].PropertyName);
			Assert.AreEqual("ProjectName", configuration.ProjectFile.CheckIdentical[1].OtherPropertyName);

			// Project file / check for value:
			Assert.IsTrue(configuration.ProjectFile.CheckIdentical.Check);
			Assert.AreEqual(2, configuration.ProjectFile.CheckForValue.Count);
			Assert.AreEqual("AppDesignerFolder", configuration.ProjectFile.CheckForValue[0].PropertyName);
			Assert.AreEqual("Properties", configuration.ProjectFile.CheckForValue[0].Value);
			Assert.AreEqual("Platform", configuration.ProjectFile.CheckForValue[1].PropertyName);
			Assert.AreEqual("AnyCPU", configuration.ProjectFile.CheckForValue[1].Value);
		}

		private void CheckCustomConfiguration(SolutionValidatorConfigurationSection configuration)
		{
			// Folder structure:
			Assert.AreEqual("definitionFilePath", configuration.FolderStructure.DefinitionFilePath);
			Assert.IsFalse(configuration.FolderStructure.Check);

			// C# formatting:
			Assert.AreEqual("optionsFilePath", configuration.CSharpFormatting.OptionsFilePath);
			Assert.IsFalse(configuration.CSharpFormatting.Check);
			Assert.AreEqual(1, configuration.CSharpFormatting.SourceFileFilters.Count);
			Assert.AreEqual("include", configuration.CSharpFormatting.SourceFileFilters[0].Include);
			Assert.AreEqual("exclude", configuration.CSharpFormatting.SourceFileFilters[0].Exclude);

			// C# Remove redundant this qualifier
			Assert.IsFalse(configuration.ProjectFile.OutputPath.Check);

			// C# member reorder
			//Assert.AreEqual(1, configuration.CSharpFormatting.CodeMemberOrder.Count);
			//Assert.AreEqual(GeneratedCodeMember.StaticFields, configuration.CSharpFormatting.CodeMemberOrder[0].Member);

			// C# private field rename
			Assert.AreEqual("find", configuration.CSharpFormatting.PrivateFieldRename.Find);
			Assert.AreEqual("replace", configuration.CSharpFormatting.PrivateFieldRename.Replace);
			Assert.IsFalse(configuration.ProjectFile.OutputPath.Check);

			// Project file / Output path:
			Assert.AreEqual("outputPath", configuration.ProjectFile.OutputPath.Value);
			Assert.IsFalse(configuration.ProjectFile.OutputPath.Check);

			// Project file / required configurations:
			Assert.IsFalse(configuration.ProjectFile.RequiredConfigurations.Check);
			Assert.AreEqual(1, configuration.ProjectFile.RequiredConfigurations.Count);
			Assert.AreEqual("name", configuration.ProjectFile.RequiredConfigurations[0].Name);

			// Project file / check identical:
			Assert.IsFalse(configuration.ProjectFile.CheckIdentical.Check);
			Assert.AreEqual(1, configuration.ProjectFile.CheckIdentical.Count);
			Assert.AreEqual("propertyName", configuration.ProjectFile.CheckIdentical[0].PropertyName);
			Assert.AreEqual("otherPropertyName", configuration.ProjectFile.CheckIdentical[0].OtherPropertyName);

			// Project file / check for value:
			Assert.IsFalse(configuration.ProjectFile.CheckForValue.Check);
			Assert.AreEqual(1, configuration.ProjectFile.CheckForValue.Count);
			Assert.AreEqual("propertyName", configuration.ProjectFile.CheckForValue[0].PropertyName);
			Assert.AreEqual("value", configuration.ProjectFile.CheckForValue[0].Value);
		}

		[Test]
		[TestCase("Empty1.config")]
		[TestCase("Empty2.config")]
		[TestCase("Empty3.config")]
		[TestCase("Empty4.config")]
		public void LoadEmpty(string configFileName)
		{
			var configuration = ConfigurationHelper.Load(string.Format(@"{0}\{1}", TestFolder, configFileName));
			CheckDefaultConfiguration(configuration);
		}

		[Test]
		public void LoadFullFromAppConfig()
		{
			var configuration = ConfigurationHelper.Load();
			CheckCustomConfiguration(configuration);
		}

		[Test]
		[TestCase("Empty1.config", TestFolder + @"\.folderStructure")]
		[TestCase("Full.config", TestFolder + @"\definitionFilePath")]
		[TestCase(null, "definitionFilePath")]
		public void TestEvaluatedDefinitionFilePath(string configFileName, string expected)
		{
			var configuration = ConfigurationHelper.Load(configFileName == null ? null : string.Format(@"{0}\{1}", TestFolder, configFileName));
			var actualFullPath = configuration.FolderStructure.EvaluatedDefinitionFilePath();
			var expectedFullPath = Path.Combine(Directory.GetCurrentDirectory(), expected);
			Assert.AreEqual(expectedFullPath.ToLower(), actualFullPath.ToLower());
		}

		[Test]
		public void TestNotExist()
		{
			var configuration = ConfigurationHelper.Load("Not Existing File Name");
			CheckDefaultConfiguration(configuration);
		}
	}
}