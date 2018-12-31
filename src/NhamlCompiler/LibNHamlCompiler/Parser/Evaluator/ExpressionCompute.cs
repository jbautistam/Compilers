using System;
using System.Collections.Generic;

using Bau.Libraries.NhamlCompiler.Parser.Instructions;
using Bau.Libraries.NhamlCompiler.Variables;

namespace Bau.Libraries.NhamlCompiler.Parser.Evaluator
{
	/// <summary>
	///		Clase para el cálculo de expresiones
	/// </summary>
	internal class ExpressionCompute
	{
		internal ExpressionCompute(VariablesCollection variables)
		{
			if (variables == null)
				Variables = new VariablesCollection();
			else
				Variables = variables.Clone();
		}

		/// <summary>
		///		Evalúa una serie de expresiones
		/// </summary>
		internal ValueBase Evaluate(ExpressionsCollection stackExpressions)
		{
			return Compute(stackExpressions.Clone());
		}

		/// <summary>
		///		Busca una variable en la colección
		/// </summary>
		internal Variable Search(ExpressionVariableIdentifier expressionVariable, out string error)
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
				if (expressionVariable.IndexExpressionsRPN != null && expressionVariable.IndexExpressionsRPN.Count > 0)
				{
					ValueBase indexValue = Compute(expressionVariable.IndexExpressionsRPN);

						if (indexValue.HasError)
							error = indexValue.Error;
						else if (!(indexValue is ValueNumeric))
							error = "La expresión del índice no tiene un valor numérico";
						else
							index = (int) (indexValue as ValueNumeric).Value;
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
			Stack<ValueBase> stackoperations = new Stack<ValueBase>();
			bool hasError = false;

				// Calcula el resultado
				foreach (ExpressionBase expression in stackExpressions)
					if (!hasError)
					{
						if (expression.Token.Type == Tokens.Token.TokenType.String || expression.Token.Type == Tokens.Token.TokenType.Number)
							stackoperations.Push(ValueBase.GetInstance(expression.Token.Content));
						else if (expression is ExpressionVariableIdentifier)
						{
							ValueBase variableValue = GetValueVariable(expression as ExpressionVariableIdentifier);

								// Añade el resultado a la pila (aunque haya un error, para que así este sea el último operando en la pila)
								if (variableValue != null)
								{
									hasError = variableValue.HasError;
									stackoperations.Push(variableValue);
								}
								else
								{
									hasError = true;
									stackoperations.Push(ValueBase.GetError("No se encuentra el valor de la variable"));
								}
						}
						else
						{
							ValueBase result = null;

								// Realiza la operación
								switch (expression.Token.Content)
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
											result = ComputeBinary(stackoperations, expression.Token.Content);
										break;
									default:
											result = ValueBase.GetError($"Operador desconocido: {expression.Token.Content}");
										break;
								}
								// Añade el resultado a la pila (aunque haya error, para que así sea el último operador de la pila)
								hasError = result.HasError;
								stackoperations.Push(result);
						}
					}
				// Obtiene el resultado
				if (hasError || stackoperations.Count == 1)
					return stackoperations.Pop();
				else if (stackoperations.Count == 0)
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
		private ValueBase ComputeBinary(Stack<ValueBase> stackoperations, string operation)
		{
			if (stackoperations.Count < 2)
				return ValueBase.GetError($"No existen suficientes operandos en la pila para ejecutar el operador '{operation}'");
			else
				return stackoperations.Pop().Execute(stackoperations.Pop(), operation);
		}

		/// <summary>
		///		Variables
		/// </summary>
		internal VariablesCollection Variables { get; set; }
	}
}
