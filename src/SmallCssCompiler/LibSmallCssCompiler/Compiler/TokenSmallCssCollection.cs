using System;
using System.Collections.Generic;

namespace Bau.Libraries.LibSmallCssCompiler.Compiler
{
	/// <summary>
	///		Colección de <see cref="TokenSmallCss"/>
	/// </summary>
	public class TokenSmallCssCollection : List<TokenSmallCss>
	{
		/// <summary>
		///		Añade un token a la colección
		/// </summary>
		public void Add(LibTokenizer.Lexical.Tokens.Token token)
		{
			base.Add(new TokenSmallCss(token));
		}
	}
}
