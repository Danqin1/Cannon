using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    #region variables

    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject cannonsPanel;
    [SerializeField] private Button newCannon;
    [SerializeField] private Button loadCannons;
    [SerializeField] private GameObject cannonCardsContainer;
    [SerializeField] private CannonUICard cardPrefab;
    [SerializeField] private Button exit;

    private readonly List<CannonUICard> cannonUICards = new List<CannonUICard>();

    #endregion

    #region unity methods

    private void Start()
    {
        newCannon.onClick.AddListener(StartWithNewCannon);
        loadCannons.onClick.AddListener(GoToCannonsPanel);
        exit.onClick.AddListener(() => Application.Quit());
        LoadCards();
        
        cannonUICards.ForEach(x=> x.onCustomize += CustomizeExistingCannon);
        cannonUICards.ForEach(x=> x.onDelete += DeleteCannon);

        loadCannons.interactable = ApplicationController.CannonsMemento.cannons.Count > 0;
    }

    private void OnDestroy()
    {
        newCannon.onClick.RemoveListener(StartWithNewCannon);
        loadCannons.onClick.RemoveListener(GoToCannonsPanel);
        exit.onClick.RemoveListener(() => Application.Quit());
        
        cannonUICards.ForEach(x=> x.onCustomize -= CustomizeExistingCannon);
        cannonUICards.ForEach(x=> x.onDelete -= DeleteCannon);
        
        SaveSystem.Save();
    }
    
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            if (cannonsPanel.activeSelf)
            {
                GoToMainPanel();
            }
        }        
    }
    
    #endregion

    #region private methods

    private void GoToCannonsPanel()
    {
        mainPanel.SetActive(false);
        cannonsPanel.SetActive(true);
    }

    private void GoToMainPanel()
    {
        cannonsPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    private void StartWithNewCannon()
    {
        ApplicationController.ChosenNewCannon = true;
        ApplicationController.LoadGameplay();
    }

    private void CustomizeExistingCannon(int id)
    {
        ApplicationController.ChosenNewCannon = false;
        ApplicationController.ChosenCannonIndex = id;
        ApplicationController.LoadGameplay();
    }

    private void LoadCards()
    {
        int cardID = 0;
        foreach (var cannonInfo in ApplicationController.CannonsMemento.cannons)
        {
            var spawnedCard = Instantiate(cardPrefab, cannonCardsContainer.transform);
            spawnedCard.InitializeCard(cannonInfo, cardID);
            cardID++;
            cannonUICards.Add(spawnedCard);
        }
    }

    private void DeleteCannon(int id, CannonUICard card)
    {
        var cannon = ApplicationController.CannonsMemento.cannons.Find(x => x.cannonId == id);
        if (cannon != null)
        {
            if (File.Exists(cannon.imagePath))
            {
                File.Delete(cannon.imagePath);
            }

            cannonUICards.Remove(card);
            Destroy(card.gameObject);
            ApplicationController.CannonsMemento.cannons.Remove(cannon);
        }

        UpdateCardsIndexes();
    }

    private void UpdateCardsIndexes()
    {
        int index = 0;
        cannonUICards.ForEach(x =>
        {
            x.CardId = index;
            index++;
        });
    }
    
    #endregion
}
