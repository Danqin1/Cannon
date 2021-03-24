using System.IO;
using UnityEngine;

public static class FileUtilities
{
	public static Texture2D LoadPNG(string filePath, int width, int height)
	{
		Texture2D tex = null;
		
		if (File.Exists(filePath))
		{
			byte[] fileData = File.ReadAllBytes(filePath);
			tex = new Texture2D(width, height);
			tex.LoadImage(fileData);
		}

		return tex;
	}
}