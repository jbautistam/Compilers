using System;

namespace Bau.Libraries.LibTokenizer.Interpreter.Instructions
{
	/// <summary>
	///		Clase con los datos de un atributo
	/// </summary>
	public class Parameter
	{
		/// <summary>
		///		Nombre del atributo
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		///		Variable
		/// </summary>
		public ExpressionsCollection Variable { get; set; } = new ExpressionsCollection();

		/// <summary>
		///		Variable (RPN)
		/// </summary>
		public ExpressionsCollection VariableRPN { get; set; } = new ExpressionsCollection();
	}
}
