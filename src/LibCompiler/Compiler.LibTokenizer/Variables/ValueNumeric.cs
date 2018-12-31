using System;

namespace Bau.Libraries.Compiler.LibTokenizer.Variables
{
	/// <summary>
	///		Valor de tipo numérico
	/// </summary>
	public class ValueNumeric : ValueBase
	{
		public ValueNumeric(double dblValue)
		{
			Value = dblValue;
		}

		/// <summary>
		///		Ejecuta una operación
		/// </summary>
		public override ValueBase Execute(ValueBase value, string operation)
		{
			ValueBase newValue = null;
			ValueBase valueNormalized = value;

				// Convierte el valor nulo en un valor numérico
				if (valueNormalized is ValueNull)
					valueNormalized = new ValueNumeric(0);
				// Ejecuta la operación
				switch (operation)
				{
					case "+":
							if (valueNormalized is ValueNumeric)
								newValue = new ValueNumeric((valueNormalized as ValueNumeric).Value + Value);
							else
								newValue = new ValueString((valueNormalized as ValueString).Value + Content);
						break;
					case "-":
							if (valueNormalized is ValueNumeric)
								newValue = new ValueNumeric((valueNormalized as ValueNumeric).Value - Value);
						break;
					case "/":
							if (valueNormalized is ValueNumeric)
							{
								if (Value == 0)
									newValue = ValueBase.GetError("No se puede dividir por cero");
								else
									newValue = new ValueNumeric((valueNormalized as ValueNumeric).Value / Value);
							}
						break;
					case "*":
							if (valueNormalized is ValueNumeric)
								newValue = new ValueNumeric((valueNormalized as ValueNumeric).Value * Value);
						break;
					case ">=":
					case "<=":
					case "==":
					case "!=":
					case ">":
					case "<":
							if (valueNormalized is ValueString)
								newValue = new ValueString(Content).Execute(valueNormalized as ValueString, operation);
							else
							{
								double first = (valueNormalized as ValueNumeric).Value;
								double second = Value;

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
		///		Valor numérico
		/// </summary>
		public double Value { get; set; }

		/// <summary>
		///		Contenido
		/// </summary>
		public override string Content
		{
			get { return Value.ToString(); }
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
