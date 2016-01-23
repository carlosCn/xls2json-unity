using UnityEditor;
using System.Diagnostics;

public class DataImporterEditor : EditorWindow
{
	
	internal class Keys
	{
		internal static readonly string SourceDirectory = "DataImporter_SourceDirectory";
		internal static readonly string DataDirectory = "DataImporter_DataDirectory";
		internal static readonly string CodeDirectory = "DataImporter_CodeDirectory";
		internal static readonly string Xls2Json = "DataImporter_Xls2Json";
		internal static readonly string RebuildAll = "DataImporter_RebuildAll";
		internal static readonly string Verbose = "DataImporter_Verbose";
        internal static readonly string OverwriteReadonly = "DataImporter_OverwriteReadonly";
	}

	internal class Defaults
	{
		internal static readonly string SourceDirectory = "Assets/Data";
		internal static readonly string DataDirectory = "Assets/StreamingAssets/Data";
		internal static readonly string CodeDirectory = "Assets/Code/Data";
		internal static readonly string Xls2Json = "Tools/xls2json/xls2json.py";
		internal static readonly bool RebuildAll = true;
		internal static readonly bool Verbose = true;
        internal static readonly bool OverwriteReadonly = true;
	}
	
	public static string SourceDirectory
	{
		get
		{
			return EditorPrefs.GetString(Keys.SourceDirectory, Defaults.SourceDirectory);
		}
	}
	
	public static string DataDirectory
	{
		get
		{
			return EditorPrefs.GetString(Keys.DataDirectory, Defaults.DataDirectory);
		}
	}
	
	public static string CodeDirectory
	{
		get
		{
			return EditorPrefs.GetString(Keys.CodeDirectory, Defaults.CodeDirectory);
		}
	}
	
	public static string Xls2Json
	{
		get
		{
			return EditorPrefs.GetString(Keys.Xls2Json, Defaults.Xls2Json);
		}
	}
	
	public static bool RebuildAll
	{
		get
		{
			return EditorPrefs.GetBool(Keys.RebuildAll, Defaults.RebuildAll);
		}
	}
	
	public static bool Verbose
	{
		get
		{
			return EditorPrefs.GetBool(Keys.Verbose, Defaults.Verbose);
		}
	}
    
    public static bool OverwriteReadonly
	{
		get
		{
			return EditorPrefs.GetBool(Keys.OverwriteReadonly, Defaults.OverwriteReadonly);
		}
	}
	
	public static string Command
	{
		get
		{
			string cmd = Xls2Json;
			cmd += " --source-dir " + SourceDirectory;
			cmd += " --data-dir " + DataDirectory;
			cmd += " --code-dir " + CodeDirectory;
			if(RebuildAll)
			{
				cmd += " --rebuild-all ";
			}
			if(Verbose)
			{
				cmd += " --verbose ";
			}
            if(OverwriteReadonly)
			{
				cmd += " --overwrite-readonly ";
			}
			return cmd;
		}
	}
	
    [MenuItem("Project/Data/Import Data")]
    public static void Import()
    {
        if(Verbose)
        {
            UnityEngine.Debug.Log(Command);
        }
        
        string result = Execute("python", Command);
        
        if(Verbose)
        {
            UnityEngine.Debug.Log(result);
        }
    }
	
	[MenuItem("Project/Data/Data Importer Settings")]
	static void DataImporterSettings()
	{
		DataImporterEditor window = (DataImporterEditor)EditorWindow.GetWindow (typeof (DataImporterEditor));
		window.Show();
	}
	
	void OnGUI ()
	{
		PreferenceTextField("Source Directory", Keys.SourceDirectory, Defaults.SourceDirectory);
		PreferenceTextField("Data Directory", Keys.DataDirectory, Defaults.DataDirectory);
		PreferenceTextField("Code Directory", Keys.CodeDirectory, Defaults.CodeDirectory);
		PreferenceTextField("XLS to JSON Converter", Keys.Xls2Json, Defaults.Xls2Json);
		PreferenceToggleField("Overwrite Read-Only Files", Keys.OverwriteReadonly, Defaults.OverwriteReadonly);
        PreferenceToggleField("Rebuild All", Keys.RebuildAll, Defaults.RebuildAll);
		PreferenceToggleField("Verbose", Keys.Verbose, Defaults.Verbose);
	}
	
	void PreferenceTextField(string label, string key, string defaultValue)
	{
		string oldText = EditorPrefs.GetString(key, defaultValue);
		string newText = EditorGUILayout.TextField(label, oldText);
		if(newText != oldText)
		{
			EditorPrefs.SetString(key, newText);
		}
	}
	
	void PreferenceToggleField(string label, string key, bool defaultValue)
	{
		bool oldState = EditorPrefs.GetBool(key, defaultValue);
		bool newState = EditorGUILayout.Toggle(label, oldState);
		if(newState != oldState)
		{
			EditorPrefs.SetBool(key, newState);
		}
	}
    
    static string Execute(string cmd, string args)
	{
		Process p = new Process();
        p.StartInfo.RedirectStandardOutput = true;
        p.StartInfo.RedirectStandardError = true;
		p.StartInfo.FileName = cmd;
		p.StartInfo.Arguments = args;
		p.StartInfo.WorkingDirectory = System.IO.Directory.GetCurrentDirectory();
		p.StartInfo.CreateNoWindow = true;
		p.StartInfo.UseShellExecute = false;		
		p.Start();
		p.WaitForExit();
        if(p.ExitCode == 0)
        {
            return p.StandardOutput.ReadToEnd();
        }
        else
        {
            return p.StandardError.ReadToEnd();
        }
	}
}