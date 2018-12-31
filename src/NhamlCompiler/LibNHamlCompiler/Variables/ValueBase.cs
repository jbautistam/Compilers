using System;

namespace Bau.Libraries.NhamlCompiler.Variables
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
			else if (double.TryParse(value.Replace(',', '.'), out double newValue))
				return new ValueNumeric(newValue);
			else if (value.Equals(ValueBool.TrueValue, StringComparison.CurrentCultureIgnoreCase))
				return new ValueBool(true);
			else if (value.Equals(ValueBool.FalseValue, StringComparison.CurrentCultureIgnoreCase))
				return new ValueBool(false);
			else
				return new ValueString(value);
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
