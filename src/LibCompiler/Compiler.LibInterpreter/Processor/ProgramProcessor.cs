using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.Compiler.LibInterpreter.Context;
using Bau.Libraries.Compiler.LibInterpreter.Context.Variables;
using Bau.Libraries.Compiler.LibInterpreter.Processor.Sentences;

namespace Bau.Libraries.Compiler.LibInterpreter.Processor
{
	/// <summary>
	///		Clase para lectura y relleno de datos de un informe
	/// </summary>
	public abstract class ProgramProcessor
	{   
		/// <summary>
		///		Inicializa los datos de ejecución
		/// </summary>
		protected void Initialize(Compiler.ParserBase parser, string startVariableAtText = "{{", string endVariableAtText = "}}")
		{
			// Crea el intérprete
			Interpreter = new Compiler.Interpreter(parser, startVariableAtText, endVariableAtText);
			// Inicializa el contexto
			Context.Clear();
			Context.Add();
		}

		/// <summary>
		///		Ejecuta una serie de sentencias
		/// </summary>
		protected void Execute(SentenceCollection sentences)
		{
			foreach (SentenceBase abstractSentence in sentences)
				if (!Stopped)
					switch (abstractSentence)
					{
						case SentenceException sentence:
								ExecuteException(sentence);
							break;
						case SentencePrint sentence:
								ExecutePrint(sentence);
							break;
						case SentenceDeclare sentence:
								ExecuteDeclare(sentence);
							break;
						case SentenceLet sentence:
								ExecuteLet(sentence);
							break;
						case SentenceFor sentence:
								ExecuteFor(sentence);
							break;
						case SentenceIf sentence:
								ExecuteIf(sentence);
							break;
						case SentenceWhile sentence:
								ExecuteWhile(sentence);
							break;
						case SentenceFunction sentence:
								ExecuteFunctionDeclare(sentence);
							break;
						case SentenceCallFunction sentence:
								ExecuteFunctionCall(sentence);
							break;
						case SentenceReturn sentence:
								ExecuteFunctionReturn(sentence);
							break;
						default:
								Execute(abstractSentence);
							break;
					}
		}

		/// <summary>
		///		Llama al procesador principal para ejecutar una sentencia desconocida
		/// </summary>
		protected abstract void Execute(SentenceBase abstractSentence);

		/// <summary>
		///		Ejecuta una serie de sentencias creando un contexto nuevo
		/// </summary>
		protected void ExecuteWithContext(SentenceCollection sentences)
		{
			// Crea el contexto
			Context.Add();
			// Ejecuta las sentencias
			Execute(sentences);
			// Elimina el contexto
			Context.Pop();
		}

		/// <summary>
		///		Ejecuta una sentencia de declaración
		/// </summary>
		private void ExecuteDeclare(SentenceDeclare sentence)
		{
			// Ejecuta la sentencia
			Context.Actual.VariablesTable.Add(sentence.Name, sentence.Value);
			// Debug
			AddDebug($"Declare {sentence.Name} = " + ConvertObjectValue(sentence.Value));
		}

		/// <summary>
		///		Convierte el valor de un objeto a una cadena para depuración
		/// </summary>
		private string ConvertObjectValue(object value)
		{
			if (value == null)
				return "null";
			else
				return value.ToString();
		}

		/// <summary>
		///		Ejecuta una sentencia de asignación
		/// </summary>
		private void ExecuteLet(SentenceLet sentence)
		{
			if (string.IsNullOrWhiteSpace(sentence.Variable))
				AddError("Cant find the variable name");
			else
			{
				VariableModel variable = Context.Actual.VariablesTable.Get(sentence.Variable);

					// Depuración
					AddDebug($"Let {sentence.Variable} = {sentence.Expression}");
					// Declara la variable si no existía
					if (variable == null && sentence.Type != VariableModel.VariableType.Unknown)
					{
						// Añade una variable con el valor predeterminado
						Context.Actual.VariablesTable.Add(sentence.Variable, sentence.Type);
						// Obtiene la variable
						variable = Context.Actual.VariablesTable.Get(sentence.Variable);
					}
					// Ejecuta la sentencia
					if (variable == null)
						AddError($"Variable {sentence.Variable} not declared");
					else
					{
						VariableModel result = Interpreter.EvaluateExpression(Context, sentence.Expression, out string error);

							if (!string.IsNullOrWhiteSpace(error))
								AddError($"Error when evaluate expression {sentence.Expression}. {error}");
							else
								variable.Value = result.Value;
					}
			}
		}

		/// <summary>
		///		Ejecuta una sentencia for
		/// </summary>
		private void ExecuteFor(SentenceFor sentence)
		{
			if (string.IsNullOrWhiteSpace(sentence.Variable))
				AddError("Cant find the variable name for loop index");
			else if (string.IsNullOrWhiteSpace(sentence.StartExpression))
				AddError("Cant find the start expression for loop index");
			else if (string.IsNullOrWhiteSpace(sentence.EndExpression))
				AddError("Cant find the end expression for loop index");
			else
			{
				VariableModel start = GetVariableValue(sentence.StartExpression, $"StartIndex_Context{Context.Actual.ScopeIndex}");
				VariableModel end = GetVariableValue(sentence.EndExpression, $"EndIndex_Context{Context.Actual.ScopeIndex}");

					if (start.Type != end.Type)
						AddError("The types of start and end variable at for loop are distinct");
					else if (start.Type != VariableModel.VariableType.Numeric && start.Type != VariableModel.VariableType.Date)
						AddError("The value of start and end at for loop must be numeric or date");
					else
					{
						VariableModel step = GetVariableValue(NormalizeStepExpression(sentence.StepExpression), $"StepIndex_Context{Context.Actual.ScopeIndex}");

							if (step.Value == null)
								AddError("Cant find any value to step in for loop");
							else
							{
								VariableModel index = new VariableModel(sentence.Variable, start.Value);
								bool isPositiveStep = step.IsGreaterThan(0);

									// Abre un nuevo contexto
									Context.Add();
									// Añade la variable al contexto
									Context.Actual.VariablesTable.Add(sentence.Variable, index.Value);
									// Ejecuta las sentencias
									while (!IsEndForLoop(index, end, isPositiveStep))
									{
										// Ejecuta las sentencias
										Execute(sentence.Sentences);
										// Incrementa / decrementa el valor al índice (el step debería ser -x si es negativo, por tanto, siempre se suma)
										index.Sum(step);
										// y lo ajusta en el contexto
										Context.Actual.VariablesTable.Add(index);
									}
									// Elimina el contexto
									Context.Pop();
							}
					}
			}
		}

		/// <summary>
		///		Si la expresión del paso de un bucle for está vacía, devuelve un paso con el valor 1
		/// </summary>
		private string NormalizeStepExpression(string stepExpression)
		{
			if (string.IsNullOrEmpty(stepExpression))
				return "1";
			else
				return stepExpression;
		}

		/// <summary>
		///		Comprueba si se ha terminado un bucle for
		/// </summary>
		private bool IsEndForLoop(VariableModel index, VariableModel end, bool isPositiveStep)
		{
			if (isPositiveStep)
				return index.IsGreaterThan(end);
			else
				return index.IsLessThan(end);
		}

		/// <summary>
		///		Obtiene el valor de una variable
		/// </summary>
		private VariableModel GetVariableValue(string expression, string name)
		{
			VariableModel variable = Context.Actual.VariablesTable.GetIfExists(expression);

				// Si no se ha encontrado la variable, supone que la expresión es un entero o una fecha y crea la variable a partir de la cadena
				if (variable == null)
				{
					// Crea una nueva variable
					variable = new VariableModel(name, null);
					// Comprueba si la expresión es un entero o una fecha
					if (expression.GetDouble() != null)
						variable.Value = expression.GetDouble();
					else if (expression.GetDateTime() != null)
						variable.Value = expression.GetDateTime();
					else
						variable.Value = expression;
				}
				// Devuelve la variable
				return variable;
		}

		/// <summary>
		///		Ejecuta una sentencia de excepción
		/// </summary>
		private void ExecuteException(SentenceException sentence)
		{
			AddError(sentence.Message);
		}

		/// <summary>
		///		Ejecuta la sentencia de impresión
		/// </summary>
		private void ExecutePrint(SentencePrint sentence)
		{
			string text = Interpreter.EvaluateText(Context.Actual, sentence.Message, out string error);

				// Log
				AddDebug($"Print: {sentence.Message}");
				// Añade el resultado de la sentencia
				if (!string.IsNullOrWhiteSpace(error))
					AddError($"Error when execute print: {error}");
				else
					AddConsoleOutput(text);
		}

		/// <summary>
		///		Ejecuta una sentencia condicional
		/// </summary>
		private void ExecuteIf(SentenceIf sentence)
		{
			if (string.IsNullOrWhiteSpace(sentence.Condition))
				AddError("Cant find condition for if sentence");
			else
			{
				bool result = Interpreter.EvaluateCondition(Context, sentence.Condition, out string error);

					if (!string.IsNullOrEmpty(error))
						AddError(error);
					else if (result && sentence.SentencesThen.Count > 0)
						ExecuteWithContext(sentence.SentencesThen);
					else if (!result && sentence.SentencesElse.Count > 0)
						ExecuteWithContext(sentence.SentencesElse);
			}
		}

		/// <summary>
		///		Ejecuta un bucle while
		/// </summary>
		private void ExecuteWhile(SentenceWhile sentence)
		{
			if (string.IsNullOrWhiteSpace(sentence.Condition))
				AddError("Cant find condition for while loop");
			else 
			{
				bool result = Interpreter.EvaluateCondition(Context, sentence.Condition, out string error);

					if (!string.IsNullOrEmpty(error))
						AddError(error);
					else if (result)
						ExecuteWithContext(sentence.Sentences);
			}
		}

		/// <summary>
		///		Ejecuta la declaración de una función: añade la función a la tabla de funciones del contexto
		/// </summary>
		private void ExecuteFunctionDeclare(SentenceFunction sentence)
		{
			if (string.IsNullOrWhiteSpace(sentence.Name))
				AddError("Cant find name for function declare");
			else
				Context.Actual.FunctionsTable.Add(sentence.Name, sentence);
		}

		/// <summary>
		///		Ejecuta una llamada a una función
		/// </summary>
		private void ExecuteFunctionCall(SentenceCallFunction sentence)
		{
			if (string.IsNullOrWhiteSpace(sentence.Function))
				AddError("Cant find the name function for call");
			else
			{
				SentenceFunction function = Context.Actual.FunctionsTable.GetIfExists(sentence.Function);

					if (function == null)
						AddError($"Cant find the function to call: {sentence.Function}");
					else
					{
						string error = string.Empty;

							// Crea un nuevo contexto
							Context.Add();
							// Añade los argumentos al contexto
							foreach (VariableModel argument in function.Arguments)
								if (string.IsNullOrWhiteSpace(error))
								{
									int index = function.Arguments.IndexOf(argument);

										// Si el argumento corresponde a un parámetro, se añade al contexto esa variable con el valor
										// Si no, se añade la variable con su valor predeterminado
										if (sentence.ParameterExpressions.Count > index)
										{
											VariableModel result = Interpreter.EvaluateExpression(Context, sentence.ParameterExpressions[index], out error);

												if (string.IsNullOrWhiteSpace(error))
													Context.Actual.VariablesTable.Add(argument.Name, result.Value);
										}
										else
											Context.Actual.VariablesTable.Add(argument);
								}
							// Si no hay ningún error, ejecuta las sentencias de la función
							if (!string.IsNullOrWhiteSpace(error))
								AddError(error);
							else
							{
								// Añade el nombre que tiene que tener el valor de retorno
								Context.Actual.ScopeFuntionResultVariable = "Return_" + Guid.NewGuid().ToString();
								// Ejecuta las sentencias de la función
								Execute(function.Sentences);
								// Obtiene el resultado de la función
								if (!string.IsNullOrWhiteSpace(sentence.ResultVariable))
								{
									VariableModel variable = Context.Actual.Parent.VariablesTable.GetIfExists(sentence.ResultVariable);

										if (variable == null)
											AddError($"The variable {sentence.ResultVariable} is undefined");
										else
										{
											VariableModel result = Context.Actual.VariablesTable.GetIfExists(Context.Actual.ScopeFuntionResultVariable);

												if (result == null)
													AddError($"Cant find result for function {sentence.Function}");
												else
													variable.Value = result.Value;
										}
								}
							}
							// Elimina el contexto
							Context.Pop();
					}
			}
		}

		/// <summary>
		///		Ejecuta la sentencia para devolver el resultado de una función
		/// </summary>
		private void ExecuteFunctionReturn(SentenceReturn sentence)
		{
			if (string.IsNullOrWhiteSpace(Context.Actual.ScopeFuntionResultVariable))
				AddError("Cant execute a return because there is not function block");
			else
			{
				VariableModel result = Interpreter.EvaluateExpression(Context, sentence.Expression, out string error);

					// Si no hay error, añade el resultado al contexto
					if (!string.IsNullOrWhiteSpace(error))
						AddError(error);
					else
						Context.Actual.VariablesTable.Add(new VariableModel(Context.Actual.ScopeFuntionResultVariable, result.Value));
			}
		}

		/// <summary>
		///		Añade un mensaje de depuración
		/// </summary>
		protected abstract void AddDebug(string message);

		/// <summary>
		///		Añade un mensaje informativo
		/// </summary>
		protected abstract void AddInfo(string message);

		/// <summary>
		///		Añade un error
		/// </summary>
		protected abstract void AddError(string error);

		/// <summary>
		///		Escribe un mensaje en la consola
		/// </summary>
		protected abstract void AddConsoleOutput(string message);

		/// <summary>
		///		Intérprete de expresiones
		/// </summary>
		private Compiler.Interpreter Interpreter { get; set; }

		/// <summary>
		///		Contexto de ejecución
		/// </summary>
		protected ContextStackModel Context { get; } = new ContextStackModel();

		/// <summary>
		///		Indica si se ha detenido el programa por una excepción
		/// </summary>
		protected bool Stopped { get; set; }
	}
}