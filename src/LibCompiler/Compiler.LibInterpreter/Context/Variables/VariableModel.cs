using System;

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

		public VariableModel(string name, object value)
		{
			Name = name;
			Value = value;
		}

		/// <summary>
		///		Nombre de la variable
		/// </summary>
		public string Name { get; }

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
						return VariableType.Unknown;
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
		///		Valor de la variable
		/// </summary>
		public object Value { get; set; }
	}
}
