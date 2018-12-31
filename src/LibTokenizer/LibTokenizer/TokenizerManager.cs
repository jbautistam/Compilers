using System;

namespace Bau.Libraries.LibTokenizer
{
	/// <summary>
	///		Manager para el proceso de obtener los tokens de un texto
	/// </summary>
	public class TokenizerManager
	{
		public TokenizerManager()
		{
			Rules = new Lexical.Rules.RuleCollection();
		}

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
		///		Carga las reglas de un archivo
		/// </summary>
		public void LoadRules(string fileName)
		{
			Rules = new Lexical.Rules.Repository.RuleRepository().Load(fileName);
		}

		/// <summary>
		///		Graba las reglas en un archivo
		/// </summary>
		public void SaveRules(string fileName)
		{
			new Lexical.Rules.Repository.RuleRepository().Save(Rules, fileName);
		}

		/// <summary>
		///		Reglas para obtener palabras
		/// </summary>
		public Lexical.Rules.RuleCollection Rules { get; private set; }
	}
}
