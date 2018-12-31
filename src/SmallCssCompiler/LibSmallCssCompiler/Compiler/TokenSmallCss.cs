using System;

using Bau.Libraries.LibTokenizer.Lexical.Tokens;

namespace Bau.Libraries.LibSmallCssCompiler.Compiler
{
	/// <summary>
	///		Tokens específicos para el compilador de CSS
	/// </summary>
	public class TokenSmallCss : Token
	{
		// Enumerados públicos
		public enum TokenCssType
		{
			Unknown,
			Error,
			Variable,
			Comment,
			ReservedWord,
			EOF,
			Literal
		}

		public TokenSmallCss(Token token) : base(token.Type, token.SubType, token.Row, token.Column, token.Value)
		{
			TypeCss = Convert(token);
			Indent = token.Indent;
		}

		/// <summary>
		///		Convierte un tipo de token normal a un tipo de token CSS
		/// </summary>
		private TokenCssType Convert(Token token)
		{
			switch (token.Type)
			{
				case Token.TokenType.Error:
					return TokenCssType.Error;
				case Token.TokenType.Variable:
					return TokenCssType.Variable;
				case Token.TokenType.Comment:
					return TokenCssType.Comment;
				case Token.TokenType.ReservedWord:
					return TokenCssType.ReservedWord;
				case Token.TokenType.EOF:
					return TokenCssType.EOF;
				default:
					return TokenCssType.Literal;
			}
		}

		/// <summary>
		///		Obtiene la información de depuración
		/// </summary>
		public override string GetDebugInfo()
		{
			return $"{TypeCss} - {base.GetDebugInfo()}";
		}

		/// <summary>
		///		Obtiene la depuración para mostrarlo con las instrucciones
		/// </summary>
		public string GetDebugForInstructions()
		{
			if (TypeCss == TokenCssType.Error)
				return $"ERROR ({Value})";
			else
				return Value;
		}

		/// <summary>
		///		Tipo del token para Css
		/// </summary>
		public TokenCssType TypeCss { get; internal set; }
	}
}
