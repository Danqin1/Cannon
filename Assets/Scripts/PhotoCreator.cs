using System.Collections;
using System.IO;
using UnityEngine;

public class PhotoCreator : MonoBehaviour
{
	[SerializeField] private Camera mainCam;
	[SerializeField] private HUD hud;
	[SerializeField] private ApplicationSettings applicationSettings;
	[SerializeField] private LevelController levelController;
	[SerializeField] private WorldInput worldInput;

	private void Start()
	{
		hud.onTakePhoto += OnTakePhoto;
		worldInput.onTakePhoto += OnTakePhoto;
	}

	private void OnDestroy()
	{
		hud.onTakePhoto -= OnTakePhoto;
		worldInput.onTakePhoto -= OnTakePhoto;
	}

	private void OnTakePhoto()
	{
		StartCoroutine(TakePhoto());
	}

	private IEnumerator TakePhoto()
	{
		hud.gameObject.SetActive(false);
		yield return new WaitForEndOfFrame();

		mainCam.targetTexture =
			RenderTexture.GetTemporary(applicationSettings.PhotoWidth, applicationSettings.PhotoHeight, 16);
		
		var renderTex = mainCam.targetTexture;
		var renderResult = new Texture2D(renderTex.width, renderTex.height, TextureFormat.ARGB32, false);
		var rect = new Rect(0, 0, renderTex.width, renderTex.height);
		renderResult.ReadPixels(rect, 0, 0);
		renderResult.Apply();

		byte[] bytes = renderResult.EncodeToPNG();
		string path = Application.dataPath + $"/Cannon{levelController.CurrentCannonId}.png";
		File.WriteAllBytes(path, bytes);

		RenderTexture.ReleaseTemporary(renderTex);
		mainCam.targetTexture = null;

		var cannonInfo =
			ApplicationController.CannonsMemento.cannons.Find(x => x.cannonId == levelController.CurrentCannonId);
		if (cannonInfo != null)
		{
			cannonInfo.imagePath = path;
		}
		levelController.CurrentImagePath = path;
		levelController.UpdateCannonsInfo();
		SaveSystem.Save();
		
		hud.gameObject.SetActive(true);
	}
}