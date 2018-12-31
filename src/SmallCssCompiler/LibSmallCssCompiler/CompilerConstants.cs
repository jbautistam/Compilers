using System;

namespace Bau.Libraries.LibSmallCssCompiler
{
	/// <summary>
	///		Constantes del compilador
	/// </summary>
	internal static class CompilerConstants
	{ 
		internal const string MixinDefinition = "$mixin";
		internal const string MixinInclude = "$include";
		internal const string IfDefined = "$ifdefined";
		internal const string Variable = "$";
		internal const string VariableSeparator = ":";
		internal const string Import = "$import";
		internal const string Media = "@media";
		internal const string Remark = "_";
	}
}
