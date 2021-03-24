using System;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public event Action onGenerateNewCannon;
    public event Action onChangeWheel;
    public event Action onChangeBarrel;
    public event Action onChangeStand;
    public event Action onTakePhoto;
    public event Action onChangeRandomColor;
    
    [SerializeField] private Button generateNewCannon;
    [SerializeField] private Button changeBarrel;
    [SerializeField] private Button changeStand;
    [SerializeField] private Button changeWheel;
    [SerializeField] private Button takePhoto;
    [SerializeField] private Button randomColor;

    private void Start()
    {
        generateNewCannon.onClick.AddListener(() => onGenerateNewCannon?.Invoke());
        changeBarrel.onClick.AddListener(() => onChangeBarrel?.Invoke());
        changeStand.onClick.AddListener(() => onChangeStand?.Invoke());
        changeWheel.onClick.AddListener(() => onChangeWheel?.Invoke());
        takePhoto.onClick.AddListener(() => onTakePhoto?.Invoke());
        randomColor.onClick.AddListener(() => onChangeRandomColor?.Invoke());
    }

    private void OnDestroy()
    {
        generateNewCannon.onClick.RemoveListener(() => onGenerateNewCannon?.Invoke());
        changeBarrel.onClick.RemoveListener(() => onChangeBarrel?.Invoke());
        changeStand.onClick.RemoveListener(() => onChangeStand?.Invoke());
        changeWheel.onClick.RemoveListener(() => onChangeWheel?.Invoke());
        takePhoto.onClick.RemoveListener(() => onTakePhoto?.Invoke());
        randomColor.onClick.RemoveListener(() => onChangeRandomColor?.Invoke());
    }
}
