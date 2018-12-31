using System;

namespace Bau.Libraries.NhamlCompiler.Variables
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

				// Ejecuta la operación
				switch (operation)
				{
					case "+":
							if (value is ValueNumeric)
								newValue = new ValueNumeric((value as ValueNumeric).Value + Value);
							else
								newValue = new ValueString((value as ValueString).Value + Content);
						break;
					case "-":
							if (value is ValueNumeric)
								newValue = new ValueNumeric((value as ValueNumeric).Value - Value);
						break;
					case "/":
							if (value is ValueNumeric)
							{
								if (Value == 0)
									newValue = ValueBase.GetError("No se puede dividir por cero");
								else
									newValue = new ValueNumeric((value as ValueNumeric).Value / Value);
							}
						break;
					case "*":
							if (value is ValueNumeric)
								newValue = new ValueNumeric((value as ValueNumeric).Value * Value);
						break;
					case ">=":
					case "<=":
					case "==":
					case "!=":
					case ">":
					case "<":
							if (value is ValueString)
								newValue = new ValueString(Content).Execute(value as ValueString, operation);
							else
							{
								double first = (value as ValueNumeric).Value;
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
					newValue = ValueBase.GetError($"No se puede utilizar el operador '{operation}' entre los valores {Content} y {value.Content}");
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
	}
}
