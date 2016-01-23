using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class DataTest
{
	[MenuItem("Project/Data/Test Data Loading")]
	public static void LoadFromJson()
	{
		string path = Application.streamingAssetsPath + "/Data/";
		Data.Load(path);
		
		Dictionary<string, Data> items = Data.Get<SampleData>();
		foreach(Data data in items.Values)
		{
			SampleData sampleData = data as SampleData;
			Debug.Log("key: " + sampleData.key);
			Debug.Log("name: " + sampleData.name);
			Debug.Log("price: " + sampleData.price);
			Debug.Log("health: " + sampleData.health);
			Debug.Log("fancy: " + sampleData.fancy);
			Debug.Log("maxHealth: " + sampleData.maxHealth);
		}
	}
}