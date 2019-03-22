using System;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.Compiler.LibInterpreter.Context.Variables
{
	/// <summary>
	///		Clase con los datos de una variable
	/// </summary>
	public class VariableModel
	{
		/// <summary>
		///		Tipo de variable
		/// </summary>
		public enum VariableType
		{
			/// <summary>Desconocida: se utiliza cuando el compilador infiere el tipo de la variable</summary>
			Unknown,
			/// <summary>Cadena</summary>
			String,
			/// <summary>Valor numérico</summary>
			Numeric,
			/// <summary>Valor lógico</summary>
			Boolean,
			/// <summary>Valor de fecha</summary>
			Date
		}
		/// <summary>
		///		Resultado de una comparación
		/// </summary>
		public enum CompareResult
		{
			/// <summary>Ambos datos son iguales</summary>
			Equals,
			/// <summary>El primer dato es mayor que el segundo</summary>
			GreaterThan,
			/// <summary>El primer dato es menor que el segundo</summary>
			LessThan
		}
		/// <summary>
		///		Tipos de incrementos de fecha
		/// </summary>
		private enum DateIncrement
		{
			/// <summary>Por días</summary>
			Day,
			/// <summary>Por semanas</summary>
			Week,
			/// <summary>Por mes</summary>
			Month,
			/// <summary>Por año</summary>
			Year
		}

		public VariableModel(string name, object value)
		{
			Name = name;
			Value = value;
		}

		public VariableModel(string name, VariableType type)
		{
			Name = name;
			DeclaredType = type;
		}

		/// <summary>
		///		Nombre de la variable
		/// </summary>
		public string Name { get; }

		/// <summary>
		///		Tipo declarado
		/// </summary>
		public VariableType DeclaredType { get; } = VariableType.Unknown;

		/// <summary>
		///		Obtiene el tipo de la variable
		/// </summary>
		public VariableType Type
		{
			get
			{
				switch (Value)
				{
					case null:
						return DeclaredType;
					case bool type:
						return VariableType.Boolean;
					case string type:
						return VariableType.String;
					case DateTime type:
						return VariableType.Date;
					default:
						return VariableType.Numeric;
				}
			}
		}

		/// <summary>
		///		Comprueba si el valor de la variable es mayor que un valor
		/// </summary>
		public bool IsGreaterThan(object value)
		{
			return Compare(ConvertToVariable(value)) == CompareResult.GreaterThan;
		}

		/// <summary>
		///		Comprueba si el valor de la variable es menor que un valor
		/// </summary>
		public bool IsLessThan(object value)
		{
			return Compare(ConvertToVariable(value)) == CompareResult.LessThan;
		}

		/// <summary>
		///		Comprueba si el valor de la variable es mayor o igual que otro
		/// </summary>
		public bool IsGreaterOrEqualThan(object value)
		{
			CompareResult result = Compare(ConvertToVariable(value));

				return result == CompareResult.Equals || result == CompareResult.GreaterThan;
		}

		/// <summary>
		///		Comprueba si el valor de la variable es menor o igual que otro
		/// </summary>
		public bool IsLessOrEqualThan(object value)
		{
			CompareResult result = Compare(ConvertToVariable(value));

				return result == CompareResult.Equals || result == CompareResult.LessThan;
		}

		/// <summary>
		///		Suma el valor de una variable
		/// </summary>
		public void Sum(object value)
		{
			VariableModel variable = ConvertToVariable(value);

				switch (Type)
				{
					case VariableType.String:
							Value = Value?.ToString() + variable.Value?.ToString();
						break;
					case VariableType.Numeric:
							Value = ConvertToNumeric(this) + ConvertToNumeric(variable);
						break;
					case VariableType.Boolean:
							Value = ConvertToBoolean(this) || ConvertToBoolean(variable);
						break;
					case VariableType.Date:
							DateTime? date = ConvertToDate(this);

								if (date == null)
									throw new NotImplementedException($"Source date has a null value. Variable {Name}");
								else
									Value = SumSubstractDate(date ?? DateTime.Now, ConvertToString(variable), true);
						break;
				}
		}

		/// <summary>
		///		Resta el valor de una variable
		/// </summary>
		public void Substract(VariableModel value)
		{
			VariableModel variable = ConvertToVariable(value);

				switch (Type)
				{
					case VariableType.String:
						throw new NotImplementedException($"Cant substract a string. Variable: {Name}");
					case VariableType.Numeric:
							Value = ConvertToNumeric(this) - ConvertToNumeric(variable);
						break;
					case VariableType.Boolean:
						throw new NotImplementedException($"Cant substract a boolean. Variable: {Name}");
					case VariableType.Date:
							DateTime? date = ConvertToDate(this);

								if (date == null)
									throw new NotImplementedException($"Source date has a null value. Variable {Name}");
								else
									Value = SumSubstractDate(date ?? DateTime.Now, ConvertToString(variable), false);
						break;
				}
		}

		/// <summary>
		///		Suma un valor a una fecha
		/// </summary>
		private DateTime SumSubstractDate(DateTime value, string increment, bool mustSum)
		{
			if (string.IsNullOrWhiteSpace(increment))
				throw new NotImplementedException($"Increment string is empty. Variable {Name}");
			else 
			{
				(int incrementValue, DateIncrement type) = GetDateIncrement(increment);

					// Si se debe restar, el incremento es negativo
					if (!mustSum)
						incrementValue = -1 * incrementValue;
					// Añade / resta los días, meses...
					switch (type)
					{
						case DateIncrement.Day:
							return value.AddDays(incrementValue);
						case DateIncrement.Week:
							return value.AddDays(7 * incrementValue);
						case DateIncrement.Month:
							return value.AddMonths(incrementValue);
						case DateIncrement.Year:
							return value.AddYears(incrementValue);
						default:
							throw new NotImplementedException($"Increment type unknown. Variable {Name}. Increment {increment}");
					}
			}
		}

		/// <summary>
		///		Obtiene los valores de un incremento
		/// </summary>
		private (int increment, DateIncrement type) GetDateIncrement(string value)
		{
			DateIncrement type = DateIncrement.Day;
			string incrementValue = string.Empty;

				// Obtiene el tipo de incremento
				value = value.ToUpper();
				if (value.EndsWith("W"))
					type = DateIncrement.Week;
				else if (value.EndsWith("M"))
					type = DateIncrement.Month;
				else if (value.EndsWith("Y"))
					type = DateIncrement.Year;
				// Quita el tipo de incremento
				if (value.EndsWith("D") || value.EndsWith("W") || value.EndsWith("M") || value.EndsWith("Y"))
				{
					if (value.Length > 1)
						value = value.Substring(0, value.Length - 1);
					else
						value = "1";
				}
				// Obtiene el incremento
				if (value.GetInt() == null)
					throw new NotImplementedException("The increment has no value");
				else
					return (value.GetInt(0), type);
		}

		/// <summary>
		///		Compara el valor de la variable con un valor
		/// </summary>
		public CompareResult Compare(object value)
		{
			VariableModel target = ConvertToVariable(value);

				if (target == null && Value == null)
					return CompareResult.Equals;
				else if (target != null && Value == null)
					return CompareResult.LessThan;
				else if (target == null && Value != null)
					return CompareResult.GreaterThan;
				else
					switch (Type)
					{
						case VariableType.Numeric:
							return CompareWithNumeric(ConvertToNumeric(target));
						case VariableType.Date:
							return CompareWithDate(ConvertToDate(target));
						case VariableType.Boolean:
							return CompareWithBoolean(ConvertToBoolean(target));
						case VariableType.String:
							return CompareWithString(ConvertToString(target));
						default:
							throw new NotImplementedException($"Cant compare variable {Name} with type {Type} and the value {target}");
					}
		}

		/// <summary>
		///		Compara el valor con una cadena
		/// </summary>
		private CompareResult CompareWithString(string target)
		{
			if (Type == VariableType.String)
				switch (ConvertToString(this).ToUpperInvariant().CompareIgnoreNullTo(target.ToUpperInvariant()))
				{
					case 0:
						return CompareResult.Equals;
					case 1:
						return CompareResult.GreaterThan;
					default:
						return CompareResult.LessThan;
				}
			else
				throw new NotImplementedException($"Cant compare string with {Type}");
		}

		/// <summary>
		///		Compara el valor con un numérico
		/// </summary>
		private CompareResult CompareWithNumeric(double target)
		{
			if (Type == VariableType.Numeric)
			{
				double value = ConvertToNumeric(this);

					if (value == target)
						return CompareResult.Equals;
					else if (value > target)
						return CompareResult.GreaterThan;
					else
						return CompareResult.LessThan;
			}
			else
				throw new NotImplementedException($"Cant compare numeric with {Type}");
		}

		/// <summary>
		///		Compara el valor con una fecha
		/// </summary>
		private CompareResult CompareWithDate(DateTime? target)
		{
			if (Type == VariableType.Date)
			{
				DateTime? value = ConvertToDate(this);

					if (value == target)
						return CompareResult.Equals;
					else if (value > target)
						return CompareResult.GreaterThan;
					else
						return CompareResult.LessThan;
			}
			else
				throw new NotImplementedException($"Cant compare numeric with {Type}");
		}

		/// <summary>
		///		Compara un valor con un boolean
		/// </summary>
		private CompareResult CompareWithBoolean(bool target)
		{
			if (Type == VariableType.Boolean)
			{
				bool value = ConvertToBoolean(this);

					if (value == target)
						return CompareResult.Equals;
					else if (value)
						return CompareResult.GreaterThan;
					else
						return CompareResult.LessThan;
			}
			else
				throw new NotImplementedException($"Cant compare numeric with {Type}");
		}

		/// <summary>
		///		Convierte un objeto en una variable (o lo deja tal cual si ya era una variable)
		/// </summary>
		private VariableModel ConvertToVariable(object value)
		{
			if (value is VariableModel variable)
				return variable;
			else
				return new VariableModel(Guid.NewGuid().ToString(), value);
		}

		/// <summary>
		///		Convierte un objeto a numérico
		/// </summary>
		private double ConvertToNumeric(VariableModel variable)
		{
			return Convert.ToDouble(variable.Value);
		}

		/// <summary>
		///		Convierte un objeto a boolean
		/// </summary>
		private bool ConvertToBoolean(VariableModel variable)
		{
			return Convert.ToBoolean(variable.Value);
		}

		/// <summary>
		///		Convierte un objeto a fecha
		/// </summary>
		private DateTime? ConvertToDate(VariableModel variable)
		{
			return variable.Value.ToString().GetDateTime();
		}

		/// <summary>
		///		Convierte un objeto a un tipo determinado
		/// </summary>
		private string ConvertToString(VariableModel variable)
		{
			switch (variable.Value)
			{
				case DateTime date:
					return $"{date:yyyy-MM-dd HH:mm:ss}";
				case double number:
					return number.ToString(System.Globalization.CultureInfo.InvariantCulture);
				case decimal number:
					return number.ToString(System.Globalization.CultureInfo.InvariantCulture);
				case int number:
					return number.ToString(System.Globalization.CultureInfo.InvariantCulture);
				case float number:
					return number.ToString(System.Globalization.CultureInfo.InvariantCulture);
				case bool boolean:
					if (boolean)
						return "true";
					else
						return "false";
				case string value:
					return value;
				default:
					return variable.Value.ToString();
			}
		}

		/// <summary>
		///		Valor de la variable
		/// </summary>
		public object Value { get; set; }
	}
}
