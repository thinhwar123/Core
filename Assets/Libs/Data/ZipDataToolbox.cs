using FullSerializer;
using Giab.Common.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZipDataToolbox
{
    /// <summary>
    /// Load Obj from json data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    public static T LoadZipJsonData<T>(string data) where T : class
    {
        if (string.IsNullOrEmpty(data))
        {
            return default;
        }

        string decodeData = StringToolbox.Base64ZipDecodeString(data);
        var parseData = fsJsonParser.Parse(decodeData);
        object deserialized = null;
        var serializer = new fsSerializer();
        serializer.TryDeserialize(parseData, typeof(T), ref deserialized).AssertSuccessWithoutWarnings();

        return deserialized as T;
    }


    public static object LoadZipJsonData(string keyType, string data)
    {
        if (string.IsNullOrEmpty(data))
        {
            return default;
        }
        try
        {
            string decodeData = StringToolbox.Base64ZipDecodeString(data);
            var parseData = fsJsonParser.Parse(decodeData);
            object deserialized = null;
            var serializer = new fsSerializer();
            serializer.TryDeserialize(parseData, Type.GetType(keyType), ref deserialized).AssertSuccessWithoutWarnings();

            return deserialized;
        }
        catch (Exception)
        {

        }

        return null;
    }


    public static List<T> LoadZipJsonData<T>(List<T> _list, string data)
    {
        if (string.IsNullOrEmpty(data))
        {
            return default;
        }
        try
        {
            string decodeData = StringToolbox.Base64ZipDecodeString(data);
            var parseData = fsJsonParser.Parse(decodeData);

            object deserialized = null;
            var serializer = new fsSerializer();
            serializer.TryDeserialize(parseData, typeof(T), ref deserialized).AssertSuccessWithoutWarnings();

            _list.Add((T)deserialized);


        }
        catch (Exception)
        {

        }

        return _list;
    }


    /// <summary>
    /// Get json data from obj.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    public static string GetZipJsonData<T>(T data) where T : class
    {
        try
        {
            fsData serializedData;
            var serializer = new fsSerializer();
            serializer.TrySerialize(data, out serializedData).AssertSuccessWithoutWarnings();

            var json = fsJsonPrinter.PrettyJson(serializedData);

            json = StringToolbox.Base64ZipEncodeString(json);

            return json;
        }
        catch (Exception)
        {

        }

        return "";
    }
}
