using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

class DataImporterPostprocessor : AssetPostprocessor 
{
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) 
	{
		string sourceDir = DataImporterEditor.SourceDirectory;
		List<string> modifiedData = new List<string>();
		
		foreach (var str in importedAssets)
		{
			if(str.Contains(sourceDir))
			{
				modifiedData.Add(str);
			}
		}
		
		foreach (var str in deletedAssets) 
		{
			if(str.Contains(sourceDir))
			{
				modifiedData.Add(str);
			}
		}

		for (var i = 0; i < movedAssets.Length; i++)
		{
			if(movedAssets[i].Contains(sourceDir))
			{
				modifiedData.Add(movedAssets[i]);
			}
		}
		
		if(modifiedData.Count > 0)
		{
            DataImporterEditor.Import();
			AssetDatabase.Refresh();
		}
	}
}