﻿#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="RepositoryInfo.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.Common
{
	public class RepositoryInfo
	{
		public RepositoryInfo(string repositoryRootPath)
		{
			RepositoryRootPath = repositoryRootPath.Trim().Replace("/", "\\");
		}

		public string RepositoryRootPath { get; private set; }
	}
}