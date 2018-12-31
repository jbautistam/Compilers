using System;

namespace Bau.Libraries.Compiler.LibTokenizer
{
	/// <summary>
	///		Manager para el proceso de obtener los tokens de un texto
	/// </summary>
	public class TokenizerManager
	{
		/// <summary>
		///		Interpreta un archivo de texto
		/// </summary>
		public Lexical.Tokens.TokenCollection ParseFile(string fileName)
		{
			return Parse(LibCommonHelper.Files.HelperFiles.LoadTextFile(fileName));
		}

		/// <summary>
		///		Interpreta un texto
		/// </summary>
		public Lexical.Tokens.TokenCollection Parse(string source)
		{
			return new Lexical.Parser.StringTokenSeparator(source, Rules).Parse();
		}

		/// <summary>
		///		Reglas para obtener palabras
		/// </summary>
		public Lexical.Rules.RuleCollection Rules { get; } = new Lexical.Rules.RuleCollection();
	}
}
