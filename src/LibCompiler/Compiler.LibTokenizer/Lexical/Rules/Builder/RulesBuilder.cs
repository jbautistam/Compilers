using System;

namespace Bau.Libraries.Compiler.LibTokenizer.Lexical.Rules.Builder
{
	/// <summary>
	///		Generador de reglas
	/// </summary>
	public class RulesBuilder
	{
		/// <summary>
		///		Añade una regla delimitada
		/// </summary>
		public RulesBuilder WithRuleDelimited(Tokens.Token.TokenType type, int? subType, string start, string end,
											  bool toEndLine, bool toFirstSpace, bool includeStart, bool includeEnd)
		{
			// Añade la regla
			Rules.Add(new RuleDelimited(type, subType, new string[] { start }, new string[] { end }, 
										toEndLine, toFirstSpace, includeStart, includeEnd));
			// Devuelve el generador
			return this;
		}

		/// <summary>
		///		Añade una regla delimitada
		/// </summary>
		public RulesBuilder WithRuleDelimited(Tokens.Token.TokenType type, int? subType, string[] starts, string[] ends,
											  bool toEndLine, bool toFirstSpace, bool includeStart, bool includeEnd)
		{
			// Añade la regla
			Rules.Add(new RuleDelimited(type, subType, starts, ends, toEndLine, toFirstSpace, includeStart, includeEnd));
			// Devuelve el generador
			return this;
		}

		/// <summary>
		///		Añade una regla de un patrón
		/// </summary>
		public RulesBuilder WithRulePattern(Tokens.Token.TokenType type, int? subType, string patternStart, string patternContent)
		{
			// Añade la regla
			Rules.Add(new RulePattern(type, subType, patternStart, patternContent));
			// Devuelve el generador
			return this;
		}

		/// <summary>
		///		Añade una regla de una palabra
		/// </summary>
		public RulesBuilder WithRuleWord(Tokens.Token.TokenType type, int? subType, string[] words, string[] separators, bool toFirstSpace)
		{
			// Añade la regla
			Rules.Add(new RuleWord(type, subType, words, separators, toFirstSpace));
			// Devuelve el generador
			return this;
		}

		/// <summary>
		///		Añade una regla de palabra reservada
		/// </summary>
		public RulesBuilder WithRuleWordFixed(Tokens.Token.TokenType type, int? subType, string word)
		{
			return WithRuleWordFixed(type, subType, new string[] { word });
		}

		/// <summary>
		///		Añade una regla de palabra reservada
		/// </summary>
		public RulesBuilder WithRuleWordFixed(Tokens.Token.TokenType type, int? subType, string[] words)
		{
			// Añade la regla
			Rules.Add(new RuleWordFixed(type, subType, words));
			// Devuelve el generador
			return this;
		}

		/// <summary>
		///		Genera las reglas
		/// </summary>
		public RuleCollection Build()
		{
			return Rules;
		}

		/// <summary>
		///		Colección de reglas
		/// </summary>
		private RuleCollection Rules { get; } = new RuleCollection();
	}
}
