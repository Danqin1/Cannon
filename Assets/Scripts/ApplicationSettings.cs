using UnityEngine;

[CreateAssetMenu(fileName = "ApplicationSettings", menuName = "Koffeecup/Settings/ApplicationSettings", order = 0)]
public class ApplicationSettings : ScriptableObject
{
	[SerializeField] private int photoWidth;
	[SerializeField] private int photoHeight;

	public int PhotoWidth => photoWidth;
	public int PhotoHeight => photoHeight;
}