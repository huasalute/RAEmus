﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BizHawk.Client.Common
{
	public class LuaDocumentation : List<LibraryFunction>
	{
		public LuaDocumentation()
			:base() { }

		public string ToTASVideosWikiMarkup()
		{
			var sb = new StringBuilder();
			
			sb
				.AppendLine("[module:ListParents]")
				.AppendLine()
				.AppendLine("This page documents the the behavior and parameters of Lua functions available for the [BizHawk] emulator.")
				.AppendLine()
				.AppendLine();

			foreach (var library in this.Select(x => new { Name = x.Library, Description = x.LibraryDescription }).Distinct())
			{
				sb
					.AppendFormat("%%TAB {0}%%", library.Name)
					.AppendLine()
					.AppendLine();
				if (!string.IsNullOrWhiteSpace(library.Description))
				{
					sb
						.Append(library.Description)
						.AppendLine()
						.AppendLine();
				}

				foreach (var func in this.Where(x => x.Library == library.Name))
				{
					sb
						.AppendFormat("__{0}.{1}__%%%", func.Library, func.Name)
						.AppendLine().AppendLine()
						.AppendFormat("* {0} {1}.{2}{3}", func.ReturnType, func.Library, func.Name, func.ParameterList)
						.AppendLine().AppendLine()
						.AppendFormat("* {0}", func.Description)
						.AppendLine().AppendLine();
				}
			}

			sb.Append("%%TAB_END%%");

			return sb.ToString();
		}
	}

	public class LibraryFunction
	{
		private readonly LuaMethodAttributes _luaAttributes;
		private readonly MethodInfo _method;

		public LibraryFunction(string library, string libraryDescription, MethodInfo method)
		{
			_luaAttributes = method.GetCustomAttributes(typeof(LuaMethodAttributes), false)
				.First() as LuaMethodAttributes;
			_method = method;

			Library = library;
			LibraryDescription = libraryDescription;
		}

		public string Library { get; private set; }
		public string LibraryDescription { get; private set; }

		public string Name
		{
			get { return _luaAttributes.Name; }
		}

		public string Description
		{
			get { return _luaAttributes.Description; }
		}

		private string _paramterList = null;

		public string ParameterList
		{
			get
			{
				if (_paramterList == null)
				{
					var parameters = _method.GetParameters();

					var list = new StringBuilder();
					list.Append('(');
					for (var i = 0; i < parameters.Length; i++)
					{
						var p =
							parameters[i].ToString().Replace("System", string.Empty)
										 .Replace(" ", string.Empty)
										 .Replace(".", string.Empty)
										 .Replace("LuaInterface", string.Empty)
										 .Replace("Object[]", "object[] ")
										 .Replace("Object", "object ")
										 .Replace("Boolean[]", "bool[] ")
										 .Replace("Boolean", "bool ")
										 .Replace("String", "string ")
										 .Replace("LuaTable", "table ")
										 .Replace("LuaFunction", "func ")
										 .Replace("Nullable`1[Int32]", "int? ")
										 .Replace("Nullable`1[UInt32]", "uint? ")
										 .Replace("Byte", "byte ")
										 .Replace("Int16", "short ")
										 .Replace("Int32", "int ")
										 .Replace("Int64", "long ")
										 .Replace("Ushort", "ushort ")
										 .Replace("Ulong", "ulong ")
										 .Replace("UInt32", "uint ")
										 .Replace("UInt64", "ulong ")
										 .Replace("Double", "double ")
										 .Replace("Uint", "uint ")
										 .Replace("Nullable`1[DrawingColor]", "Color? ")
										 .Replace("DrawingColor", "Color ");

						if (parameters[i].IsOptional)
						{
							var def = parameters[i].DefaultValue != null ? parameters[i].DefaultValue.ToString() : "null";
							list.AppendFormat("[{0} = {1}]", p, def);
						}
						else
						{
							list.Append(p);
						}

						if (i < parameters.Length - 1)
						{
							list.Append(", ");
						}
					}

					list.Append(')');
					_paramterList = list.ToString();
				}

				return _paramterList;
			}
		}

		public string ReturnType
		{
			get
			{
				var returnType = _method.ReturnType.ToString();

				return returnType
					.Replace("System.", string.Empty)
					.Replace("LuaInterface.", string.Empty)
					.ToLower()
					.Trim();
			}
		}
	}
}
