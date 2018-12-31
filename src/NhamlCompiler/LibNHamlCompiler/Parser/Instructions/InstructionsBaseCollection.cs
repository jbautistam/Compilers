using System;
using System.Collections.Generic;

namespace Bau.Libraries.NhamlCompiler.Parser.Instructions
{
	/// <summary>
	///		Colección de <see cref="InstructionBase"/>
	/// </summary>
	internal class InstructionsBaseCollection : List<InstructionBase>
	{
		/// <summary>
		///		Cadena con el contenido de la colección
		/// </summary>
		internal string GetDebugString(int indent = 0)
		{
			string message = "";

				// Obtiene la cadena de depuración
				foreach (InstructionBase instruction in this)
					message += Environment.NewLine + instruction.GetDebugString(indent);
				// Devuelve el mensaje
				return message;
		}

		/// <summary>
		///		Selecciona las primeras n instrucciones
		/// </summary>
		internal InstructionsBaseCollection Select(int maxInstructions)
		{
			InstructionsBaseCollection instructions = new InstructionsBaseCollection();

				// Obtiene las primeras instrucciones
				for (int index = 0; index < maxInstructions && index < Count; index++)
					instructions.Add(this[index]);
				// Devuelve las instrucciones
				return instructions;
		}
	}
}
