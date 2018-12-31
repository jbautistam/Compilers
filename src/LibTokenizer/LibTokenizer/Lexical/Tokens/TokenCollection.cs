using System;
using System.Collections.Generic;

namespace Bau.Libraries.LibTokenizer.Lexical.Tokens
{
	/// <summary>
	///		Colección de <see cref="Token"/>
	/// </summary>
	public class TokenCollection : List<Token>
	{
		/// <summary>
		///		Añade una palabra a la colección
		/// </summary>
		public void Add(Token.TokenType type, int subType, int row, int column, string value)
		{
			Add(new Token(type, subType, row, column, value));
		}

		/// <summary>
		///		Imprime las líneas de depuración
		/// </summary>
		public void Debug()
		{
			foreach (Token token in this)
				System.Diagnostics.Debug.WriteLine(token.GetDebugInfo());
		}
	}
}
