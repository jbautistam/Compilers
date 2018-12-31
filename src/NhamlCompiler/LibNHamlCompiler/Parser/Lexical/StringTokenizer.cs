using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.NhamlCompiler.Parser.Tokens;

namespace Bau.Libraries.NhamlCompiler.Parser.Lexical
{
	/// <summary>
	///		Conversor de una cadena en tokens
	/// </summary>
	internal class StringTokenizer
	{ 
		// Constantes privadas
		private string[] Sentences = { "code", "<%code%>", "else", "end", "for", "to", "step", "foreach", "if", "let", "else", "while" };

		internal StringTokenizer(string source)
		{
			Wordizer = new StringWord(source);
		}

		/// <summary>
		///		Obtiene todos los tokens de la cadena
		/// </summary>
		internal TokensCollection GetAllTokens()
		{
			TokensCollection tokens = new TokensCollection();

				// Obtiene todos los tokens
				do
				{
					tokens.Add(GetNextToken());
				}
				while (tokens[tokens.Count - 1].Type != Token.TokenType.EOF);
				// Devuelve la colección de tokens
				return tokens;
		}

		/// <summary>
		///		Obtiene el siguiente token
		/// </summary>
		internal Token GetNextToken()
		{
			Token token = new Token();
			StringWord.WordStruct word = Wordizer.GetNextWord();

				// Pasa los datos de la cadena al token
				token.Content = word.Content;
				if (!string.IsNullOrEmpty(token.Content))
					token.Content = token.Content.Trim();
				token.Row = word.Row;
				token.Column = word.Column;
				token.Indent = word.Indent;
				// Obtiene el tipo de token
				if (word.IsEof)
					token.Type = Token.TokenType.EOF;
				else
					token.Type = GetType(token.Content);
				// Si es un literal que comienza por un carácter \ lo quita del contenido
				if (token.Type == Token.TokenType.Literal && token.Content.StartsWithIgnoreNull("\\"))
					token.Content = token.Content.Mid(1, token.Content.Length);
				// Devuelve el token
				return token;
		}

		/// <summary>
		///		Obtiene el tipo de contenido
		/// </summary>
		private Token.TokenType GetType(string content)
		{
			if (content.StartsWith("\\"))
				return Token.TokenType.Literal;
			if (content.StartsWith("\""))
				return Token.TokenType.String;
			else if (content == "<!--")
				return Token.TokenType.StartComment;
			else if (content == "-->")
				return Token.TokenType.EndComment;
			else if (content == "[")
				return Token.TokenType.LeftCorchete;
			else if (content == "]")
				return Token.TokenType.RightCorchete;
			else if (content == "->")
				return Token.TokenType.VariablePointer;
			else if (Wordizer.IsCodeMode && content == "&&" || content == "||")
				return Token.TokenType.Relationaloperation;
			else if (content.StartsWith("$"))
				return Token.TokenType.Variable;
			else if (content.EqualsIgnoreCase("<%code%>"))
				return Token.TokenType.Sentence;
			else if (content.StartsWith("<%"))
				return Token.TokenType.StartSentenceBlock;
			else if (content == "%>")
				return Token.TokenType.EndSentenceBlock;
			else if (content == "{")
				return Token.TokenType.LeftLlave;
			else if (content == "}")
				return Token.TokenType.RightLlave;
			else if (Wordizer.IsCodeMode && content == "(")
				return Token.TokenType.LeftParentesis;
			else if (Wordizer.IsCodeMode && content == ")")
				return Token.TokenType.RightParentesis;
			else if (content == "#")
				return Token.TokenType.RightTagHTMLInner;
			else if (content == "=")
				return Token.TokenType.Equal;
			else if (Wordizer.IsCodeMode && (content == "+" || content == "-" || content == "*" ||
											 content == "/" || content == "\\"))
				return Token.TokenType.Arithmeticoperation;
			else if (Wordizer.IsCodeMode && (content == ">" || content == "<" || content == ">=" ||
											 content == "<=" || content == "=>" || content == "=<" ||
											 content == "==" || content == "!="))
				return Token.TokenType.Logicaloperation;
			else if (Wordizer.IsCodeMode && content != "" && IsNumeric(content))
				return Token.TokenType.Number;
			else if (content.StartsWith("#") && content.Length > 1)
				return Token.TokenType.LeftTagHTMLInner;
			else if (content.StartsWith("%") || content.StartsWith("&") || content.StartsWith("·"))
				return Token.TokenType.TagHTML;
			else if (Wordizer.IsCodeMode && IsSentence(content))
				return Token.TokenType.Sentence;
			else
				return Token.TokenType.Literal;
		}

		/// <summary>
		///		Comprueba si una palabra es una sentencia válida
		/// </summary>
		private bool IsSentence(string content)
		{ 
			// Comprueba si una palabra es una sentencia
			if (!string.IsNullOrWhiteSpace(content))
				foreach (string sentence in Sentences)
					if (sentence.Equals(content, StringComparison.CurrentCultureIgnoreCase))
						return true;
			// Si ha llegado hasta aquí es porque no es una sentencia
			return false;
		}

		/// <summary>
		///		Comprueba si es un número
		/// </summary>
		private bool IsNumeric(string content)
		{
			bool existDot = false;

				// Comprueba todos los caracteres o si la cadena es sólo un punto
				if (content == ".")
					return false;
				else
					foreach (char actual in content)
						if (!char.IsDigit(actual) && actual != '.')
							return false;
						else if (actual == '.')
						{
							if (existDot)
								return false;
							else
								existDot = true;
						}
				// Si ha llegado hasta aquí es porqe es numérico
				return !string.IsNullOrWhiteSpace(content);
		}

		/// <summary>
		///		Objeto para interpretación de palabras
		/// </summary>
		private StringWord Wordizer { get; }
	}
}
