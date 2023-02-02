using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Dal;

static class XMLTools
{
    const string s_dir = @"..\xml\";

    //ממצגת 10 של אפרת
    static XMLTools()
    {
        if(!Directory.Exists(s_dir)) 
            Directory.CreateDirectory(s_dir);
    }

    #region SaveLoadWithXElement

    /// <summary>
    /// recives an XElement root that includes a list of IEnumerable<XElement>, and saves it in XML file
    /// </summary>
    /// <param name="rootElem"></param>
    /// <param name="entity"></param>
    /// <exception cref="Exception"></exception>
    public static void SaveListToXMLElement(XElement rootElem, string entity) 
    {
        string filePath = $"{s_dir + entity}.xml";
        try
        {
            rootElem.Save(filePath);
        }
        catch (Exception ex)
        {
            throw new Exception($"Fail to create XML file: {filePath}", ex);
        }
    }

    /// <summary>
    /// Loads an XML file into XElement root thet includes a list of IEnumerable<XElement>
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static XElement LoadListFromXMLElement(string entity) 
    {
        string filePath = $"{s_dir + entity}.xml";
        try
        {
            if(File.Exists(filePath))
                return XElement.Load(filePath);
            XElement rootElem = new(entity);
            rootElem.Save(filePath);
            return rootElem;
        }
        catch (Exception ex)
        {
            throw new Exception($"Fail to load XML file: {filePath}", ex);
        }
    }

    #endregion

    #region SaveLoadWithXMLSerializer
    /// <summary>
    /// Automatic saving of generic list in XML file with XmlSerializer
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="entity"></param>
    /// <exception cref="Exception"></exception>
    public static void SaveListToXMLSerializer<T>(List<T?> list, string entity) where T : struct
    {
        string filePath = $"{s_dir + entity}.xml";
        try
        {
            using FileStream file = new(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
            XmlSerializer serializer = new(typeof(List<T?>));
            serializer.Serialize(file, list);
        }
        catch (Exception ex)
        {
            throw new Exception($"Fail to create xml file: {filePath}", ex);
        }
    }

    /// <summary>
    /// Automatic load of XML file to a generic list with XmlSerializer class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entity"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static List<T?> LoadListFromXMLSerializer<T>(string entity) where T : struct
    {
        string filePath = $"{s_dir + entity}.xml";
        try
        {
            if (!File.Exists(filePath))
                return new();
            using FileStream file = new(filePath, FileMode.Open);
            XmlSerializer x = new(typeof(List<T?>));
            return x.Deserialize(file) as List<T?> ?? new();
        }
        catch (Exception ex)
        {
            throw new Exception($"Fail to load xml file: {filePath}", ex);
        }
    }
    #endregion

    #region Extension Fuctions
    public static T? ToEnumNullable<T>(this XElement element, string name) where T : struct, Enum =>
        Enum.TryParse<T>((string?)element.Element(name), out var result) ? (T?)result : null;

    public static DateTime? ToDateTimeNullable(this XElement element, string name) =>
        DateTime.TryParse((string?)element.Element(name), out var result) ? (DateTime?)result : null;

    public static double? ToDoubleNullable(this XElement element, string name) =>
        double.TryParse((string?)element.Element(name), out var result) ? (double?)result : null;

    public static int? ToIntNullable(this XElement element, string name) =>
        int.TryParse((string?)element.Element(name), out var result) ? (int?)result : null;
    #endregion
}
