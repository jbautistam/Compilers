using System;
using System.Collections.Generic;

using Bau.Libraries.NhamlCompiler.Parser.Instructions;

namespace Bau.Libraries.NhamlCompiler.Parser.Evaluator
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
			Stack<ExpressionBase> stackoperations = new Stack<ExpressionBase>();

				// Convierte las expresiones en una pila
				foreach (ExpressionBase expression in expressions)
					if (expression is ExpressionBase)
					{
						switch (expression.Token.Type)
						{
							case Tokens.Token.TokenType.LeftParentesis:
									// Paréntesis izquierdo, se mete directamente en la pila de operadores
									stackoperations.Push(expression);
								break;
							case Tokens.Token.TokenType.RightParentesis:
									bool end = false;

										// Paréntesis derecho. Saca todos los elementos del stack hasta encontrar un paréntesis izquierdo
										while (stackoperations.Count > 0 && !end)
										{
											ExpressionBase expressionoperation = stackoperations.Pop();

												if (expressionoperation.Token.Type == Tokens.Token.TokenType.LeftParentesis)
													end = true;
												else
													stackOutput.Add(expressionoperation);
										}
								break;
							case Tokens.Token.TokenType.Arithmeticoperation:
							case Tokens.Token.TokenType.Logicaloperation:
							case Tokens.Token.TokenType.Relationaloperation:
									bool endoperation = false;

										// Recorre los operadores de la pila 
										while (stackoperations.Count > 0 && !endoperation)
										{
											ExpressionBase objLastoperation = null;

											// Obtiene el último operador de la pila (sin sacarlo)
											objLastoperation = stackoperations.Peek();
											// Si no hay ningún operador en la pila o la prioridad del operador actual es mayor que la del último de la pila se mete el último operador
											if (objLastoperation == null || expression.Token.Type == Tokens.Token.TokenType.LeftParentesis ||
													expression.Priority > objLastoperation.Priority)
												endoperation = true;
											else // ... si el operador tiene una prioridad menor que el último de la fila, se quita el último operador de la pila y se compara de nuevo
												stackOutput.Add(stackoperations.Pop());
										}
									// Añade el operador a la pila de operadores
									stackoperations.Push(expression);
								break;
							case Tokens.Token.TokenType.Number:
							case Tokens.Token.TokenType.String:
							case Tokens.Token.TokenType.Variable:
									// Si es un número o una cadena o una variable se copia directamente en la pila de salida
									stackOutput.Add(expression);
								break;
							default:
								stackOutput.Add(new ExpressionBase(new Tokens.Token { Type = Tokens.Token.TokenType.Error, Content = "Expresión desconocida" }));
								break;
						}
					}
				// Añade todos los elementos que queden en el stack de operadores al stack de salida
				while (stackoperations.Count > 0)
					stackOutput.Add(stackoperations.Pop());
				// Devuelve la pila convertida a notación polaca inversa
				return stackOutput;
		}
	}
}
