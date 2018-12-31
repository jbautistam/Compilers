using System;

namespace Bau.Libraries.LibTokenizer.Interpreter.Symbols
{
	/// <summary>
	///		Tabla de símbolos
	/// </summary>
	public class SymbolTable
	{
		public SymbolTable(string id)
		{
			ID = id;
		}

		/// <summary>
		///		Identificador de la tabla de símbolos
		/// </summary>
		public string ID { get; }

		/// <summary>
		///		Variables
		/// </summary>
		public Variables.VariablesCollection Variables { get; } = new Variables.VariablesCollection();
	}
}
