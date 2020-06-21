using System;

namespace Bau.Libraries.Compiler.LibInterpreter.Expressions
{
	/// <summary>
	///		Expresión con los datos de una constante
	/// </summary>
    public class ExpressionConstant : ExpressionBase
    {
		public ExpressionConstant(Context.Variables.VariableModel.VariableType type, object value)
		{
			Type = type;
			Value = value;
		}

		/// <summary>
		///		Clona la expresión
		/// </summary>
		public override ExpressionBase Clone()
		{
			return new ExpressionConstant(Type, Value);
		}

		/// <summary>
		///		Obtiene la información de depuración
		/// </summary>
		public override string GetDebugInfo()
		{
			return $"[{Type.ToString()} - {Value}]";
		}

		/// <summary>
		///		Tipo de la constante
		/// </summary>
		public Context.Variables.VariableModel.VariableType Type { get; }

		/// <summary>
		///		Valor de la constante
		/// </summary>
		public object Value { get; }
	}
}
