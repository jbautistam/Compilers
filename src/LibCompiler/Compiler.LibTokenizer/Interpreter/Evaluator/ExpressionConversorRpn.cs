using System;
using System.Collections.Generic;

using Bau.Libraries.Compiler.LibTokenizer.Interpreter.Instructions;
using Bau.Libraries.Compiler.LibTokenizer.Lexical.Tokens;

namespace Bau.Libraries.Compiler.LibTokenizer.Interpreter.Evaluator
{
	/// <summary>
	///		Conversor de expresiones a notación polaca inversa
	/// </summary>
	internal class ExpressionConversorRpn
	{
		/// <summary>
		///		Convierte una colección de expresiones en una pila de expresiones en notación polaca inversa (sin paréntesis)
		/// </summary>
		internal ExpressionsCollection ConvertToRPN(ExpressionsCollection expressions)
		{
			ExpressionsCollection stackOutput = new ExpressionsCollection();
			Stack<ExpressionBase> stackOperators = new Stack<ExpressionBase>();

				// Convierte las expresiones en una pila
				foreach (ExpressionBase expression in expressions)
					if (expression is ExpressionBase)
					{
						switch (expression.Token.Type)
						{
							case Token.TokenType.LeftParentesis:
									// Paréntesis izquierdo, se mete directamente en la pila de operadores
									stackOperators.Push(expression);
								break;
							case Token.TokenType.RightParentesis:
									bool end = false;

									// Paréntesis derecho. Saca todos los elementos del stack hasta encontrar un paréntesis izquierdo
									while (stackOperators.Count > 0 && !end)
									{
										ExpressionBase expressionOperator = stackOperators.Pop();

											if (expressionOperator.Token.Type == Token.TokenType.LeftParentesis)
												end = true;
											else
												stackOutput.Add(expressionOperator);
									}
								break;
							case Token.TokenType.ArithmeticOperator:
							case Token.TokenType.LogicalOperator:
							case Token.TokenType.RelationalOperator:
								bool endOperator = false;

									// Recorre los operadores de la pila 
									while (stackOperators.Count > 0 && !endOperator)
									{
										ExpressionBase lastOperator = stackOperators.Peek();

											// Si no hay ningún operador en la pila o la prioridad del operador actual es mayor que la del último de la pila se mete el último operador
											if (lastOperator == null || expression.Token.Type == Token.TokenType.LeftParentesis ||
													expression.Priority > lastOperator.Priority)
												endOperator = true;
											else // ... si el operador tiene una prioridad menor que el último de la fila, se quita el último operador de la pila y se compara de nuevo
												stackOutput.Add(stackOperators.Pop());
									}
									// Añade el operador a la pila de operadores
									stackOperators.Push(expression);
								break;
							case Token.TokenType.Number:
							case Token.TokenType.String:
							case Token.TokenType.Variable:
									// Si es un número o una cadena o una variable se copia directamente en la pila de salida
									stackOutput.Add(expression);
								break;
							default:
									stackOutput.Add(new ExpressionBase(new Token(Token.TokenType.Error, null, -1, -1, "Expresión desconocida")));
								break;
						}
					}
				// Añade todos los elementos que queden en el stack de operadores al stack de salida
				while (stackOperators.Count > 0)
					stackOutput.Add(stackOperators.Pop());
				// Devuelve la pila convertida a notación polaca inversa
				return stackOutput;
		}
	}
}
