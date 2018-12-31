using System;
using System.Collections.Generic;

using Bau.Libraries.Compiler.LibInterpreter.Context.Variables;

namespace Bau.Libraries.Compiler.LibInterpreter.Context
{
	/// <summary>
	///		Pila de <see cref="ContextModel"/>
	/// </summary>
	public class ContextStackModel
	{
		/// <summary>
		///		Añade un contexto a la pila
		/// </summary>
		public void Add()
		{
			Contexts.Add(new ContextModel(Actual));
		}

		/// <summary>
		///		Limpia el contexto
		/// </summary>
		public void Clear()
		{
			Contexts.Clear();
		}

		/// <summary>
		///		Quita el último contexto de la pila
		/// </summary>
		public void Pop()
		{
			if (Contexts.Count > 0)
				Contexts.RemoveAt(Contexts.Count - 1);
		}

		///// <summary>
		/////		Evalúa una condición
		///// </summary>
		//public bool EvaluateCondition(Expressions.ExpressionsCollection expressions, out string error)
		//{
		//	VariableModel result = new Evaluator.ExpressionCompute().Evaluate(Actual, expressions, out error);

		//		// Calcula el resultado
		//		if (!string.IsNullOrWhiteSpace(error) || result == null || result.Value == null)
		//			return false;
		//		else if (result.Type != VariableModel.VariableType.Boolean)
		//		{
		//			error = "Result isn't a logical value";
		//			return false;
		//		}
		//		else
		//			return (bool) result.Value;
		//}

		///// <summary>
		/////		Evalúa una expresión
		///// </summary>
		//public VariableModel EvaluateExpression(Expressions.ExpressionsCollection expressions, out string error)
		//{
		//	return new Evaluator.ExpressionCompute().Evaluate(Actual, expressions, out error);
		//}

		///// <summary>
		/////		Obtiene las variables de contexto
		///// </summary>
		//private TableVariableModel GetContextVariables()
		//{
		//	var variables = new TableVariableModel(Actual);

		//		// Obtiene una tabla con todas las variables de los contextos
		//		foreach (ContextModel context in Contexts)
		//			foreach (KeyValuePair<string, VariableModel> variable in context.Variables.GetAll())
		//				variables.Add(variable.Value);
		//		// Devuelve las variables
		//		return variables;
		//}

		/// <summary>
		///		Contexto actual
		/// </summary>
		public ContextModel Actual
		{
			get 
			{
				if (Contexts.Count == 0)
					return null;
				else
					return Contexts[Contexts.Count - 1];
			}
		}

		/// <summary>
		///		Contextos
		/// </summary>
		private List<ContextModel> Contexts { get; } = new List<ContextModel>();
	}
}
