using System;

namespace Bau.Libraries.Compiler.LibTokenizer.Lexical.Rules
{
	/// <summary>
	///		Regla con los datos de una palabra con longitud fija (sin separadores)
	/// </summary>
	public class RuleWordFixed : RuleBase
	{
		public RuleWordFixed(Tokens.Token.TokenType type, int? subType, string[] words) : base(type, subType, false, false)
		{
			Words = words;
		}

		/// <summary>
		///		Palabras clave
		/// </summary>
		public string[] Words { get; set; }
	}
}
