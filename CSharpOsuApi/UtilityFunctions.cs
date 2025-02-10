using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;
using CSharpOsuApi.JsonUtils;

namespace CSharpOsuApi;

internal static class UtilityFunctions
{
    internal static T SerializeHttpResponseMessage<T>(HttpResponseMessage res, params object[] objects)
    {
        if (res.Content == null) throw new NullReferenceException();
        StreamReader r = new StreamReader(res.Content.ReadAsStream());
        string jsonContent = r.ReadToEnd();
        T obj;
        try
        {
            obj = JsonSerializer.Deserialize<T>(jsonContent) ?? throw new InvalidOperationException();
        }
        catch (JsonException)
        {
            try
            {
                Console.WriteLine(JsonSerializer.Serialize(JsonDocument.Parse(jsonContent)),
                    JsonOptions.PrettyPrint);
            }
            catch (FormatException)
            {
                Console.WriteLine(jsonContent);
            }
            
            throw;
        }
        
        return AddSpecialAttributesToObject(obj, objects);
    }

    internal static T AddSpecialAttributesToObject<T>(T theObject, object[] theObjects)
    {
        if (theObject == null) return theObject;
        List<int> setObjects = [];
        
        foreach (PropertyInfo propertyInfo in theObject.GetType().GetProperties())
        {
            for (int i = 0; i < theObjects.Length; i++)
            {
                if (setObjects.Contains(i)) continue;
                object obj = theObjects[i];
                if (obj.GetType() == propertyInfo.GetType())
                {
                    setObjects.Add(i);
                    propertyInfo.SetValue(propertyInfo.GetType(), obj);
                }
            }
        }

        return theObject;
    }

    internal static string GetOsuSessionStringFromOsuCookiesFile(string osuCookiesFilePath)
    {
        using StreamReader r = new StreamReader(osuCookiesFilePath);
        string cookieFile = r.ReadToEnd();
        
        char[] stopChars = ['\t', '\n'];
        string currentToken = "";
        bool seekingForCode = false;
        foreach (char letter in cookieFile)
        {
            if (stopChars.Contains(letter))
            {
                if (currentToken == "osu_session" && !seekingForCode)
                {
                    seekingForCode = true;
                }
                else if (seekingForCode)  // we got the whole code now
                {
                    return currentToken;
                }
                else
                {
                    seekingForCode = false;
                }

                currentToken = "";
                continue;
            }
            currentToken += letter;
        }
        
        // if we got here then we either couldnt find the code in the url or the code was the last part of the url
        if (currentToken.Length == 0) 
            throw new Exception($"Could not find osu_session in given cookieFile:\n{cookieFile}");
        return currentToken;
    }

    internal static void PrintAllHeaders(HttpHeaders headers)
    {
        Console.WriteLine("request headers");
        Console.WriteLine(GetAllRequestHeadersString(headers, "\n"));
        Console.WriteLine("no more request headers");
    }

    internal static string GetAllRequestHeadersString(HttpHeaders headers, string separator=", ")
    {
        string text = "";
        foreach (var header in headers)
        {
            string valueText = "";
            foreach (string thing in header.Value)
            {
                valueText += $"{thing}";
            }
            text += $"{header.Key} = {valueText}{separator}";
        }

        return text;
    }

    internal static string GetAllAttributeValuesText(object obj, string separator= ", ")
    {
        string text = "";
        foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
        {
            text += $"{propertyInfo.Name} = {propertyInfo.GetValue(propertyInfo)}";
        }

        return text;
    }
    
    // shamelessly stolen https://stackoverflow.com/questions/249760/how-can-i-convert-a-unix-timestamp-to-datetime-and-vice-versa
    internal static DateTime UnixTimeStampToDateTime( double unixTimeStamp )
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        return dateTime;
    }
    
    // shamelessly stolen https://stackoverflow.com/questions/1749966/c-sharp-how-to-determine-whether-a-type-is-a-number
    internal static bool IsNumericType(this object o)
    {   
        switch (Type.GetTypeCode(o.GetType()))
        {
            case TypeCode.Byte:
            case TypeCode.SByte:
            case TypeCode.UInt16:
            case TypeCode.UInt32:
            case TypeCode.UInt64:
            case TypeCode.Int16:
            case TypeCode.Int32:
            case TypeCode.Int64:
            case TypeCode.Decimal:
            case TypeCode.Double:
            case TypeCode.Single:
                return true;
            default:
                return false;
        }
    }
}