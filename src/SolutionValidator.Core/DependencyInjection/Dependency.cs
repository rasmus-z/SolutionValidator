﻿#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="Dependency.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace SolutionValidator.DependencyInjection
{
	#region using...
	using Catel.IoC;

	#endregion

	/// <summary>
	///     Very simple Inversion of Control class for global DI resolving.
	///     NOTE: This class is DI container independent
	/// </summary>
	public static class Dependency
	{
		#region Constants and Fields
		#endregion

		#region Public Methods and Operators
		/// <summary>
		///     Use this for all service / interface resolving
		/// </summary>
		/// <typeparam name="T">Interface to resolve</typeparam>
		/// <returns>Resolved concrete instance (service)</returns>
		public static T Resolve<T>()
		{
			return ServiceLocator.Default.ResolveType<T>();
		}
		#endregion
	}
}