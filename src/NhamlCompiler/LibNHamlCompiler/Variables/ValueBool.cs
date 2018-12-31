using System;

namespace Bau.Libraries.NhamlCompiler.Variables
{
	/// <summary>
	///		Valor lógico
	/// </summary>
	public class ValueBool : ValueBase
	{ 
		// Constantes internas
		public const string TrueValue = "#true#";
		public const string FalseValue = "#false#";

		public ValueBool(bool value)
		{
			Value = value;
		}

		/// <summary>
		///		Ejecuta una operación aritmética / lógica
		/// </summary>
		public override ValueBase Execute(ValueBase value, string operation)
		{
			ValueBase newValue = null;

			// Ejecuta la operación
			if (value is ValueBool)
			{
				ValueBool second = value as ValueBool;

					switch (operation)
					{
						case "==":
								newValue = new ValueBool(Value == second.Value);
							break;
						case "!=":
								newValue = new ValueBool(Value != second.Value);
							break;
						case "&&":
								newValue = new ValueBool(Value && second.Value);
							break;
						case "||":
								newValue = new ValueBool(Value || second.Value);
							break;
					}
			}
			// Crea el error
			if (newValue == null)
				newValue = ValueBase.GetError($"No se puede utilizar el operador '{operation}' entre los valores {Content} y {value.Content}");
			// Devuelve el valor
			return newValue;
		}

		/// <summary>
		///		Valor
		/// </summary>
		public bool Value { get; set; }

		/// <summary>
		///		Contenido en formato cadena
		/// </summary>
		public override string Content
		{
			get
			{
				if (Value)
					return TrueValue;
				else
					return FalseValue;
			}
		}
	}
}
