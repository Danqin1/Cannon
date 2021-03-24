using System.Collections.Generic;
using UnityEngine;
using UnityTemplateProjects;

public class LevelController : MonoBehaviour
{
    #region variables
    
    [SerializeField] private CannonPartsDatabase database;
    [SerializeField] private HUD hud;

    private CannonMaterialsHandler cannonMaterialsHandler;
    private CannonPart spawnedBarrel;
    private CannonPart spawnedWheel1;
    private CannonPart spawnedWheel2;
    private CannonPart spawnedStand;

    private int id;
    private int currentBarrelIndex = -1;
    private int currentWheelsIndex = -1;
    private int currentStandIndex = -1;
    
    #endregion

    #region properties

    public int CurrentCannonId => id;
    public string CurrentImagePath { get; set; }

    #endregion

    #region unity methods

    private void Start()
    {
        CannonInfo info = null;
        cannonMaterialsHandler = new CannonMaterialsHandler(new List<CannonPart>());

        if (ApplicationController.ChosenNewCannon)
        {
            SpawnRandomCannon();
        }
        else if (ApplicationController.CannonsMemento != null 
                 && ApplicationController.CannonsMemento.cannons.Count > ApplicationController.ChosenCannonIndex)
        {
            info = ApplicationController.CannonsMemento.cannons[ApplicationController.ChosenCannonIndex];
            RestoreCannon(info);
        }
        else
        {
            SpawnRandomCannon();
        }
        
        UpdateCannonMaterialsHandler(false);
        if (info != null)
        {
            cannonMaterialsHandler.RestoreColors(info.partsColors);
        }

        SubscribeToEvents();
        UpdateCannonsInfo();
    }

    private void OnDestroy()
    {
        SaveSystem.Save();
        DisposeFromEvents();
    }

    #endregion

    #region public methods

    public void UpdateCannonsInfo(bool overwriteColors = true)
    {
        var cannon = ApplicationController.CannonsMemento.cannons.Find(x => x.cannonId == id);
        
        if (cannon != null)
        {
            cannon.barrelIndex = currentBarrelIndex;
            cannon.standIndex = currentStandIndex;
            cannon.wheelsIndex = currentWheelsIndex;
            if (CurrentImagePath != null)
            {
                cannon.imagePath = CurrentImagePath;
            }

            if (overwriteColors)
            {
                cannon.partsColors = new List<Color>()
                {
                    spawnedBarrel.Mesh.material.color,
                    spawnedStand.Mesh.material.color,
                    spawnedWheel1.Mesh.material.color,
                    spawnedWheel2.Mesh.material.color
                };
            }
        }
        else
        {
            ApplicationController.CannonsMemento.cannons.Add(new CannonInfo()
            {
                cannonId = id,
                barrelIndex = currentBarrelIndex,
                standIndex = currentStandIndex,
                wheelsIndex = currentWheelsIndex,
                imagePath = CurrentImagePath,
                partsColors = new List<Color>()
                {
                    spawnedBarrel.Mesh.material.color,
                    spawnedStand.Mesh.material.color,
                    spawnedWheel1.Mesh.material.color,
                    spawnedWheel2.Mesh.material.color
                }
            });
        }
    }

    #endregion

    #region private methods
    
    private void SubscribeToEvents()
    {
        hud.onGenerateNewCannon += OnGenerateNewCannon;
        hud.onChangeBarrel += OnBarrelChanged;
        hud.onChangeStand += OnChangeStand;
        hud.onChangeWheel += OnChangeWheels;
        hud.onChangeRandomColor += OnColorChange;
    }

    private void DisposeFromEvents()
    {
        hud.onGenerateNewCannon += OnBarrelChanged;
        hud.onChangeBarrel += OnBarrelChanged;
        hud.onChangeStand += OnBarrelChanged;
        hud.onChangeWheel += OnBarrelChanged;
        hud.onChangeRandomColor -= OnColorChange;
    }

    private void RestoreCannon(CannonInfo infos)
    {
        int restoredWheels = 0;
        spawnedBarrel = Instantiate(database.Barrels[infos.barrelIndex]);
        id = infos.cannonId;
        CannonPart part = spawnedBarrel;
        
        spawnedBarrel.Sockets.ForEach(x =>
            {
                switch (x.Type)
                {
                    case CannonPartType.Wheel:
                        part = Instantiate(database.Wheels[infos.wheelsIndex]);

                        if (restoredWheels == 0)
                        {
                            spawnedWheel1 = part;
                        }
                        else
                        {
                            spawnedWheel2 = part;
                        }
                        restoredWheels++;
                        break;
                    case CannonPartType.Stand:
                        part = Instantiate(database.Stands[infos.standIndex]);
                        spawnedStand = part;
                        break;
                }

                part.transform.position = x.transform.position;
                part.transform.rotation = x.transform.rotation;
            });
        currentBarrelIndex = infos.barrelIndex;
        currentWheelsIndex = infos.wheelsIndex;
        currentStandIndex = infos.standIndex;
    }
    
    private void OnChangeWheels()
    {
       DestroyWheels();
       int spawnedWheelsCount = 0;
       
       spawnedBarrel.Sockets.FindAll(x => x.Type == CannonPartType.Wheel)
           .ForEach(w =>
           {
               currentWheelsIndex = spawnedWheelsCount == 0 ? (currentWheelsIndex + 1) % database.Wheels.Count : currentWheelsIndex;
               
               var part = Instantiate(database.Wheels[currentWheelsIndex], w.transform.position, w.transform.rotation);
               
               if (spawnedWheelsCount == 0)
               {
                   spawnedWheel1 = part;
               }
               else
               {
                   spawnedWheel2 = part;
               }
               spawnedWheelsCount++;
           });
       UpdateCannonMaterialsHandler();
    }

    private void OnChangeStand()
    {
        Destroy(spawnedStand.gameObject);
        
        spawnedBarrel.Sockets.FindAll(x => x.Type == CannonPartType.Stand)
            .ForEach(w =>
            {
                currentStandIndex = (currentStandIndex + 1) % database.Stands.Count;
                spawnedStand = Instantiate(database.Stands[currentStandIndex], w.transform.position, w.transform.rotation);
            });
        UpdateCannonMaterialsHandler();
    }

    private void OnBarrelChanged()
    {
        int doneWheels = 0;
        Transform transformToSet = null;
        
        Destroy(spawnedBarrel.gameObject);
        currentBarrelIndex = (currentBarrelIndex + 1) % database.Barrels.Count;
        spawnedBarrel = Instantiate(database.Barrels[currentBarrelIndex]);
        
        
        spawnedBarrel.Sockets.ForEach(x =>
            {
                switch (x.Type)
                {
                    case CannonPartType.Wheel:
                        if (doneWheels == 0)
                        {
                            transformToSet = spawnedWheel1.transform;
                            doneWheels++;
                        }
                        else
                        {
                            transformToSet = spawnedWheel2.transform;
                        }
                        break;
                    
                    case CannonPartType.Stand:
                        transformToSet = spawnedStand.transform;
                        break;
                }

                if (transformToSet != null)
                {
                    transformToSet.position = x.transform.position;
                    transformToSet.rotation = x.transform.rotation;
                }
            });
        UpdateCannonMaterialsHandler();
    }

    private void OnGenerateNewCannon()
    {
        Destroy(spawnedBarrel.gameObject);
        Destroy(spawnedStand.gameObject);
        DestroyWheels();
        
        currentWheelsIndex = -1;
        currentBarrelIndex = -1;
        currentStandIndex = -1;
        
        spawnedBarrel = null;
        spawnedStand = null;

        SpawnRandomCannon();
        UpdateCannonMaterialsHandler();
    }

    private void OnColorChange()
    {
        cannonMaterialsHandler.ChangeColorToRandom();
        UpdateCannonsInfo();
    }

    private void SpawnRandomCannon()
    {
        int spawnedWheels = 0;
        var dataResponse = database.GetRandomPart(CannonPartType.Barrel);
        currentBarrelIndex = dataResponse.index;
        spawnedBarrel = Instantiate(dataResponse.part, Vector3.zero, Quaternion.identity);

        spawnedBarrel.Sockets.ForEach(x =>
        {
            CannonPart spawnedPart = null;
            if (x.Type == CannonPartType.Wheel && spawnedWheels != 0)
            {
                dataResponse.part = database.Wheels[currentWheelsIndex];
            }
            else
            {
                dataResponse = database.GetRandomPart(x.Type);
            }

            spawnedPart = Instantiate(dataResponse.part, x.transform.position, x.transform.rotation);

            switch (spawnedPart.Type)
            {
                case CannonPartType.Wheel:
                    if (spawnedWheels == 0)
                    {
                        spawnedWheel1 = spawnedPart;
                        currentWheelsIndex = dataResponse.index;
                    }
                    else
                    {
                        spawnedWheel2 = spawnedPart;
                    }
                    spawnedWheels++;
                    break;
                
                case CannonPartType.Stand:
                    spawnedStand = spawnedPart;
                    currentStandIndex = dataResponse.index;
                    break;
            }
        });

        CreateID();
        UpdateCannonMaterialsHandler();
    }

    private void DestroyWheels()
    {
        Destroy(spawnedWheel1.gameObject);
        Destroy(spawnedWheel2.gameObject);

        spawnedWheel1 = null;
        spawnedWheel2 = null;
    }

    private void CreateID()
    {
        id = gameObject.GetHashCode();
    }

    private void UpdateCannonMaterialsHandler(bool overwriteColors = true)
    {
        cannonMaterialsHandler?.UpdateParts(new List<CannonPart>
        {
            spawnedBarrel,
            spawnedStand,
            spawnedWheel1,
            spawnedWheel2
        });
        UpdateCannonsInfo(overwriteColors);
    }

    #endregion
}
