using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace INSS.EIIR.Models.Helpers;

public static class EnumHelper
{
    public static string FromEnum<T>(this T enumValue) where T : Enum
    {
        var stringValue = JsonConvert.SerializeObject(enumValue
            , new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new StringEnumConverter() }
            }).Trim('"');

        return stringValue;
    }

    public static T ToEnum<T>(this string enumString) where T : Enum
    {
        var enumValue = JsonConvert.DeserializeObject<T>($"\"{enumString}\""
           , new JsonSerializerSettings
           {
               Converters = new List<JsonConverter> { new StringEnumConverter() }
           });

        return enumValue;
    }
}
