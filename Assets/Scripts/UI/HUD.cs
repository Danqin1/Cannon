using System;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public event Action onGenerateNewCannon;
    public event Action onChangeWheel;
    public event Action onChangeBarrel;
    public event Action onChangeStand;
    
    [SerializeField] private Button generateNewCannon;
    [SerializeField] private Button changeBarrel;
    [SerializeField] private Button changeStand;
    [SerializeField] private Button changeWheel;

    private void Start()
    {
        generateNewCannon.onClick.AddListener(() => onGenerateNewCannon?.Invoke());
        changeBarrel.onClick.AddListener(() => onChangeBarrel?.Invoke());
        changeStand.onClick.AddListener(() => onChangeStand?.Invoke());
        changeWheel.onClick.AddListener(() => onChangeWheel?.Invoke());
    }

    private void OnDestroy()
    {
        generateNewCannon.onClick.RemoveListener(() => onGenerateNewCannon?.Invoke());
        changeBarrel.onClick.RemoveListener(() => onChangeBarrel?.Invoke());
        changeStand.onClick.RemoveListener(() => onChangeStand?.Invoke());
        changeWheel.onClick.RemoveListener(() => onChangeWheel?.Invoke());
    }
}
