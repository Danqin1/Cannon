using System.Collections.Generic;

public static class ApplicationController
{
	static ApplicationController()
	{
		CannonsMemento = SaveSystem.Restore();
		if (CannonsMemento.Equals(default) || CannonsMemento.cannons == null)
		{
			CannonsMemento = new CannonsMemento {cannons = new List<CannonInfo>()};
		}
	}
	
	public static bool ChosenNewCannon { get; set; } = true;
	public static int ChosenCannonIndex { get; set; }
	public static CannonsMemento CannonsMemento { get; set; } 
}