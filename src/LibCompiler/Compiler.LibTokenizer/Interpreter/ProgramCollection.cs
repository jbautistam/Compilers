using System;
using System.Linq;
using System.Collections.Generic;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.Compiler.LibTokenizer.Interpreter
{
	/// <summary>
	///		Colección de <see cref="Program"/>
	/// </summary>
	public class ProgramCollection : List<Program>
	{
		/// <summary>
		///		Obtiene una función
		/// </summary>
		public Program Search(string name)
		{ 
			return this.FirstOrDefault<Program>(program => program.ID.EqualsIgnoreCase(name));
		}
	}
}
