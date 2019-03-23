using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.Compiler.LibInterpreter.Context;
using Bau.Libraries.Compiler.LibInterpreter.Context.Variables;
using Bau.Libraries.Compiler.LibInterpreter.Evaluator;
using Bau.Libraries.Compiler.LibInterpreter.Expressions;

namespace Bau.Libraries.Compiler.LibInterpreter.Processor.Compiler
{
	/// <summary>
	///		Clase para interpretación de una fórmula
	/// </summary>
	public class Interpreter
	{ 
		public Interpreter(ParserBase parser, string startVariable, string endVariable)
		{
			Parser = parser;
			StartVariable = startVariable;
			EndVariable = endVariable;
		}

		/// <summary>
		///		Evalúa una condición
		/// </summary>
		public bool EvaluateCondition(ContextStackModel context, string code, out string error)
		{
			bool result = false;
			VariableModel variable = EvaluateExpression(context, code, out error);

				// Obtiene el valor lógico de la condición
				if (string.IsNullOrEmpty(error))
				{
					if (variable.Type != VariableModel.VariableType.Boolean)
						error = "Result isn't a logical value";
					else
						result = (bool) variable.Value;
				}
				// Devuelve el resultado
				return result;
		}

		/// <summary>
		///		Evalúa una expresión
		/// </summary>
		public VariableModel EvaluateExpression(ContextStackModel context, string code, out string error)
		{
			ExpressionsCollection expressions = Parser.Parse(code, out error);

				if (string.IsNullOrEmpty(error))
					return new ExpressionCompute().Evaluate(context.Actual, expressions, out error);
				else
					return null;
		}

		/// <summary>
		///		Convierte un texto con el contenido de las variables
		/// </summary>
		public string EvaluateText(ContextModel context, string text, out string error)
		{
			TableVariableModel variables = context.GetVariablesRecursive();
			string result = "";

				// Inicializa los argumentos de salida
				error = string.Empty;
				// Mientras quede algo de texto por interpretar ...
				while (!string.IsNullOrWhiteSpace(text) && string.IsNullOrEmpty(error))
				{
					int start = text.IndexOf(StartVariable);

						// Interpreta la variable
						if (start >= 0)
						{
							int end;

								// Añade el inicio al resultado
								result += text.Left(start);
								// Quita el token de inicio de la variable del texto
								text = text.From(start + EndVariable.Length);
								// Obtiene el índice de la parte final de la variable
								end = text.IndexOf(EndVariable);
								// Si se ha encontrado un nombre de variable
								if (end >= 0)
								{
									string variable = text.Left(end);
									string format = GetFormat(variable);
									VariableModel symbol;

										// Quita el nombre de la variable del texto
										text = text.From(end + EndVariable.Length);
										// Si se ha encontrado un formato se quita del nombre de la cadena
										if (!format.IsEmpty())
											variable = variable.Left(variable.Length - format.Length - 1);
										// Interpreta la cadena
										symbol = variables.Get(variable);
										// Si se ha encontrado, se añade el mismo valor al resultando
										if (symbol == null)
											error = $"Can't find the variable {variable}";
										else
											result += ConvertStringValue(symbol.Value, format);
								}
								else //... no hay cadena de fin de variable se añade al resultado el comienzo de variable
									result += StartVariable;
						}
						else
						{
							result += text;
							text = "";
						}
				}
				// Devuelve el contenido
				return result;
		}

		/// <summary>
		///		Convierte el valor de una cadena
		/// </summary>
		private string ConvertStringValue(object value, string format)
		{
			if (value is int || value is long || value is float || value is double || value is decimal)
				return ConvertDouble(value.ToString().GetDouble(), format);
			else if (value is DateTime date)
				return ConvertDateTime(date, format);
			else if (value is bool boolean)
				return ConvertBoolean(boolean, format);
			else
				return ConvertString(value?.ToString(), format);
		}

		/// <summary>
		///		Convierte un valor numérico
		/// </summary>
		private string ConvertDouble(double? value, string format)
		{
			return ConvertObject(value, format);
		}

		/// <summary>
		///		Convierte un valor de fecha
		/// </summary>
		private string ConvertDateTime(DateTime? value, string format)
		{
			if (string.IsNullOrWhiteSpace(format))
				return $"#{value:yyyy-MM-dd HH:mm:ss}#";
			else
				return ConvertObject(value, format);
		}

		/// <summary>
		///		Convierte un valor de cadena
		/// </summary>
		private string ConvertString(string value, string format)
		{
			return ConvertObject(value, format);
		}

		/// <summary>
		///		Convierte un valor lógico
		/// </summary>
		private string ConvertBoolean(bool value, string format)
		{
			return ConvertObject(value, format);
		}

		/// <summary>
		///		Convierte un objeto
		/// </summary>
		private string ConvertObject(object value, string format)
		{
			if (value == null)
				return "NULL";
			else if (!string.IsNullOrEmpty(format))
				return string.Format("{0:" + format + "}", value);
			else
				return value.ToString();
		}

		/// <summary>
		///		Obtiene el formato asociado a una variable
		/// </summary>
		private string GetFormat(string variable)
		{
			string format = string.Empty;
			int startIndex = variable.IndexOf(":");

				// Obtiene el formato
				if (startIndex >= 0)
					format = variable.From(startIndex + 1).TrimIgnoreNull();
				// Devuelve el formato encontrado
				return format;
		}

		/// <summary>
		///		Intérprete
		/// </summary>
		public ParserBase Parser { get; }

		/// <summary>
		///		Cadena que indica el comienzo del nombre de una variable en texto
		/// </summary>
		public string StartVariable { get; }

		/// <summary>
		///		Cadena que indica el comienzo fin del nombre de una variable en texto
		/// </summary>
		public string EndVariable { get; }
	}
}