using System;

using Bau.Libraries.LibTokenizer;
using Bau.Libraries.LibTokenizer.Lexical.Rules;
using Bau.Libraries.LibTokenizer.Lexical.Tokens;

namespace Bau.Libraries.LibSmallCssCompiler.Compiler
{
	/// <summary>
	///		Intérprete de un archivo CSS
	/// </summary>
	internal class Tokenizer
	{
		/// <summary>
		///		Interpreta una cadena
		/// </summary>
		internal TokenSmallCssCollection Parse(string content)
		{
			TokenizerManager tokenizer = new TokenizerManager();

			// Añade las reglas de lectura
			tokenizer.Rules.Add(new RuleDelimited(Token.TokenType.Comment, null,
												  new string[] { "/*" }, new string[] { "*/" },
												  false, false, false, false)); // ... comentarios en bloque
			tokenizer.Rules.Add(new RuleDelimited(Token.TokenType.Comment, null,
												  new string[] { "//", "_" }, null,
												  true, false, false, false)); // ... comentarios
			tokenizer.Rules.Add(new RuleWord(Token.TokenType.ReservedWord, null,
											 new string[] 
												{ 
													CompilerConstants.MixinDefinition,
													CompilerConstants.MixinInclude,
													CompilerConstants.IfDefined,
													CompilerConstants.Media,
													CompilerConstants.Import 
												},
											 null, true)); // ... palabras reservadas
			tokenizer.Rules.Add(new RulePattern(Token.TokenType.Variable, null,
												CompilerConstants.Variable + "A", "A9_")); // ... definición de variables
			tokenizer.Rules.Add(new RuleDelimited(Token.TokenType.String, null,
												  new string[] { "\"" }, new string[] { "\"" },
												  false, false, false, false)); // ... cadenas
			// Obtiene los tokens
			return Convert(tokenizer.Parse(content));
		}

		/// <summary>
		///		Convierte una colección de tokens estándar en los específicos para este compilador
		/// </summary>
		private TokenSmallCssCollection Convert(TokenCollection sourceTokens)
		{
			TokenSmallCssCollection target = new TokenSmallCssCollection();

			// Convierte los tokens estándar
			foreach (Token source in sourceTokens)
				target.Add(source);
			// Devuelve la colección de tokens convertidos
			return target;
		}
	}
}
