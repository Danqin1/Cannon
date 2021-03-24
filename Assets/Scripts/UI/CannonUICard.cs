using System;
using UnityEngine;
using UnityEngine.UI;

public class CannonUICard : MonoBehaviour
{
    public event Action<int> onCustomize;
    public event Action<int, CannonUICard> onDelete;
    
    [SerializeField] private Image cannonImage;
    [SerializeField] private Button customizeButton;
    [SerializeField] private Button deleteButton;
    [SerializeField] private Sprite defaultSprite;
    
    private int cannonID;
    
    public int CardId { get; set; }

    private void Start()
    {
        customizeButton.onClick.AddListener(() => onCustomize?.Invoke(CardId));
        deleteButton.onClick.AddListener(() => onDelete?.Invoke(cannonID, this));
    }

    private void OnDestroy()
    {
        customizeButton.onClick.RemoveListener(() => onCustomize?.Invoke(CardId));
        deleteButton.onClick.RemoveListener(() => onDelete?.Invoke(cannonID, this));
    }

    public void InitializeCard(CannonInfo info, int cardID)
    {
        CardId = cardID;
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
