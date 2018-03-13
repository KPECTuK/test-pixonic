using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Sim.Module.Data.Serialization
{
	/// <summary>
	///     Очень ограниченный конвертер (по умолчанию - миллисекунды, s,m,h - секунды, минуты, часы соотв).
	///     Не проверяет число.
	/// </summary>
	public sealed class TimeSpanConverter : JsonConverter
	{
		private const string QUANT_SEC_S = "s";
		private const string QUANT_MIN_S = "m";
		private const string QUANT_HOURS_S = "h";

		// error there might be any number of '.' and ',' simultaneously
		private static readonly Regex _form = new Regex(@"(?'num'[\d\.\,]+)(?'quant'(s|m|h)?)");

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			return Deserialize(reader.Value as string ?? string.Empty);
		}

		public static TimeSpan Deserialize(string source)
		{
			var match = _form.Match(source);
			if(!match.Success)
			{
				throw new FormatException();
			}

			var interval = double.Parse(match.Groups["num"].Value, CultureInfo.InvariantCulture);
			var quant = match.Groups["quant"].Value;

			if(string.Equals(quant, QUANT_SEC_S))
			{
				return TimeSpan.FromSeconds(interval);
			}

			if(string.Equals(quant, QUANT_MIN_S))
			{
				return TimeSpan.FromMinutes(interval);
			}

			if(string.Equals(quant, QUANT_HOURS_S))
			{
				return TimeSpan.FromHours(interval);
			}

			return TimeSpan.FromMilliseconds(interval);
		}

		public override bool CanConvert(Type objectType)
		{
			return typeof(TimeSpan) == objectType;
		}
	}
}