using System;

namespace Bau.Libraries.Compiler.LibTokenizer.Lexical.Rules
{
	/// <summary>
	///		Regla definida por un patrón
	/// </summary>
	public class RulePattern : RuleBase
	{
		public RulePattern(Tokens.Token.TokenType type, int? subType, string patternStart,
						   string patternContent) : base(type, subType, false, true)
		{
			PatternStart = patternStart;
			PatternContent = patternContent;
		}

		/// <summary>
		///		Patrón de inicio
		/// </summary>
		public string PatternStart { get; set; }

		/// <summary>
		///		Patrón de contenido
		/// </summary>
		public string PatternContent { get; set; }
	}
}
