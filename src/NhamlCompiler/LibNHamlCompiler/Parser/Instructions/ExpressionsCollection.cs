using System;
using System.Collections.Generic;

namespace Bau.Libraries.NhamlCompiler.Parser.Instructions
{
	/// <summary>
	///		Colección de <see cref="ExpressionBase"/>
	/// </summary>
	internal class ExpressionsCollection : List<ExpressionBase>
	{
		/// <summary>
		///		Obtiene una cadena de depuración
		/// </summary>
		internal string GetDebugInfo()
		{
			string debug = "";

				// Añade los datos a la cadena de depuración
				foreach (ExpressionBase expression in this)
				{
					if (!string.IsNullOrEmpty(debug))
						debug += " # ";
					debug += expression.GetDebugInfo();
				}
				// Devuelve la cadena de depuración
				return debug;
		}

		/// <summary>
		///		Clona la colección de expresiones
		/// </summary>
		internal ExpressionsCollection Clone()
		{
			ExpressionsCollection expressions = new ExpressionsCollection();

				// Clona las expresiones
				foreach (ExpressionBase expression in this)
					expressions.Add(expression.Clone());
				// Devuelve la colección
				return expressions;
		}
	}
}
