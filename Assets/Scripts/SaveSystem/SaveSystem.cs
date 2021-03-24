using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class CannonsMemento
{
	public int saveVersion;
	public List<CannonInfo> cannons;
}

[System.Serializable]
public class CannonInfo
{
	public int cannonId;
	public int barrelIndex;
	public int standIndex;
	public int wheelsIndex;
	public string imagePath;
	public List<Color> partsColors;
}

public static class SaveSystem
{
	private static readonly string saveName = "CannonsSave";
	private static readonly string path = Application.dataPath + $"/{saveName}.txt";
	private static readonly int saveVersion = 1;
	
	public static void Save()
	{
		var cannonsMemento = ApplicationController.CannonsMemento;
		cannonsMemento.saveVersion = saveVersion;
		string json = JsonUtility.ToJson(cannonsMemento);
		File.WriteAllText(path, json);
	}

	public static CannonsMemento Restore()
	{
		if (File.Exists(path))
		{
			string savedString = File.ReadAllText(path);
			var memento = JsonUtility.FromJson<CannonsMemento>(savedString);
			if (memento.saveVersion != saveVersion)
			{
				File.Delete(path);
				return default;
			}

			return memento;
		}

		return default;
	}
}