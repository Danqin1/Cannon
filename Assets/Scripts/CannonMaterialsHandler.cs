using System.Collections.Generic;
using UnityEngine;

public class CannonMaterialsHandler
{
	private List<CannonPart> cannonParts;

	public CannonMaterialsHandler(List<CannonPart> cannonParts)
	{
		this.cannonParts = cannonParts;
	}

	public void UpdateParts(List<CannonPart> parts)
	{
		cannonParts.Clear();
		cannonParts.AddRange(parts);
	}

	public void ChangeColorToRandom()
	{
		cannonParts.ForEach(x =>
		{
			Color randomColor = new Color(
				Random.Range(0f, 1f), 
				Random.Range(0f, 1f), 
				Random.Range(0f, 1f),
				1f
			);

			x.Mesh.material.color = randomColor;
		});
	}

	public void RestoreColors(List<Color> colors)
	{
		for (int i = 0; i < cannonParts.Count; i++)
		{
			cannonParts[i].Mesh.material.color = colors[i];
		}
	}
}