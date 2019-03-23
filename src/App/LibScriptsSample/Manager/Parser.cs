using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.Compiler.LibTokenizer.Lexical.Rules;
using Bau.Libraries.Compiler.LibTokenizer.Lexical.Tokens;
using Bau.Libraries.Compiler.LibInterpreter.Expressions;

namespace Bau.Libraries.LibScriptsSample.Manager
{
	/// <summary>
	///		Intérprete
	/// </summary>
	internal class Parser : Compiler.LibInterpreter.Processor.Compiler.ParserBase
	{
		/// <summary>
		///		Subtipo del token
		/// </summary>
		private enum TokenSubType
		{
			/// <summary>Fecha</summary>
			Date,
			/// <summary>Nombre de variable (entre {{ y }})</summary>
			Variable
		}

		/// <summary>
		///		Inicializa el lector de tokens
		/// </summary>
		public override void Initialize()
		{
			Compiler.LibTokenizer.Lexical.Rules.Builder.RulesBuilder builder = new Compiler.LibTokenizer.Lexical.Rules.Builder.RulesBuilder();

				// Inicializa el tokenizador con las órdenes básicas
				InitializaGeneralTokenizer(builder);
				// Añade las reglas de este tipo de procesador
				builder.WithRuleDelimited(Token.TokenType.UserDefined, (int) TokenSubType.Date, "#", "#",
										  false, false, false, false); // ... fechas
				builder.WithRuleDelimited(Token.TokenType.UserDefined, (int) TokenSubType.Variable, "{{", "}}",
										  false, false, false, false); // ... contenido de variable
				// Genera las reglas
				Tokenizer.Rules.AddRange(builder.Build());
		}

		/// <summary>
		///		Interpreta un token definido por el usuario
		/// </summary>
		protected override ExpressionBase ParseTokenUserDefined(Token token)
		{
			switch ((TokenSubType) (token.SubType ?? 2000))
			{
				case TokenSubType.Date:
					return new ExpressionConstant(Compiler.LibInterpreter.Context.Variables.VariableModel.VariableType.Date, token.Value.GetDateTime());
				case TokenSubType.Variable:
					return new ExpressionVariableIdentifier(token.Value);
				default:
					return new ExpressionError("Unknown token type");
			}
		}
	}
}
