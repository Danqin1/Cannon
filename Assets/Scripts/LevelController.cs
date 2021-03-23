using UnityEngine;
using UnityTemplateProjects;

public class LevelController : MonoBehaviour
{
    #region variables
    
    [SerializeField] private CannonPartsDatabase database;
    [SerializeField] private HUD hud;

    private CannonPart spawnedBarrel;
    private CannonPart spawnedWheel1;
    private CannonPart spawnedWheel2;
    private CannonPart spawnedStand;

    private int id;
    private int currentBarrelIndex = -1;
    private int currentWheelsIndex = -1;
    private int currentStandIndex = -1;
    
    #endregion

    #region unity methods

    private void Start()
    {
        SubscribeToEvents();
        if (ApplicationController.ChosenNewCannon)
        {
            SpawnRandomCannon();
        }
        else if (!ApplicationController.CannonsMemento.Equals(default(CannonsMemento)))
        {
            RestoreCannon(ApplicationController.CannonsMemento.cannons[ApplicationController.ChosenCannonIndex]);
        }
        else
        {
            SpawnRandomCannon();
        }
    }

    private void OnDestroy()
    {
        SaveSystem.Save(CreateMemento());
        DisposeFromEvents();
    }

    #endregion

    #region private methods
    
    private void SubscribeToEvents()
    {
        hud.onGenerateNewCannon += OnGenerateNewCannon;
        hud.onChangeBarrel += OnBarrelChanged;
        hud.onChangeStand += OnChangeStand;
        hud.onChangeWheel += OnChangeWheels;
    }

    private void DisposeFromEvents()
    {
        hud.onGenerateNewCannon += OnBarrelChanged;
        hud.onChangeBarrel += OnBarrelChanged;
        hud.onChangeStand += OnBarrelChanged;
        hud.onChangeWheel += OnBarrelChanged;
    }

    private void RestoreCannon(CannonInfo infos)
    {
        int spawnedWheelsCount = 0;
        spawnedBarrel = Instantiate(database.Barrels[infos.barrelIndex]);
        CannonPart part = spawnedBarrel;
        
        spawnedBarrel.Sockets.ForEach(x =>
            {
                switch (x.Type)
                {
                    case CannonPartType.Wheel:
                        part = Instantiate(database.Wheels[infos.wheelsIndex]);
                        spawnedWheelsCount++;
               
                        if (spawnedWheel1 == null)
                        {
                            spawnedWheel1 = part;
                        }
                        else
                        {
                            spawnedWheel2 = part;
                        }
                        break;
                    case CannonPartType.Stand:
                        part = Instantiate(database.Stands[infos.standIndex]);
                        break;
                }

                part.transform.position = x.transform.position;
                part.transform.rotation = x.transform.rotation;
            });
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
               spawnedWheelsCount++;
               
               if (spawnedWheel1 == null)
               {
                   spawnedWheel1 = part;
               }
               else
               {
                   spawnedWheel2 = part;
               }
           });
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
    }

    private void SpawnRandomCannon()
    {
        var dataResponse = database.GetRandomPart(CannonPartType.Barrel);
        currentBarrelIndex = dataResponse.index;
        spawnedBarrel = Instantiate(dataResponse.part, Vector3.zero, Quaternion.identity);

        spawnedBarrel.Sockets.ForEach(x =>
        {
            CannonPart spawnedPart = null;
            if (x.Type == CannonPartType.Wheel && spawnedWheel1 != null)
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
                    if (spawnedWheel1 == null)
                    {
                        spawnedWheel1 = spawnedPart;
                        currentWheelsIndex = dataResponse.index;
                    }
                    else
                    {
                        spawnedWheel2 = spawnedPart;
                    }
                    break;
                
                case CannonPartType.Stand:
                    spawnedStand = spawnedPart;
                    currentStandIndex = dataResponse.index;
                    break;
            }
        });

        CreateID();
        AddToCannons();
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

    private void AddToCannons()
    {
        ApplicationController.CannonsMemento.cannons.Add(new CannonInfo()
        {
            cannonId = id,
            barrelIndex = currentBarrelIndex,
            standIndex = currentStandIndex,
            wheelsIndex = currentWheelsIndex
        });
    }

    private CannonsMemento CreateMemento()
    {
        var savedCannon = ApplicationController.CannonsMemento.cannons.Find(x => x.cannonId == id);
        if (!savedCannon.Equals(default))
        {
            savedCannon.barrelIndex = currentBarrelIndex;
            savedCannon.standIndex = currentStandIndex;
            savedCannon.wheelsIndex = currentWheelsIndex;
        }
        else
        {
            AddToCannons();
        }

        return ApplicationController.CannonsMemento;
    }

    #endregion
}
