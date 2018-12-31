using System;
using System.Collections.Generic;

using Bau.Libraries.Compiler.LibInterpreter.Context;
using Bau.Libraries.Compiler.LibInterpreter.Context.Variables;
using Bau.Libraries.Compiler.LibInterpreter.Expressions;

namespace Bau.Libraries.Compiler.LibInterpreter.Evaluator
{
	/// <summary>
	///		Clase para el cálculo de expresiones
	/// </summary>
	public class ExpressionCompute
	{
		/// <summary>
		///		Evalúa una serie de expresiones
		/// </summary>
		public VariableModel Evaluate(ContextModel context, ExpressionsCollection expressions, out string error)
		{
			return Compute(context, new ExpressionConversorRpn().ConvertToRPN(expressions), out error);
		}

		/// <summary>
		///		Evalúa una serie de expresiones pasadas a notación polaca
		/// </summary>
		public VariableModel EvaluateRpn(ContextModel context, ExpressionsCollection expressionsRpn, out string error)
		{
			return Compute(context, expressionsRpn.Clone(), out error);
		}

		/// <summary>
		///		Busca recursivamente el valor de una variable
		/// </summary>
		private VariableModel Search(ContextModel context, ExpressionVariableIdentifier expressionVariable, out string error)
		{
			VariableModel variable = null;
			int index = 0;

				// Inicializa las variables de salida
				error = string.Empty;
				// Obtiene el índice asociado a la variable
				if (expressionVariable.IndexExpressionsRPN?.Count > 0)
				{
					VariableModel indexVariable = Compute(context, expressionVariable.IndexExpressionsRPN, out error);

						if (indexVariable.Type != VariableModel.VariableType.Numeric)
							error = "La expresión del índice no tiene un valor numérico";
						else
							index = (int) indexVariable.Value;
				}
				// Si no hay ningún error, obtiene la variable
				if (string.IsNullOrWhiteSpace(error))
					variable = context.Variables.Get(expressionVariable.Name, index);
				// Devuelve la variable
				return variable;
		}

		/// <summary>
		///		Calcula una expresión
		/// </summary>
		private VariableModel Compute(ContextModel context, ExpressionsCollection stackExpressions, out string error)
		{
			Stack<VariableModel> stackOperators = new Stack<VariableModel>();

				// Inicializa los argumentos de salida
				error = string.Empty;
				// Calcula el resultado
				foreach (ExpressionBase expressionBase in stackExpressions)
					if (string.IsNullOrWhiteSpace(error))
						switch (expressionBase)
						{
							case ExpressionConstant expression:
									stackOperators.Push(new VariableModel("Constant", expression.Value));
								break;
							case ExpressionVariableIdentifier expression:
									VariableModel variable = Search(context, expression, out error);

										// Comprueba que se haya encontrado la variable
										if (variable == null)
											error = "No se encuentra el valor de la variable";
										// Si no hay ningún error, se añade la variable a la pila
										if (string.IsNullOrWhiteSpace(error))
											stackOperators.Push(variable);
								break;
							case ExpressionOperatorBase expression:
									if (stackOperators.Count < 2)
										error = "No existen suficientes operandos en la pila para ejecutar la operación";
									else
									{
										VariableModel second = stackOperators.Pop(); //? cuidado al sacar de la pila, están al revés
										VariableModel first = stackOperators.Pop();
										VariableModel result = ComputeBinary(expression, first, second, out error);

											// Si no ha habido ningún error, se añade a la pila
											if (string.IsNullOrEmpty(error))
												stackOperators.Push(result);
									}
								break;
						}
				// Obtiene el resultado
				if (string.IsNullOrWhiteSpace(error) && stackOperators.Count == 1)
					return stackOperators.Pop();
				else if (stackOperators.Count == 0)
				{
					error = "No hay ningún operador en la pila de operaciones";
					return null; 
				}
				else
				{
					error = "Hay más de un operador en la pila de instrucciones";
					return null;
				}
		}

		/// <summary>
		///		Calcula una operación con dos valores
		/// </summary>
		private VariableModel ComputeBinary(ExpressionOperatorBase expression, VariableModel first, VariableModel second, out string error)
		{
			switch (first.Type)
			{
				case VariableModel.VariableType.Boolean:
					return ComputeBoolean(expression, first, second, out error);
				case VariableModel.VariableType.String:
					return ComputeString(expression, first, second, out error);
				case VariableModel.VariableType.Date:
					return ComputeDate(expression, first, second, out error);
				case VariableModel.VariableType.Numeric:
					return ComputeNumeric(expression, first, second, out error);
				default:
					error = "Tipo desconocido";
					return null;
			}
		}

		/// <summary>
		///		Calcula una operación numérica
		/// </summary>
		private VariableModel ComputeNumeric(ExpressionOperatorBase expression, VariableModel first, VariableModel second, out string error)
		{
			// Inicializa los argumentos de salida
			error = string.Empty;
			// Si el segundo valor es una cadena, convierte y procesa con cadenas
			switch (second.Type)
			{
				case VariableModel.VariableType.String:
					return ComputeString(expression, new VariableModel("Converted", first.Value.ToString()), second, out error);
				case VariableModel.VariableType.Date:
					return ComputeDate(expression, second, first, out error);
				case VariableModel.VariableType.Numeric:
						double firstValue = (double?) first.Value ?? 0;
						double secondValue = (double?) second.Value ?? 0;

							switch (expression)
							{
								case ExpressionOperatorMath operation:
										switch (operation.Type)
										{
											case ExpressionOperatorMath.MathType.Sum:
												return new VariableModel("Result", firstValue + secondValue);
											case ExpressionOperatorMath.MathType.Substract:
												return new VariableModel("Result", firstValue - secondValue);
											case ExpressionOperatorMath.MathType.Multiply:
												return new VariableModel("Result", firstValue * secondValue);
											case ExpressionOperatorMath.MathType.Divide:
													if (secondValue == 0)
														error = "No se puede dividir por cero";
													else
														return new VariableModel("Result", firstValue / secondValue);
												break;
											case ExpressionOperatorMath.MathType.Modulus:
												if (secondValue == 0)
													error = "No se puede calcular un módulo por cero";
												else
													return new VariableModel("Result", firstValue % secondValue);
												break;
										}
									break;
								case ExpressionOperatorLogical operation:
										switch (operation.Type)
										{
											case ExpressionOperatorLogical.LogicalType.Distinct:
												return new VariableModel("Result", firstValue != secondValue);
											case ExpressionOperatorLogical.LogicalType.Equal:
												return new VariableModel("Result", firstValue == secondValue);
											case ExpressionOperatorLogical.LogicalType.Greater:
												return new VariableModel("Result", firstValue > secondValue);
											case ExpressionOperatorLogical.LogicalType.GreaterOrEqual:
												return new VariableModel("Result", firstValue >= secondValue);
											case ExpressionOperatorLogical.LogicalType.Less:
												return new VariableModel("Result", firstValue < secondValue);
											case ExpressionOperatorLogical.LogicalType.LessOrEqual:
												return new VariableModel("Result", firstValue <= secondValue);
										}
									break;
							}
					break;
			}
			// Si ha llegado hasta aquí es porque no se ha podido evaluar la operación
			if (string.IsNullOrEmpty(error))
				error = "No se puede ejecutar esta operación con un valor numérico";
			return null;
		}

		/// <summary>
		///		Calcula una operación de fecha
		/// </summary>
		private VariableModel ComputeDate(ExpressionOperatorBase expression, VariableModel first, VariableModel second, out string error)
		{
			DateTime firstValue = (DateTime) first.Value;

				// Inicializa los valores de salida
				error = string.Empty;
				// Dependiendo del tipo del segundo valor
				switch (second.Type)
				{
					case VariableModel.VariableType.Numeric:
							if (expression is ExpressionOperatorMath operation)
							{
								double days = (double) second.Value;

									switch (operation.Type)
									{
										case ExpressionOperatorMath.MathType.Sum:
											return new VariableModel("Result", firstValue.AddDays(days));
										case ExpressionOperatorMath.MathType.Substract:
											return new VariableModel("Result", firstValue.AddDays(-1 * days));
									}
							}
						break;
					case VariableModel.VariableType.Date:
							if (expression is ExpressionOperatorLogical logical)
							{
								DateTime secondValue = (DateTime) second.Value;

									switch (logical.Type)
									{
										case ExpressionOperatorLogical.LogicalType.Distinct:
											return new VariableModel("Result", firstValue != secondValue);
										case ExpressionOperatorLogical.LogicalType.Equal:
											return new VariableModel("Result", firstValue == secondValue);
										case ExpressionOperatorLogical.LogicalType.Greater:
											return new VariableModel("Result", firstValue > secondValue);
										case ExpressionOperatorLogical.LogicalType.GreaterOrEqual:
											return new VariableModel("Result", firstValue >= secondValue);
										case ExpressionOperatorLogical.LogicalType.Less:
											return new VariableModel("Result", firstValue < secondValue);
										case ExpressionOperatorLogical.LogicalType.LessOrEqual:
											return new VariableModel("Result", firstValue <= secondValue);
									}
							}
						break;
				}
				// Si ha llegado hasta aquí es porque no se ha podido evaluar la operación
				error = "No se puede ejecutar esta operación con una fecha";
				return null;
		}

		/// <summary>
		///		Calcula una operación de cadena
		/// </summary>
		private VariableModel ComputeString(ExpressionOperatorBase expression, VariableModel first, VariableModel second, out string error)
		{
			string firstValue = first.Value.ToString();
			string secondValue = second.Value.ToString();

				// Inicializa los argumentos de salida
				error = string.Empty;
				// Ejecuta la operación
				switch (expression)
				{
					case ExpressionOperatorMath operation:
							if (operation.Type == ExpressionOperatorMath.MathType.Sum)
								return new VariableModel("Result", firstValue + secondValue);
						break;
					case ExpressionOperatorLogical operation:
							int compare = NormalizeString(firstValue).CompareTo(NormalizeString(secondValue));

								switch (operation.Type)
								{
									case ExpressionOperatorLogical.LogicalType.Distinct:
										return new VariableModel("Result", compare != 0);
									case ExpressionOperatorLogical.LogicalType.Equal:
										return new VariableModel("Result", compare == 0);
									case ExpressionOperatorLogical.LogicalType.Greater:
										return new VariableModel("Result", compare > 0);
									case ExpressionOperatorLogical.LogicalType.GreaterOrEqual:
										return new VariableModel("Result", compare >= 0);
									case ExpressionOperatorLogical.LogicalType.Less:
										return new VariableModel("Result", compare < 0);
									case ExpressionOperatorLogical.LogicalType.LessOrEqual:
										return new VariableModel("Result", compare <= 0);
								}
						break;
				}
				// Si ha llegado hasta aquí es porque no se puede ejecutar la operación
				error = "No se puede ejecutar esta operación con una cadena";
				return null;
		}

		/// <summary>
		///		Normaliza una cadena: sin nulos, sin espacios y en mayúsculas
		/// </summary>
		private string NormalizeString(string value)
		{   
			// Normaliza la cadena
			if (string.IsNullOrWhiteSpace(value))
				value = "";
			// Devuelve la cadena sin espacios y en mayúscula
			return value.Trim().ToUpper();
		}

		/// <summary>
		///		Calcula una operación lógica
		/// </summary>
		private VariableModel ComputeBoolean(ExpressionOperatorBase expression, VariableModel first, VariableModel second, out string error)
		{
			bool firstValue = (bool) first.Value;
			bool secondValue = false;

				// Inicializa los argumentos de salida
				error = string.Empty;
				// Normaliza el segundo valor
				if (second.Value != null)
					secondValue = (bool) second.Value;
				// Ejecuta la operación
					switch (expression)
					{
						case ExpressionOperatorLogical logical:
								switch (logical.Type)
								{
									case ExpressionOperatorLogical.LogicalType.Distinct:
										return new VariableModel("Result", firstValue != secondValue);
									case ExpressionOperatorLogical.LogicalType.Equal:
										return new VariableModel("Result", firstValue == secondValue);
								}
							break;
						case ExpressionOperatorRelational relational:
								switch (relational.Type)
								{
									case ExpressionOperatorRelational.RelationalType.And:
										return new VariableModel("Result", firstValue && secondValue);
									case ExpressionOperatorRelational.RelationalType.Or:
										return new VariableModel("Result", firstValue || secondValue);
								}
							break;
					}
				// Si ha llegado hasta aquí es porque no se ha podido ejecutar la operación
				error = "No se puede ejecutar la operación con valores lógicos";
				return null;
		}
	}
}
