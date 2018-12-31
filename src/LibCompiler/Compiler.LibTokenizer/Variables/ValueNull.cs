using System;

namespace Bau.Libraries.Compiler.LibTokenizer.Variables
{
	/// <summary>
	///		Valor nulo
	/// </summary>
	public class ValueNull : ValueBase
	{
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
								newValue = new ValueNumeric((value as ValueNumeric).Value);
							else
								newValue = new ValueString((value as ValueString).Value);
						break;
					case "==":
							newValue = new ValueBool(value is ValueNull);
						break;
					case "!=":
							newValue = new ValueBool(!(value is ValueNull));
						break;
				}
				// Crea el error
				if (newValue == null)
					newValue = this;
				// Devuelve el valor
				return newValue;
		}

		/// <summary>
		///		Contenido del valor
		/// </summary>
		public override string Content
		{
			get { return "null"; }
		}

		/// <summary>
		///		Obtiene un objeto con el valor interno
		/// </summary>
		public override object InnerValue
		{
			get { return null; }
		}
	}
}
