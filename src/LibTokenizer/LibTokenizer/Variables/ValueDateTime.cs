using System;

namespace Bau.Libraries.LibTokenizer.Variables
{
	/// <summary>
	///		Valor de tipo fecha
	/// </summary>
	public class ValueDateTime : ValueBase
	{
		public ValueDateTime(DateTime dtmValue)
		{
			Value = dtmValue;
		}

		/// <summary>
		///		Ejecuta una operación
		/// </summary>
		public override ValueBase Execute(ValueBase value, string operation)
		{
			ValueBase newValue = null;
			ValueBase valueNormalized = value;

				// Normaliza el valor
				if (valueNormalized is ValueNull)
					valueNormalized = new ValueDateTime(DateTime.MinValue);
				// Ejecuta la operación
				switch (operation)
				{
					case "+":
							if (valueNormalized is ValueNumeric)
								newValue = new ValueDateTime(Value.AddDays((valueNormalized as ValueNumeric)?.Value ?? 0));
						break;
					case "-":
							if (valueNormalized is ValueNumeric)
								newValue = new ValueDateTime(Value.AddDays(-1 * (valueNormalized as ValueNumeric)?.Value ?? 0));
						break;
					case ">=":
					case "<=":
					case "==":
					case "!=":
					case ">":
					case "<":
							if (valueNormalized is ValueDateTime)
							{
								DateTime first = (valueNormalized as ValueDateTime).Value;
								DateTime second = Value;

									// Compara los números
									switch (operation)
									{
										case ">=":
												newValue = new ValueBool(first >= second);
											break;
										case "<=":
												newValue = new ValueBool(first <= second);
											break;
										case "==":
												newValue = new ValueBool(first == second);
											break;
										case "!=":
												newValue = new ValueBool(first != second);
											break;
										case ">":
												newValue = new ValueBool(first > second);
											break;
										case "<":
												newValue = new ValueBool(first < second);
											break;
									}
							}
						break;
				}
				// Crea el error
				if (newValue == null)
					newValue = ValueBase.GetError($"No se puede utilizar el operador '{operation}' entre los valores {Content} y {valueNormalized.Content}");
				// Devuelve el valor
				return newValue;
		}

		/// <summary>
		///		Fecha
		/// </summary>
		public DateTime Value { get; set; }

		/// <summary>
		///		Contenido
		/// </summary>
		public override string Content
		{
			get { return $"{Value:dd-MM-yyyy}"; }
		}

		/// <summary>
		///		Obtiene un objeto con el valor interno
		/// </summary>
		public override object InnerValue
		{
			get { return Value; }
		}
	}
}
