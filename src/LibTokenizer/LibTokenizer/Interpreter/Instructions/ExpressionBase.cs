using System;

using Bau.Libraries.LibTokenizer.Lexical.Tokens;

namespace Bau.Libraries.LibTokenizer.Interpreter.Instructions
{
	/// <summary>
	///		Clase con los datos de una expresión
	/// </summary>
	public class ExpressionBase
	{
		public ExpressionBase(Token token)
		{
			Token = token;
		}

		/// <summary>
		///		Información de depuración
		/// </summary>
		public virtual string GetDebugInfo()
		{
			return Token.Value;
		}

		/// <summary>
		///		Token asociado a la expresión
		/// </summary>
		public Token Token { get; }

		/// <summary>
		///		Prioridad de la expresión
		/// </summary>
		public int Priority
		{
			get
			{
				if (Token.Value == "*" || Token.Value == "/" || Token.Value == "%")
					return 20;
				else if (Token.Value == "+" || Token.Value == "-")
					return 19;
				else if (Token.Value == "<" || Token.Value == ">" || Token.Value == ">=" || Token.Value == "<=")
					return 18;
				else if (Token.Value == "==" || Token.Value == "!=")
					return 17;
				else if (Token.Value == "&&")
					return 16;
				else if (Token.Value == "||")
					return 15;
				else if (Token.Value == "=")
					return 14;
				else
					return 0;
			}
		}

		/// <summary>
		///		Clona una expresión
		/// </summary>
		public virtual ExpressionBase Clone()
		{
			return new ExpressionBase(Token);
		}
	}
}
