using System.Collections.Generic;
using UnityEngine.SceneManagement;

public static class ApplicationController
{
	static ApplicationController()
	{
		CannonsMemento = SaveSystem.Restore() ?? new CannonsMemento {cannons = new List<CannonInfo>()};
	}
	
	public static bool ChosenNewCannon { get; set; } = true;
	public static int ChosenCannonIndex { get; set; }
	public static CannonsMemento CannonsMemento { get; }

	public static void LoadGameplay()
	{
		SceneManager.LoadScene("Gameplay");
	}

	public static void LoadMainMenu()
	{
		SceneManager.LoadScene("MainMenu");
	}
}