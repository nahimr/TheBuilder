using UnityEngine;

public class JsonHelper
{
    //Usage:
    //YouObject[] objects = JsonHelper.getJsonArray<YouObject> (jsonString);
    public static T[] GetJsonArray<T>(string json)
    {
        //var newJson = "{ \"array\": " + json + "}";
        var wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.array;
    }

    //Usage:
    //string jsonString = JsonHelper.arrayToJson<YouObject>(objects);
    public static string ArrayToJson<T>(T[] array)
    {
        var wrapper = new Wrapper<T> {array = array};
        return JsonUtility.ToJson(wrapper);
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }
}
