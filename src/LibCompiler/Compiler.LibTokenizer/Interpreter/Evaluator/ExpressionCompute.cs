using System;
using System.Collections.Generic;

using Bau.Libraries.Compiler.LibTokenizer.Interpreter.Instructions;
using Bau.Libraries.Compiler.LibTokenizer.Lexical.Tokens;
using Bau.Libraries.Compiler.LibTokenizer.Variables;

namespace Bau.Libraries.Compiler.LibTokenizer.Interpreter.Evaluator
{
	/// <summary>
	///		Clase para el cálculo de expresiones
	/// </summary>
	public class ExpressionCompute
	{
		/// <summary>
		///		Calcula una expresión
		/// </summary>
		public ExpressionCompute(VariablesCollection variables)
		{
			if (variables == null)
				Variables = new VariablesCollection();
			else
				Variables = variables.Clone();
		}

		/// <summary>
		///		Evalúa una serie de expresiones
		/// </summary>
		public ValueBase Evaluate(ExpressionsCollection expressions)
		{
			return Compute(new ExpressionConversorRpn().ConvertToRPN(expressions));
		}

		/// <summary>
		///		Evalúa una serie de expresiones pasadas a notación polaca
		/// </summary>
		public ValueBase EvaluateRpn(ExpressionsCollection expressionsRpn)
		{
			return Compute(expressionsRpn.Clone());
		}

		/// <summary>
		///		Busca una variable en la colección
		/// </summary>
		public Variable Search(ExpressionVariableIdentifier expressionVariable, out string error)
		{
			return Search(expressionVariable, Variables, out error);
		}

		/// <summary>
		///		Busca recursivamente una variable y su miembro
		/// </summary>
		private Variable Search(ExpressionVariableIdentifier expressionVariable, VariablesCollection variables, out string error)
		{
			Variable variable = null;
			int index = 0;

				// Inicializa las variables de salida
				error = null;
				// Obtiene el índice asociado a la variable
				if (expressionVariable.IndexExpressionsRPN?.Count > 0)
				{
					ValueBase indexVariable = Compute(expressionVariable.IndexExpressionsRPN);

						if (indexVariable.HasError)
							error = indexVariable.Error;
						else if (!(indexVariable is ValueNumeric))
							error = "La expresión del índice no tiene un valor numérico";
						else
							index = (int) (indexVariable as ValueNumeric).Value;
				}
				// Si no hay ningún error, obtiene la variable
				if (string.IsNullOrWhiteSpace(error))
				{ 
					// Obtiene la variable
					variable = variables.Search(expressionVariable.Name, index);
					// Si tiene algún miembro, busca ese miembro
					if (expressionVariable.Member != null && !string.IsNullOrWhiteSpace(expressionVariable.Member.Name))
						variable = Search(expressionVariable.Member, variable.Members, out error);
				}
				// Devuelve la variable
				return variable;
		}

		/// <summary>
		///		Calcula una expresión
		/// </summary>
		private ValueBase Compute(ExpressionsCollection stackExpressions)
		{
			Stack<ValueBase> stackOperators = new Stack<ValueBase>();
			bool error = false;

				// Calcula el resultado
				foreach (ExpressionBase expression in stackExpressions)
					if (!error)
					{
						if (expression.Token.Type == Token.TokenType.String || expression.Token.Type == Token.TokenType.Number)
							stackOperators.Push(ValueBase.GetInstance(expression.Token.Value));
						else if (expression is ExpressionVariableIdentifier ||
										 (expression is ExpressionBase && expression.Token.Type == Token.TokenType.Variable))
						{
							ValueBase variableValue;

								// Obtiene el valor de la variable
								if (expression is ExpressionVariableIdentifier)
									variableValue = GetValueVariable(expression as ExpressionVariableIdentifier);
								else
									variableValue = GetValueVariable(new ExpressionVariableIdentifier(expression.Token));
								// Añade el resultado a la pila (aunque haya un error, para que así este sea el último operando en la pila)
								if (variableValue != null)
								{
									error = variableValue.HasError;
									stackOperators.Push(variableValue);
								}
								else
								{
									error = true;
									stackOperators.Push(ValueBase.GetError("No se encuentra el valor de la variable"));
								}
						}
						else
						{
							ValueBase result = null;

								// Realiza la operación
								switch (expression.Token.Value)
								{
									case "+":
									case "-":
									case "*":
									case "/":
									case ">=":
									case "<=":
									case "==":
									case "!=":
									case ">":
									case "<":
									case "||":
									case "&&":
											result = ComputeBinary(stackOperators, expression.Token.Value);
										break;
									default:
											result = ValueBase.GetError("Operador desconocido: " + expression.Token.Value);
										break;
							}
							// Añade el resultado a la pila (aunque haya error, para que así sea el último operador de la pila)
							error = result.HasError;
							stackOperators.Push(result);
						}
					}
				// Obtiene el resultado
				if (error || stackOperators.Count == 1)
					return stackOperators.Pop();
				else if (stackOperators.Count == 0)
					return ValueBase.GetError("No hay ningún operador en la pila de operaciones");
				else
					return ValueBase.GetError("Hay más de un operador en la pila de instrucciones");
		}

		/// <summary>
		///		Obtiene el valor contenido en una variable
		/// </summary>
		private ValueBase GetValueVariable(ExpressionVariableIdentifier variableIdentifier)
		{
			Variable variable = Search(variableIdentifier, out string error);

				if (!string.IsNullOrWhiteSpace(error))
					return ValueBase.GetError(error);
				else
					return variable.Value;
		}

		/// <summary>
		///		Calcula una operación con dos valores
		/// </summary>
		private ValueBase ComputeBinary(Stack<ValueBase> stackOperators, string operation)
		{
			if (stackOperators.Count < 2)
				return ValueBase.GetError("No existen suficientes operandos en la pila para ejecutar el operador '" + operation + "'");
			else
				return stackOperators.Pop().Execute(stackOperators.Pop(), operation);
		}

		/// <summary>
		///		Variables
		/// </summary>
		internal VariablesCollection Variables { get; set; }
	}
}
