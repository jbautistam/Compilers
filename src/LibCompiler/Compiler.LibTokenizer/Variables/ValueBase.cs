using System;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.Compiler.LibTokenizer.Variables
{
	/// <summary>
	///		Valor
	/// </summary>
	public abstract class ValueBase
	{
		/// <summary>
		///		Obtiene un valor predeterminado a partir del tipo de contenido
		/// </summary>
		public static ValueBase GetInstance(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
				return new ValueString("");
			else if (value.EqualsIgnoreCase("null"))
				return new ValueNull();
			else if (double.TryParse(value.Replace(',', '.'), System.Globalization.NumberStyles.AllowDecimalPoint, 
					 System.Globalization.CultureInfo.InvariantCulture, out double number))
				return new ValueNumeric(number);
			else if (DateTime.TryParse(value, out DateTime date))
				return new ValueDateTime(date);
			else if (value.Equals(ValueBool.TrueValue, StringComparison.CurrentCultureIgnoreCase))
				return new ValueBool(true);
			else if (value.Equals(ValueBool.FalseValue, StringComparison.CurrentCultureIgnoreCase))
				return new ValueBool(false);
			else
				return new ValueString(value);
		}

		/// <summary>
		///		Obtiene un valor predeterminado a partir del tipo de contenido
		/// </summary>
		public static ValueBase GetInstance(object value)
		{
			if (value == null)
				return new ValueNull();
			else if (value is int || value is double || value is decimal || value is byte ||
					 value is Int16 || value is Int32 || value is Int64 || value is float || value is long)
				return new ValueNumeric(Convert.ToDouble(value));
			else if (value is bool)
				return new ValueBool(Convert.ToBoolean(value));
			else if (value is DateTime)
				return new ValueDateTime(Convert.ToDateTime(value));
			else
				return new ValueString(value.ToString());
		}

		/// <summary>
		///		Obtiene un valor predeterminado con un error
		/// </summary>
		public static ValueBase GetError(string error)
		{
			ValueString value = new ValueString("ERROR");

				// Asigna el error
				value.Error = error;
				// Devuelve el valor
				return value;
		}

		/// <summary>
		///		Ejecuta una operación
		/// </summary>
		public abstract ValueBase Execute(ValueBase value, string operation);

		/// <summary>
		///		Contenido del valor (numérico, cadena ...)
		/// </summary>
		public abstract string Content { get; }

		/// <summary>
		///		Objeto contenido en el valor
		/// </summary>
		public abstract object InnerValue { get; }

		/// <summary>
		///		Comprueba si hay algún error en el valor
		/// </summary>
		public bool HasError
		{
			get { return !string.IsNullOrEmpty(Error); }
		}

		/// <summary>
		///		Error encontrado en la última operación
		/// </summary>
		public string Error { get; private set; }
	}
}
