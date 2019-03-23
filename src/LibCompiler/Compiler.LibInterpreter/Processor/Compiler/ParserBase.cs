using System;
using System.Collections.Generic;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.Compiler.LibTokenizer;
using Bau.Libraries.Compiler.LibTokenizer.Lexical.Tokens;
using Bau.Libraries.Compiler.LibInterpreter.Expressions;

namespace Bau.Libraries.Compiler.LibInterpreter.Processor.Compiler
{
	/// <summary>
	///		Clase con el analizador léxico del programa
	/// </summary>
	public abstract class ParserBase
	{
		/// <summary>
		///		Inicializa el analizaor léxico
		/// </summary>
		public abstract void Initialize();

		/// <summary>
		///		Inicializa reglas genéricas para los diferentes lenguajes
		/// </summary>
		protected void InitializaGeneralTokenizer(LibTokenizer.Lexical.Rules.Builder.RulesBuilder builder)
		{
			// Rellena los operadores si es necesario
			FillOperators();
			// Añade las reglas de lectura
			builder.WithRulePattern(Token.TokenType.Variable, null, "A", "A9_"); // ... definición de variables
			builder.WithRuleDelimited(Token.TokenType.String, null, "\"", "\"", false, false, false, false); // ... cadenas
			builder.WithRulePattern(Token.TokenType.Number, null, "9", "9."); // ... definición de números
			builder.WithRuleWordFixed(Token.TokenType.ArithmeticOperator, null, GetOperatorKeys(MathOperators));
			builder.WithRuleWordFixed(Token.TokenType.LogicalOperator, null, GetOperatorKeys(LogicalOperators));
			builder.WithRuleWordFixed(Token.TokenType.RelationalOperator, null, GetOperatorKeys(RelationalOperators));
			builder.WithRuleWordFixed(Token.TokenType.LeftParentesis, null, LeftParenthesis);
			builder.WithRuleWordFixed(Token.TokenType.RightParentesis, null, RightParenthesis);
			// tokenizer.Rules.Add(new RuleWordFixed(Token.TokenType.EndInstruction, null, new string[] { ";" }));
			// tokenizer.Rules.Add(new RuleWordFixed(Token.TokenType.StartBlock, null, new string[] { "{" }));
			// tokenizer.Rules.Add(new RuleWordFixed(Token.TokenType.EndBlock, null, new string[] { "}" }));
		}

		/// <summary>
		///		Rellena los operadores
		/// </summary>
		private void FillOperators()
		{
			// Asigna los paréntesis
			if (string.IsNullOrWhiteSpace(LeftParenthesis))
				LeftParenthesis = "(";
			if (string.IsNullOrWhiteSpace(RightParenthesis))
				RightParenthesis = ")";
			// Añada los operadores matemáticos
			if (MathOperators.Count == 0)
			{
				MathOperators.Add("+", ExpressionOperatorMath.MathType.Sum);
				MathOperators.Add("-", ExpressionOperatorMath.MathType.Substract);
				MathOperators.Add("*", ExpressionOperatorMath.MathType.Multiply);
				MathOperators.Add("/", ExpressionOperatorMath.MathType.Divide);
				MathOperators.Add("%", ExpressionOperatorMath.MathType.Modulus);
			}
			// Añada los operadores lógicos
			if (LogicalOperators.Count == 0)
			{
				LogicalOperators.Add("<", ExpressionOperatorLogical.LogicalType.Less);
				LogicalOperators.Add(">", ExpressionOperatorLogical.LogicalType.Greater);
				LogicalOperators.Add(">=", ExpressionOperatorLogical.LogicalType.GreaterOrEqual);
				LogicalOperators.Add("<=", ExpressionOperatorLogical.LogicalType.LessOrEqual);
				LogicalOperators.Add("==", ExpressionOperatorLogical.LogicalType.Equal);
				LogicalOperators.Add("!=", ExpressionOperatorLogical.LogicalType.Distinct);
			}
			// Añade los operadores relacionales
			if (RelationalOperators.Count == 0)
			{
				RelationalOperators.Add("<", ExpressionOperatorRelational.RelationalType.Or);
				RelationalOperators.Add(">", ExpressionOperatorRelational.RelationalType.And);
				RelationalOperators.Add(">=", ExpressionOperatorRelational.RelationalType.Not);
			}
		}

		/// <summary>
		///		Obtiene las claves de los operadores
		/// </summary>
		private string [] GetOperatorKeys<TypeData>(Dictionary<string, TypeData> operations)
		{
			string [] keys = new string[operations.Count];
			int index = 0;

				// Añade las claves
				foreach (KeyValuePair<string, TypeData> operation in operations)
					keys[index++] = operation.Key;
				// Devuelve las claves
				return keys;
		}

		/// <summary>
		///		Obtiene las expresiones de un código
		/// </summary>
		public ExpressionsCollection Parse(string code, out string error)
		{
			// Obtiene los tokens del código
			Tokens = Tokenizer.Parse(code);
			// Inicializa el índice de tokens
			ActualTokenIndex = 0;
			// Añade un token de fin de instrucción
			Tokens.Add(new Token(Token.TokenType.EndInstruction, null, 0, 0, ";"));
			// Interpreta las expresiones
			return ParseExpression(new Token.TokenType[] { Token.TokenType.EndInstruction }, out error);
		}

		/// <summary>
		///		Interpreta una expresión hasta encontrar un token de cierre
		/// </summary>
		private ExpressionsCollection ParseExpression(Token.TokenType[] tokensTypeEnd, out string error)
		{
			ExpressionsCollection expressions = new ExpressionsCollection();
			int parenthesisOpen = 0;
			bool isEnd = false;

				// Inicializa los argumentos de salida
				error = string.Empty;
				// Interpreta las expresiones
				while (!isEnd && string.IsNullOrEmpty(error))
				{ 
					// Añade el token actual a la colección de expresiones
					switch (ActualToken.Type)
					{
						case Token.TokenType.ArithmeticOperator:
								expressions.Add(GetExpressionMath(ActualToken.Value));
							break;
						case Token.TokenType.LeftParentesis:
						case Token.TokenType.RightParentesis:
								// Añade la expresión del paréntesis
								expressions.Add(new ExpressionParenthesis(ActualToken.Type == Token.TokenType.LeftParentesis));
								// Añade o quita el paréntesis del contador
								if (ActualToken.Type == Token.TokenType.LeftParentesis)
									parenthesisOpen++;
								else if (ActualToken.Type == Token.TokenType.RightParentesis)
									parenthesisOpen--;
							break;
						case Token.TokenType.LogicalOperator:
								expressions.Add(GetExpressionLogical(ActualToken.Value));
							break;
						case Token.TokenType.RelationalOperator:
								expressions.Add(GetExpressionRelation(ActualToken.Value));
							break;
						case Token.TokenType.Number:
								expressions.Add(new ExpressionConstant(Context.Variables.VariableModel.VariableType.Numeric, 
																	   ActualToken.Value.GetDouble(0)));
							break;
						case Token.TokenType.String:
								expressions.Add(new ExpressionConstant(Context.Variables.VariableModel.VariableType.String,
																	   ActualToken.Value));
							break;
						case Token.TokenType.UserDefined:
								expressions.Add(ParseTokenUserDefined(ActualToken));
							break;
						case Token.TokenType.Variable:
								expressions.Add(new ExpressionVariableIdentifier(ActualToken.Value));
							break;
						default:
								expressions.Add(new ExpressionError("Unknown expression type"));
							break;
					}
					// Si la última expresión es un error, se detiene
					if (expressions.Count > 0 && expressions[0] is ExpressionError expression)
						error = expression.Message;
					else
					{
						// Pasa al siguiente token
						NextToken();
						// Comprueba si es el final de la expresión
						isEnd = IsEndExpression(tokensTypeEnd, parenthesisOpen);
					}
				}
				// Devuelve la colección de expresiones
				return expressions;
		}

		/// <summary>
		///		Interpreta un token definido por el usuario
		/// </summary>
		protected abstract ExpressionBase ParseTokenUserDefined(Token token);

		/// <summary>
		///		Obtiene una expresión matemática
		/// </summary>
		private ExpressionBase GetExpressionMath(string operation)
		{
			if (MathOperators.ContainsKey(operation))
				return new ExpressionOperatorMath(MathOperators[operation]);
			else
				return new ExpressionError($"Unknown operator {operation}");
		}

		/// <summary>
		///		Obtiene una expresión lógica
		/// </summary>
		private ExpressionBase GetExpressionLogical(string operation)
		{
			if (LogicalOperators.ContainsKey(operation))
				return new ExpressionOperatorLogical(LogicalOperators[operation]);
			else
				return new ExpressionError($"Unknown operator {operation}");
		}

		/// <summary>
		///		Obtiene una expresión de relación
		/// </summary>
		private ExpressionBase GetExpressionRelation(string operation)
		{
			if (RelationalOperators.ContainsKey(operation))
				return new ExpressionOperatorRelational(RelationalOperators[operation]);
			else
				return new ExpressionError($"Unknown operator {operation}");
		}

		/// <summary>
		///		Interpreta el fin de una expresión
		/// </summary>
		private bool IsEndExpression(Token.TokenType[] tokensTypeEnd, int parenthesisOpen)
		{
			bool isEnd = false;

				// Comprueba si es el final de la expresión
				if (IsEndProgram)
					isEnd = true;
				else
					foreach (Token.TokenType type in tokensTypeEnd)
						if (type == Token.TokenType.RightParentesis)
						{
							if (ActualToken.Type == Token.TokenType.RightParentesis && parenthesisOpen == 0)
								isEnd = true;
						}
						else if (type == ActualToken.Type)
							isEnd = true;
				// Si ha llegado hasta aquí es porque no es el final de la expresión
				return isEnd;
		}

		/// <summary>
		///		Pasa al siguiente token
		/// </summary>
		private void NextToken()
		{
			ActualTokenIndex++;
		}

		/// <summary>
		///		Lector de tokens
		/// </summary>
		protected TokenizerManager Tokenizer { get; } = new TokenizerManager();

		/// <summary>
		///		Operadores matemáticos
		/// </summary>
		protected Dictionary<string, ExpressionOperatorMath.MathType> MathOperators { get; } = new Dictionary<string, ExpressionOperatorMath.MathType>();

		/// <summary>
		///		Operadores lógicos
		/// </summary>
		protected Dictionary<string, ExpressionOperatorLogical.LogicalType> LogicalOperators { get; } = new Dictionary<string, ExpressionOperatorLogical.LogicalType>();

		/// <summary>
		///		Operadores relacionales
		/// </summary>
		protected Dictionary<string, ExpressionOperatorRelational.RelationalType> RelationalOperators { get; } = new Dictionary<string, ExpressionOperatorRelational.RelationalType>();

		/// <summary>
		///		Paréntesis izquierdo
		/// </summary>
		protected string LeftParenthesis { get; set; }

		/// <summary>
		///		Paréntesis derecho
		/// </summary>
		protected string RightParenthesis { get; set; }

		/// <summary>
		///		Tokens a analizar
		/// </summary>
		private TokenCollection Tokens { get; set; }

		/// <summary>
		///		Token que se está analizando
		/// </summary>
		private int ActualTokenIndex { get; set; } = 0;

		/// <summary>
		///		Token actual
		/// </summary>
		private Token ActualToken
		{
			get
			{
				if (ActualTokenIndex < Tokens.Count)
					return Tokens[ActualTokenIndex];
				else
					return new Token(Token.TokenType.EOF, null, 0, 0, "Program end");
			}
		}

		/// <summary>
		///		Indica si es el final del programa
		/// </summary>
		private bool IsEndProgram
		{
			get { return ActualTokenIndex >= Tokens.Count - 1; }
		}
	}
}