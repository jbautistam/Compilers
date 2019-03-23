using System;
using System.Collections.Generic;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;

namespace Bau.Applications.CompilerTestConsole.Repository
{
	/// <summary>
	///		Clase de lectura de los datos de un script
	/// </summary>
	internal class ScriptRepository
	{
		// Constantes privadas
		private const string TagRoot = "Job";
		private const string TagScript = "Script";
		private const string TagFileName = "FileName";
		private const string TagParameter = "Parameter";
		private const string TagKey = "Key";
		private const string TagType = "Type";
		private const string TagValue = "Value";
		private const string TagNow = "Now";
		private const string TagIncrement = "Increment";
		private const string TagInterval = "Interval";
		private const string TagMode = "Mode";
		// Enumerados privados
		/// <summary>
		///		Tipo de parámetro (para no estar pasando de mayúsculas a minúsculas por el XML)
		///	</summary>
		private enum ParameterType
		{
			Unknown,
			Numeric,
			DateTime,
			String,
			Boolean
		}
		/// <summary>
		///		Tipo de intervalo
		/// </summary>
		private enum IntervalType
		{
			Year,
			Month,
			Day
		}
		/// <summary>Modo de ajuste del intervalo</summary>
		private enum IntervalMode
		{
			Unknown,
			NextMonday,
			PreviousMonday,
			MonthEnd,
			MonthStart
		}

		/// <summary>
		///		Carga los datos de un trabajo
		/// </summary>
		internal List<Models.ScriptModel> Load(string fileName)
		{
			List<Models.ScriptModel> scripts = new List<Models.ScriptModel>();

				// Carga los datos del archivo
				if (System.IO.File.Exists(fileName))
				{
					MLFile fileML = new Libraries.LibMarkupLanguage.Services.XML.XMLParser().Load(fileName);

						if (fileML != null)
							foreach (MLNode rootML in fileML.Nodes)
								if (rootML.Name == TagRoot)
									foreach (MLNode nodeML in rootML.Nodes)
										if (nodeML.Name == TagScript)
										{
											Models.ScriptModel script = new Models.ScriptModel();

												// Carga los parámetros básicos
												script.FileName = nodeML.Attributes[TagFileName].Value;
												// Carga el resto de parámetros
												foreach (MLNode childML in nodeML.Nodes)
													switch (childML.Name)
													{
														case TagParameter:
																LoadParameter(childML, script.Parameters);
															break;
													}
												// Añade el script a la colección
												scripts.Add(script);
										}
				}
				// Devuelve la colección de scripts
				return scripts;
		}

		/// <summary>
		///		Carga un parámetro
		/// </summary>
		private void LoadParameter(MLNode rootML, Dictionary<string, object> parameters)
		{
			object value = null;

				// Obtiene el valor
				switch (rootML.Attributes[TagType].Value.GetEnum(ParameterType.Unknown))
				{
					case ParameterType.Numeric:
							value = rootML.Attributes[TagValue].Value.GetDouble(0);
						break;
					case ParameterType.Boolean:
							value = rootML.Attributes[TagValue].Value.GetBool();
						break;
					case ParameterType.DateTime:
							value = ConvertDate(rootML);
						break;
					default:
							if (string.IsNullOrWhiteSpace(rootML.Attributes[TagValue].Value))
								value = rootML.Value;
							else
								value = rootML.Attributes[TagValue].Value;
						break;
				}
				// Añade el parámetro
				parameters.Add(rootML.Attributes[TagKey].Value, value);
		}

		/// <summary>
		///		Convierte una fecha
		/// </summary>
		private DateTime ConvertDate(MLNode nodeML)
		{
			int increment = nodeML.Attributes[TagIncrement].Value.GetInt(0);
			DateTime date = DateTime.Now.Date;

				// Se recoge la fecha (si se ha introducido alguna)
				if (!nodeML.Attributes[TagValue].Value.EqualsIgnoreCase(TagNow))
					date = nodeML.Attributes[TagValue].Value.GetDateTime(DateTime.Now);
				// Ajusta el valor con los parámetros del XML
				if (increment != 0)
					switch (nodeML.Attributes[TagInterval].Value.GetEnum(IntervalType.Day))
					{
						case IntervalType.Day:
								date = date.AddDays(increment);
							break;
						case IntervalType.Month:
								date = date.AddMonths(increment);
							break;
						case IntervalType.Year:
								date = date.AddYears(increment);
							break;
					}
				// Ajusta la fecha
				switch (nodeML.Attributes[TagMode].Value.GetEnum(IntervalMode.Unknown))
				{
					case IntervalMode.PreviousMonday:
							date = GetPreviousMonday(date);
						break;
					case IntervalMode.NextMonday:
							date = GetNextMonday(date);
						break;
					case IntervalMode.MonthStart:
							date = GetFirstMonthDay(date);
						break;
					case IntervalMode.MonthEnd:
							date = GetLastMonthDay(date);
						break;
				}
				// Devuelve la fecha calculada
				return date.Date;
		}

		/// <summary>
		///		Obtiene el lunes anterior a una fecha (o la misma fecha si ya es lunes)
		/// </summary>
		private DateTime GetPreviousMonday(DateTime date)
		{
			// Busca el lunes anterior
			while (date.DayOfWeek != DayOfWeek.Monday)
				date = date.AddDays(-1);
			// Devuelve la fecha
			return date;
		}

		/// <summary>
		///		Obtiene el lunes siguiente a una fecha (o la misma fecha si ya es lunes)
		/// </summary>
		private DateTime GetNextMonday(DateTime date)
		{
			// Busca el lunes anterior
			while (date.DayOfWeek != DayOfWeek.Monday)
				date = date.AddDays(1);
			// Devuelve la fecha
			return date;
		}

		/// <summary>
		///		Obtiene el primer día del mes
		/// </summary>
		private DateTime GetFirstMonthDay(DateTime date)
		{
			return new DateTime(date.Year, date.Month, 1);
		}

		/// <summary>
		///		Obtiene el último día del mes
		/// </summary>
		private DateTime GetLastMonthDay(DateTime date)
		{
			return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
		}
	}
}
