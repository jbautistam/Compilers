using System;

namespace Bau.Libraries.LibTokenizer.Lexical.Rules
{
	/// <summary>
	///		Regla de definición para una palabra
	/// </summary>
	public class RuleWord : RuleBase
	{
		public RuleWord(Tokens.Token.TokenType type, int? subType, string[] words, string[] separators,
						bool toFirstSpace) : base(type, subType, toFirstSpace, true)
		{
			Words = words;
			Separators = separators;
		}

		/// <summary>
		///		Palabras que definen la regla
		/// </summary>
		public string[] Words { get; set; }

		/// <summary>
		///		Separadores
		/// </summary>
		public string[] Separators { get; set; }
	}
}
