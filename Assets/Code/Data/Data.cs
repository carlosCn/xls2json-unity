using LitJson;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

public class Data
{
	public readonly string key;
	
	private static Dictionary<Type, Dictionary<string, Data>> managedData;
	
	public static T[] CreateFromJsonArray<T>(string json)
	{
		return null;
	}
	
	public static bool Load(string dataPath)
	{ 
		managedData = new Dictionary<Type, Dictionary<string, Data>>();
       		
		BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Static;
		MethodInfo toObjectStatic = typeof(Data).GetMethod("JsonMapperToObjectWrapper", bindingFlags);
		
		List<Type> dataTypes = Types.FindAllDerivedTypes<Data>();
		foreach(Type t in dataTypes)
		{
			// get or create the data dictionary for this type
			Dictionary<string, Data> dataDict;
			if(!managedData.TryGetValue(t, out dataDict))
			{
				dataDict = new Dictionary<string, Data>();
				managedData.Add(t, dataDict);
			}
				
			string path = dataPath + t.Name + ".json";
			using(StreamReader sr = new StreamReader(path))
			{
				// load json data from the file
				string json = sr.ReadToEnd();
				
				// use reflection to get create the load method
				// i bet this will piss off iOS somehow
				MethodInfo toObjectGeneric = toObjectStatic.MakeGenericMethod(t);
				Data[] data = toObjectGeneric.Invoke(null, new[] { json }) as Data[];
				
				// add to the dict
				for(int n = 0; n < data.Length; n++)
				{
					dataDict.Add(data[n].key, data[n]);
				}
			}
		}
		
		return false;
	}
		
	public static Dictionary<string, Data> Get<T>()
	{
		Dictionary<string, Data> dataDict;
		if(managedData.TryGetValue(typeof(T), out dataDict))
		{
			return dataDict;
		}
		return null;
	}
	
	public static T Get<T>(string key) where T : Data
	{
		Dictionary<string, Data> dataDict;
		if(managedData.TryGetValue(typeof(T), out dataDict))
		{
			if(dataDict.ContainsKey(key))
			{
				return dataDict[key] as T;
			}
		}
		return null;
	}
	
	public static int ManagedTypeCount
	{
		get
		{
			if(managedData != null)
			{
				return managedData.Count;
			}
			return 0;
		}
	}
	
	// wrapper method around JsonMapper.ToObject to disambiguate
	static T[] JsonMapperToObjectWrapper<T>(string json) where T : Data
	{
		return JsonMapper.ToObject<T[]>(json);
	}
}
