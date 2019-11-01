using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Newtonsoft.Json;
using Tokenize.Client;

namespace Vt.Platform.Utils.Json
{
    public class TokenPropertyConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var model = value as ITokenizable;
            if (model == null)
            {
                writer.WriteValue(value);
            }
            else
            {
                var val = model.ValueObj;
                if (val != null)
                {
                    serializer.Serialize(writer, val);
                }
                else
                {

                    var token = model.Token;
                    serializer.Serialize(writer, token);
                }
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return existingValue;
        }

        public override bool CanConvert(Type objectType)
        {
            if (objectType == null) return false;
            return objectType.GetInterfaces().Contains(typeof(ITokenizable));
        }
    }
}
