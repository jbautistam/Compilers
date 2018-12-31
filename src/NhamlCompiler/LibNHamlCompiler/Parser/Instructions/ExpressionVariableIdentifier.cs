using System;

namespace Bau.Libraries.NhamlCompiler.Parser.Instructions
{
	/// <summary>
	///		Expresión de tipo variable
	/// </summary>
	internal class ExpressionVariableIdentifier : ExpressionBase
	{
		internal ExpressionVariableIdentifier(Tokens.Token token) : base(token)
		{
			Name = token.Content;
			IndexExpressions = new ExpressionsCollection();
			IndexExpressionsRPN = new ExpressionsCollection();
		}

		/// <summary>
		///		Clona el identificador de variable
		/// </summary>
		internal override ExpressionBase Clone()
		{
			ExpressionVariableIdentifier variable = new ExpressionVariableIdentifier(base.Token);

				// Clona las expresiones
				variable.IndexExpressions = IndexExpressions.Clone();
				variable.IndexExpressionsRPN = IndexExpressionsRPN.Clone();
				if (Member != null)
					variable.Member = Member.Clone() as ExpressionVariableIdentifier;
				// Devuelve el objeto clonado
				return variable;
		}

		/// <summary>
		///		Obtiene la información de depuración
		/// </summary>
		internal override string GetDebugInfo()
		{
			string debug = Name;

				// Añade el índice
				if (IndexExpressions != null && IndexExpressions.Count > 0)
					debug += "[" + IndexExpressions.GetDebugInfo() + "]";
				// Añade el índice en formato RPN
				if (IndexExpressionsRPN != null && IndexExpressionsRPN.Count > 0)
					debug += " (RPN: " + IndexExpressionsRPN.GetDebugInfo() + ")";
				// Añade el miembro
				if (Member != null)
					debug += "->" + Member.GetDebugInfo();
				// Devuelve la información de depuración
				return debug;
		}

		/// <summary>
		///		Nombre de la variable
		/// </summary>
		internal string Name { get; set; }

		/// <summary>
		///		Expresiones de índice
		/// </summary>
		internal ExpressionsCollection IndexExpressions { get; set; }

		/// <summary>
		///		Expresiones del índice en formato RPN
		/// </summary>
		internal ExpressionsCollection IndexExpressionsRPN { get; set; }

		/// <summary>
		///		Identificador de variable
		/// </summary>
		internal ExpressionVariableIdentifier Member { get; set; }
	}
}
