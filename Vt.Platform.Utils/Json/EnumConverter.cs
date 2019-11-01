using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Vt.Platform.Utils.Json
{
    public class EnumConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var model = value as Enum;
            if (model == null)
            {
                writer.WriteValue(value);
            }
            else
            {
                var newModel = model.ToEnumDetails();
                serializer.Serialize(writer, newModel);
            }

            writer.Flush();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var t = objectType;
            // UNWRAP NULLABLE TYPE IF PRESENT
            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                t = Nullable.GetUnderlyingType(t);
            }

            if (reader.TokenType == JsonToken.StartObject)
            {
                var props = new List<Tuple<string, object>>();
                do
                {
                    reader.Read();
                    if (reader.TokenType == JsonToken.PropertyName)
                    {
                        var prop = reader.Value as string;
                        reader.Read();
                        var propVal = reader.Value;
                        props.Add(Tuple.Create(prop, propVal));
                    }
                } while (reader.TokenType != JsonToken.EndObject);

                if (
                    props.Any(x => x.Item1.Equals("name", StringComparison.InvariantCultureIgnoreCase)) &&
                    props.Any(x => x.Item1.Equals("value", StringComparison.InvariantCultureIgnoreCase))
                )
                {
                    var enumName = props.First(x => x.Item1.Equals("name", StringComparison.InvariantCultureIgnoreCase))
                        .Item2 as string;
                    var enumValue = props.First(x => x.Item1.Equals("value", StringComparison.InvariantCultureIgnoreCase))
                        .Item2 as long?;

                    if (string.IsNullOrWhiteSpace(enumName) || enumValue == null)
                    {
                        return null;
                    }

                    var result = Enum.Parse(t, enumValue.ToString());
                    if ((int)result == Convert.ToInt32(enumValue.Value))
                    {
                        return result;
                    }
                }
                else
                {
                    return null;
                }
            }

            var val = reader.Value;
            if (val == null) return null;

            try
            {
                if (val is string)
                {
                    var result = Enum.Parse(t, val.ToString(), ignoreCase: true);
                    return result;
                }
                else
                {
                    var result = Enum.ToObject(t, val);
                    return result;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public override bool CanConvert(Type objectType)
        {
            if (objectType.IsEnum) return true;

            Type t = Nullable.GetUnderlyingType(objectType);
            return t?.IsEnum ?? false;
        }
    }
}