//dotnet add package Microsoft.AspNetCore.Session
//dotnet add package NewtonSoft.Json

using Newtonsoft.Json;

namespace Info360.Models;

public static class Objeto
{
    public static string ObjectToString<T>(T? obj)
    {
        return JsonConvert.SerializeObject(obj);
    }
    public static T? StringToObject<T>(string txt)
    {
        if (string.IsNullOrEmpty(txt))
            return default;
        else
            return JsonConvert.DeserializeObject<T>(txt);
    }
}