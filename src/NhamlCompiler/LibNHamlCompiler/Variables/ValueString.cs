﻿using System;

namespace Bau.Libraries.NhamlCompiler.Variables
{
	/// <summary>
	///		Valor de tipo cadena
	/// </summary>
	public class ValueString : ValueBase
	{
		public ValueString(string value)
		{ 
			// Asigna el valor
			Value = value;
			// Asigna la cadena de inicio
			if (!string.IsNullOrEmpty(Value) && Value.StartsWith("\""))
			{
				if (Value.Length == 1)
					Value = "";
				else
					Value = Value.Substring(1);
			}
			// Asigna la cadena de fin
			if (!string.IsNullOrEmpty(Value) && Value.EndsWith("\""))
			{
				if (Value.Length == 1)
					Value = "";
				else
					Value = Value.Substring(0, Value.Length - 1);
			}
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
							newValue = new ValueString(value.Content + Value);
						else
							newValue = new ValueString((value as ValueString).Value + Value);
						break;
					case ">=":
					case "<=":
					case "==":
					case "!=":
					case ">":
					case "<":
						int compare = Normalize(value.Content).CompareTo(Normalize(Content));

							// Compara las cadenas
							switch (operation)
							{
								case ">=":
										newValue = new ValueBool(compare >= 0);
									break;
								case "<=":
										newValue = new ValueBool(compare <= 0);
									break;
								case "==":
										newValue = new ValueBool(compare == 0);
									break;
								case "!=":
										newValue = new ValueBool(compare != 0);
									break;
								case ">":
										newValue = new ValueBool(compare > 0);
									break;
								case "<":
										newValue = new ValueBool(compare < 0);
									break;
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
		///		Normaliza una cadena: sin nulos, sin espacios y en mayúsculas
		/// </summary>
		private string Normalize(string value)
		{   
			// Normaliza la cadena
			if (string.IsNullOrWhiteSpace(value))
				value = "";
			// Devuelve la cadena sin espacios y en mayúscula
			return value.Trim().ToUpper();
		}

		/// <summary>
		///		Valor
		/// </summary>
		public string Value { get; set; }

		/// <summary>
		///		Contenido
		/// </summary>
		public override string Content
		{
			get { return Value; }
		}
	}
}
