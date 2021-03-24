using System;
using UnityEngine;
using UnityEngine.UI;

public class CannonUICard : MonoBehaviour
{
    public event Action<int> onCustomize;
    public event Action<int, GameObject> onDelete;
    
    [SerializeField] private Image cannonImage;
    [SerializeField] private Button customizeButton;
    [SerializeField] private Button deleteButton;
    [SerializeField] private Sprite defaultSprite;
    
    private int cannonID;
    private int cardId;

    private void Start()
    {
        customizeButton.onClick.AddListener(() => onCustomize?.Invoke(cardId));
        deleteButton.onClick.AddListener(() => onDelete?.Invoke(cannonID, gameObject));
    }

    private void OnDestroy()
    {
        customizeButton.onClick.RemoveListener(() => onCustomize?.Invoke(cardId));
        deleteButton.onClick.RemoveListener(() => onDelete?.Invoke(cannonID, gameObject));
    }

    public void InitializeCard(CannonInfo info, int cardID)
    {
        cardId = cardID;
        cannonID = info.cannonId;
        var image = FileUtilities.LoadPNG(info.imagePath, (int)cannonImage.preferredWidth, (int)cannonImage.preferredHeight);
        if (image != null)
        {
            var sprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height), Vector2.zero, 100);
            cannonImage.sprite = sprite;
        }
        else
        {
            cannonImage.sprite = defaultSprite;
        }
    }
}
