using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Sim.Module.Extensions
{
	public static class UtilityExtensions
	{
		public static string ToText<T>(this IEnumerable<T> source, string header = null, Func<T, string> renderer = null)
		{
			var rows = 0;
			var builder = new StringBuilder();
			renderer = renderer ?? (@object => @object.ToString());
			if(source == null)
			{
				return
					(header != null
						? $"{header} [rows: {rows}]\n{builder}"
						: builder.ToString())
					.TrimEnd(Environment.NewLine.ToCharArray());
			}

			foreach(var item in source)
			{
				builder.AppendLine(item == null
					? "null"
					: $"{++rows}: {renderer(item)}");
			}

			return
				(header != null
					? $"{header} [rows: {rows}]\n{builder}"
					: builder.ToString())
				.TrimEnd(Environment.NewLine.ToCharArray());
		}

		public static string ToText(this IEnumerable source, string header = null, Func<object, string> renderer = null)
		{
			return ToText(source.Cast<object>(), header, renderer);
		}

		private static readonly Regex _typeTemplate = new Regex("(?'name'[^`]*)");

		private static string WriteArgs(Type type)
		{
			return
				type.IsGenericType
					? type.GetGenericArguments()
						.Aggregate(new StringBuilder(), (builder, _) => builder.Append(_.NameNice() + ", "))
						.ToString()
						.TrimEnd(", ".ToArray())
					: string.Empty;
		}

		public static string NameNice(this Type source)
		{
			if(source == null)
			{
				return "null";
			}

			return
				source.IsGenericType
					? $"{_typeTemplate.Match(source.Name).Groups["name"].Value}<{WriteArgs(source)}>"
					: source.Name;
		}

		// transformations

		[ThreadStatic] private static Random _random;
		private static readonly char[] _base62Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();

		public static string MakeUnique(this string source, int length = 6, int @base = 62)
		{
			_random = _random ?? new Random();
			var builder = new StringBuilder(source, source.Length + length);
			for(var ctr = 0; ctr < length; ctr++)
			{
				builder.Append(_base62Chars[_random.Next(@base)]);
			}

			return builder.ToString();
		}
	}
}