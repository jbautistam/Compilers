using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.Compiler.LibTokenizer.Lexical.Rules;
using Bau.Libraries.Compiler.LibTokenizer.Lexical.Tokens;

namespace Bau.Libraries.Compiler.LibTokenizer.Lexical.Parser
{
	/// <summary>
	///		Clase para la separación de tokens
	/// </summary>
	internal class StringTokenSeparator
	{
		internal StringTokenSeparator(string source, RuleCollection rules)
		{
			CharSeparator = new StringCharSeparator(source);
			Rules = rules;
		}

		/// <summary>
		///		Interpreta las palabras
		/// </summary>
		internal TokenCollection Parse()
		{
			TokenCollection tokens = new TokenCollection();

				// Interpreta la cadena
				while (!CharSeparator.IsEof)
				{
					bool found = false;
					int firstCharacter;

						// Salta los espacios
						CharSeparator.SkipSpaces();
						// Guarda el índice del primer carácter
						firstCharacter = CharSeparator.IndexActualChar;
						// Si no ha terminado ya
						if (!CharSeparator.IsEof)
						{
							// Obtiene la regla que se corresponde con la siguiente cadena
							foreach (RuleBase rule in Rules)
								if (!found)
									if (rule is RuleDelimited)
										CheckRuleDelimited(rule as RuleDelimited, tokens, ref found);
									else if (rule is RulePattern)
										CheckRulePattern(rule as RulePattern, tokens, ref found);
									else if (rule is RuleWord)
										CheckRuleWord(rule as RuleWord, tokens, ref found);
									else if (rule is RuleWordFixed)
										CheckRuleWordFixed(rule as RuleWordFixed, tokens, ref found);
									else
										throw new NotImplementedException("Tipo de regla desconocida");
							// Si no se ha encontrado nada, obtiene una palabra hasta el siguiente espacio
							if (!found)
								tokens.Add(ReadWordToSpaces());
							// Obtiene la indentación del primer carácter
							tokens[tokens.Count - 1].Indent = CharSeparator.GetIndentFrom(firstCharacter);
						}
				}
				// Devuelve la colección de palabras
				return tokens;
		}

		/// <summary>
		///		Obtiene un token a partir de una palabra clave de longitud fija
		/// </summary>
		private void CheckRuleWordFixed(RuleWordFixed rule, TokenCollection tokens, ref bool found)
		{
			foreach (string word in rule.Words)
				if (!found && CharSeparator.LookAtChar(word.Length).EqualsIgnoreCase(word))
				{ 
					// Añade el token
					tokens.Add(GetToken(rule, word, true));
					// Indica que se ha encontrado
					found = true;
				}
		}

		/// <summary>
		///		Comprueba una regla delimitada
		/// </summary>
		private void CheckRuleDelimited(RuleDelimited rule, TokenCollection tokens, ref bool found)
		{
			string startRule = GetStartRule(rule.Starts);

				if (!startRule.IsEmpty())
				{ 
					// Añade la palabra
					tokens.Add(ReadWord(startRule, rule));
					// Indica que se ha encontrado
					found = true;
				}
		}

		/// <summary>
		///		Comprueba una regla de palabra reservada
		/// </summary>
		private void CheckRuleWord(RuleWord rule, TokenCollection tokens, ref bool found)
		{
			foreach (string word in rule.Words)
				if (!found && CharSeparator.LookAtChar(word.Length).EqualsIgnoreCase(word))
				{
					if (rule.ToFirstSpace && CharSeparator.CheckIsSpace(CharSeparator.LookAtChars(word.Length, 1)))
					{ 
						// Añade el token
						tokens.Add(GetToken(rule, word, true));
						// Indica que se ha encontrado
						found = true;
					}
				}
		}

		/// <summary>
		///		Crea un token a partir de una regla y una palabra
		/// </summary>
		private Token GetToken(RuleBase rule, string value, bool extractChars)
		{
			Token token = new Token(rule.Type, rule.SubType, CharSeparator.Row, CharSeparator.Column, value);

				// Quita los caracteres del token
				if (extractChars)
					CharSeparator.GetChars(value.Length);
				// Elimina los espacios
				if (rule.MustTrim)
					token.Value = token.Value.TrimIgnoreNull();
				// Devuelve el token
				return token;
		}

		/// <summary>
		///		Comprueba una regla a partir de un patrón de caracteres
		/// </summary>
		private void CheckRulePattern(RulePattern rule, TokenCollection tokens, ref bool found)
		{
			if (CharSeparator.CheckPatternStart(rule.PatternStart))
			{ 
				// Añade el token de un patrón
				tokens.Add(GetToken(rule, CharSeparator.GetCharsPattern(rule.PatternStart, rule.PatternContent), false));
				// Indica que se ha grabado
				found = true;
			}
		}

		/// <summary>
		///		Obtiene la cadena de inicio que coincide con la regla
		/// </summary>
		private string GetStartRule(string[] starts)
		{ 
			// Busca la palabra de inicio que coincide con la regla
			if (starts?.Length > 0)
				foreach (string start in starts)
					if (CharSeparator.LookAtChar(start.Length).EqualsIgnoreCase(start))
						return start;
			// Si ha llegado hasta aquí es porque no se corresponde con la regla
			return "";
		}

		/// <summary>
		///		Lee una palabra hasta que se encuentran los separadores
		/// </summary>
		private Token ReadWord(string startRule, RuleDelimited rule)
		{
			Token token = new Token(rule.Type, rule.SubType, CharSeparator.Row, CharSeparator.Column, "");

				// Obtiene la cadena de inicio
				if (rule.IncludeStart)
					token.Value = CharSeparator.GetChars(startRule.Length);
				else // ... se salta el inicio
					CharSeparator.GetChars(startRule.Length);
				// Busca hasta el final
				if (rule.ToEndLine)
					token.Value += CharSeparator.GetCharsToEndLine();
				else if (rule.ToFirstSpace || rule.Ends == null || rule.Ends.Length == 0)
					token.Value += CharSeparator.GetCharsToSpace();
				else
				{
					bool end = false;

						while (!CharSeparator.IsEof && !end)
						{
							string actualRule = GetStartRule(rule.Ends);

								// Comprueba si se ha terminado con la palabra ...
								if (!actualRule.IsEmpty()) // ... si coincide con una de las reglas de fin
								{
									if (rule.IncludeEnd)
										token.Value += CharSeparator.GetChars(actualRule.Length);
									else
										CharSeparator.GetChars(actualRule.Length);
									end = true;
								}
								else if (rule.ToFirstSpace && CharSeparator.IsSpace)
									end = true;
								else
									token.Value += CharSeparator.GetNextChar();
						}
				}
				// Limpia la palabra
				if (rule.MustTrim && !token.Value.IsEmpty())
					token.Value = token.Value.Trim();
				// Devuelve la palabra
				return token;
		}

		/// <summary>
		///		Lee un token hasta encontrar un espacio
		/// </summary>
		private Token ReadWordToSpaces()
		{
			return new Token(Token.TokenType.Unknown, null, CharSeparator.Row, CharSeparator.Column, CharSeparator.GetCharsToSpace());
		}

		/// <summary>
		///		Clase para obtención de caracteres
		/// </summary>
		internal StringCharSeparator CharSeparator { get; }

		/// <summary>
		///		Reglas
		/// </summary>
		internal RuleCollection Rules { get; }
	}
}
