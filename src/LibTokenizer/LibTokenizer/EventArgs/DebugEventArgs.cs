using System;

namespace Bau.Libraries.LibTokenizer.EventArgs
{
	/// <summary>
	///		Argumentos de los eventos de depuración
	/// </summary>
	public class DebugEventArgs : System.EventArgs
	{
		public DebugEventArgs(string message)
		{
			Message = message;
		}

		/// <summary>
		///		Mensaje de depuración
		/// </summary>
		public string Message { get; }
	}
}
