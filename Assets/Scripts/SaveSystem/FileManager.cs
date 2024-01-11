using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class FileManager
{
    public static List<string> GetAllSaveFiles()
    {
        var files = new List<string>();
        var directoryInfo = new DirectoryInfo(Application.persistentDataPath);
        var fileInfo = directoryInfo.GetFiles();
        foreach (var file in fileInfo)
        {
            if (file.Extension == ".json")
            {
                files.Add(file.Name.TrimEnd(".json".ToCharArray()));
            }
        }

        return files;
    }

	public static bool WriteToFile(string fileName, string fileContents)
	{
		var fullPath = Path.Combine(Application.persistentDataPath, fileName);

		try
		{
			File.WriteAllText(fullPath, fileContents);
			return true;
		}
		catch (Exception e)
		{
			Debug.LogError($"Failed to write to {fullPath} with exception {e}");
			return false;
		}
	}

	public static bool LoadFromFile(string fileName, out string result)
	{
		var fullPath = Path.Combine(Application.persistentDataPath, fileName);
		if(!File.Exists(fullPath))
		{
			File.WriteAllText(fullPath, ""); 
		}
		try
		{
			result = File.ReadAllText(fullPath);
			return true;
		}
		catch (Exception e)
		{
			Debug.LogError($"Failed to read from {fullPath} with exception {e}");
			result = "";
			return false;
		}
	}

	public static bool MoveFile(string fileName, string newFileName)
	{
		var fullPath = Path.Combine(Application.persistentDataPath, fileName);
		var newFullPath = Path.Combine(Application.persistentDataPath, newFileName);

		try
		{
			if (File.Exists(newFullPath))
			{
				File.Delete(newFullPath);
			}

			if (!File.Exists(fullPath))
			{
				return false;
			}
			
			File.Move(fullPath, newFullPath);
		}
		catch (Exception e)
		{
			Debug.LogError($"Failed to move file from {fullPath} to {newFullPath} with exception {e}");
			return false;
		}

		return true;
	}

    internal static string LoadFromFile(string saveFilename)
    {
        throw new NotImplementedException();
    }
}
