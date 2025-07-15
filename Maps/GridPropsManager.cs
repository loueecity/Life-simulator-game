using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(GenGUID))]
public class GridPropsManager : SingletonMonoBehaviour<GridPropsManager>, ISaveable
{
    private Transform resourceParentTransform;
    private Tilemap groundDecoration1;
    private Tilemap groundDecoration2;
    private Grid grid;
    private Dictionary<string, GridPropertyInfo> gridPropertyDictionary;
    [SerializeField] private SO_ResourceDetailsList so_CropDetailsList = null;
    [SerializeField] private SO_GridProps[] so_gridPropertiesArray = null;
    [SerializeField] private Tile[] dugGround = null;
    [SerializeField] private Tile[] wateredGround = null;
    private bool isFirstTimeLoaded=true;

    private string _iSaveableUniqueID;
    public string ISaveableUniqueID { get { return _iSaveableUniqueID; } set { _iSaveableUniqueID = value; } }

    private SaveGameObjects _saveGameObjects;
    public SaveGameObjects saveGameObjects { get { return _saveGameObjects; } set { _saveGameObjects = value; } }

    protected override void Awake()
    {
        base.Awake();

        ISaveableUniqueID = GetComponent<GenGUID>().GUID;
        saveGameObjects = new SaveGameObjects();
    }

    private void OnEnable()
    {
        ISaveableRegister();

        EventHandler.AfterSceneLoadEvent += AfterSceneLoaded;
        EventHandler.AdvanceGameDayEvent += AdvanceDay;
    }

    private void OnDisable()
    {
        ISaveableDeregister();

        EventHandler.AfterSceneLoadEvent -= AfterSceneLoaded;
        EventHandler.AdvanceGameDayEvent -= AdvanceDay;
    }

    private void Start()
    {
        InitialiseGridProperties();
    }

    private void ClearDisplayGroundDecorations()
    {
        // Remove ground decorations
        groundDecoration1.ClearAllTiles();
        groundDecoration2.ClearAllTiles();
    }

    private void ClearDisplayAllPlantedCrops()
    {
        // Destroy all crops in scene

        Resource[] resourceArray;
        resourceArray = FindObjectsOfType<Resource>();

        foreach (Resource resource in resourceArray)
        {
            Destroy(resource.gameObject);
        }
    }

    private void ClearDisplayGridPropertyDetails()
    {
        ClearDisplayGroundDecorations();

        ClearDisplayAllPlantedCrops();
    }

    public void DisplayDugGround(GridPropertyInfo gridPropertyDetails)
    {
        // Dug
        if (gridPropertyDetails.sinceDug > -1)
        {
            ConnectDugGround(gridPropertyDetails);
        }
    }

    public void DisplayWateredGround(GridPropertyInfo gridPropertyDetails)
    {
        // Watered
        if (gridPropertyDetails.sinceWatered > -1)
        {
            ConnectWateredGround(gridPropertyDetails);
        }
    }


    private void ConnectDugGround(GridPropertyInfo gridPropertyDetails)
    {
        // Select tile based on surrounding dug tiles

        Tile dugTile0 = SetDugTile(gridPropertyDetails.xGrid, gridPropertyDetails.yGrid);
        groundDecoration1.SetTile(new Vector3Int(gridPropertyDetails.xGrid, gridPropertyDetails.yGrid, 0), dugTile0);

        // Set 4 tiles if dug surrounding current tile - up, down, left, right now that this central tile has been dug

        GridPropertyInfo adjacentGridPropertyDetails;

        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.xGrid, gridPropertyDetails.yGrid + 1);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.sinceDug > -1)
        {
            Tile dugTile1 = SetDugTile(gridPropertyDetails.xGrid, gridPropertyDetails.yGrid + 1);
            groundDecoration1.SetTile(new Vector3Int(gridPropertyDetails.xGrid, gridPropertyDetails.yGrid + 1, 0), dugTile1);
        }

        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.xGrid, gridPropertyDetails.yGrid - 1);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.sinceDug > -1)
        {
            Tile dugTile2 = SetDugTile(gridPropertyDetails.xGrid, gridPropertyDetails.yGrid - 1);
            groundDecoration1.SetTile(new Vector3Int(gridPropertyDetails.xGrid, gridPropertyDetails.yGrid - 1, 0), dugTile2);
        }
        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.xGrid - 1, gridPropertyDetails.yGrid);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.sinceDug > -1)
        {
            Tile dugTile3 = SetDugTile(gridPropertyDetails.xGrid - 1, gridPropertyDetails.yGrid);
            groundDecoration1.SetTile(new Vector3Int(gridPropertyDetails.xGrid - 1, gridPropertyDetails.yGrid, 0), dugTile3);
        }

        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.xGrid + 1, gridPropertyDetails.yGrid);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.sinceDug > -1)
        {
            Tile dugTile4 = SetDugTile(gridPropertyDetails.xGrid + 1, gridPropertyDetails.yGrid);
            groundDecoration1.SetTile(new Vector3Int(gridPropertyDetails.xGrid + 1, gridPropertyDetails.yGrid, 0), dugTile4);
        }
    }

    private void ConnectWateredGround(GridPropertyInfo gridPropertyDetails)
    {
        // Select tile based on surrounding watered tiles

        Tile wateredTile0 = SetWateredTile(gridPropertyDetails.xGrid, gridPropertyDetails.yGrid);
        groundDecoration2.SetTile(new Vector3Int(gridPropertyDetails.xGrid, gridPropertyDetails.yGrid, 0), wateredTile0);

        // Set 4 tiles if watered surrounding current tile - up, down, left, right now that this central tile has been watered

        GridPropertyInfo adjacentGridPropertyDetails;

        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.xGrid, gridPropertyDetails.yGrid + 1);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.sinceWatered > -1)
        {
            Tile wateredTile1 = SetWateredTile(gridPropertyDetails.xGrid, gridPropertyDetails.yGrid + 1);
            groundDecoration2.SetTile(new Vector3Int(gridPropertyDetails.xGrid, gridPropertyDetails.yGrid + 1, 0), wateredTile1);
        }

        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.xGrid, gridPropertyDetails.yGrid - 1);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.sinceWatered > -1)
        {
            Tile wateredTile2 = SetWateredTile(gridPropertyDetails.xGrid, gridPropertyDetails.yGrid - 1);
            groundDecoration2.SetTile(new Vector3Int(gridPropertyDetails.xGrid, gridPropertyDetails.yGrid - 1, 0), wateredTile2);
        }

        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.xGrid - 1, gridPropertyDetails.yGrid);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.sinceWatered > -1)
        {
            Tile wateredTile3 = SetWateredTile(gridPropertyDetails.xGrid - 1, gridPropertyDetails.yGrid);
            groundDecoration2.SetTile(new Vector3Int(gridPropertyDetails.xGrid - 1, gridPropertyDetails.yGrid, 0), wateredTile3);
        }

        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.xGrid + 1, gridPropertyDetails.yGrid);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.sinceWatered > -1)
        {
            Tile wateredTile4 = SetWateredTile(gridPropertyDetails.xGrid + 1, gridPropertyDetails.yGrid);
            groundDecoration2.SetTile(new Vector3Int(gridPropertyDetails.xGrid + 1, gridPropertyDetails.yGrid, 0), wateredTile4);
        }
    }

    private Tile SetDugTile(int xGrid, int yGrid)
    {
        //Get whether surrounding tiles (up,down,left, and right) are dug or not

        bool upDug = IsGridSquareDug(xGrid, yGrid + 1);
        bool downDug = IsGridSquareDug(xGrid, yGrid - 1);
        bool leftDug = IsGridSquareDug(xGrid - 1, yGrid);
        bool rightDug = IsGridSquareDug(xGrid + 1, yGrid);

        #region Set appropriate tile based on whether surrounding tiles are dug or not

        if (!upDug && !downDug && !rightDug && !leftDug)
        {
            return dugGround[0];
        }
        else if (!upDug && downDug && rightDug && !leftDug)
        {
            return dugGround[1];
        }
        else if (!upDug && downDug && rightDug && leftDug)
        {
            return dugGround[2];
        }
        else if (!upDug && downDug && !rightDug && leftDug)
        {
            return dugGround[3];
        }
        else if (!upDug && downDug && !rightDug && !leftDug)
        {
            return dugGround[4];
        }
        else if (upDug && downDug && rightDug && !leftDug)
        {
            return dugGround[5];
        }
        else if (upDug && downDug && rightDug && leftDug)
        {
            return dugGround[6];
        }
        else if (upDug && downDug && !rightDug && leftDug)
        {
            return dugGround[7];
        }
        else if (upDug && downDug && !rightDug && !leftDug)
        {
            return dugGround[8];
        }
        else if (upDug && !downDug && rightDug && !leftDug)
        {
            return dugGround[9];
        }
        else if (upDug && !downDug && rightDug && leftDug)
        {
            return dugGround[10];
        }
        else if (upDug && !downDug && !rightDug && leftDug)
        {
            return dugGround[11];
        }
        else if (upDug && !downDug && !rightDug && !leftDug)
        {
            return dugGround[12];
        }
        else if (!upDug && !downDug && rightDug && !leftDug)
        {
            return dugGround[13];
        }
        else if (!upDug && !downDug && rightDug && leftDug)
        {
            return dugGround[14];
        }
        else if (!upDug && !downDug && !rightDug && leftDug)
        {
            return dugGround[15];
        }

        return null;

        #endregion Set appropriate tile based on whether surrounding tiles are dug or not
    }

    private bool IsGridSquareDug(int xGrid, int yGrid)
    {
        GridPropertyInfo gridPropertyDetails = GetGridPropertyDetails(xGrid, yGrid);

        if (gridPropertyDetails == null)
        {
            return false;
        }
        else if (gridPropertyDetails.sinceDug > -1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private Tile SetWateredTile(int xGrid, int yGrid)
    {
        // Get whether surrounding tiles (up,down,left, and right) are watered or not

        bool upWatered = IsGridSquareWatered(xGrid, yGrid + 1);
        bool downWatered = IsGridSquareWatered(xGrid, yGrid - 1);
        bool leftWatered = IsGridSquareWatered(xGrid - 1, yGrid);
        bool rightWatered = IsGridSquareWatered(xGrid + 1, yGrid);

        #region Set appropriate tile based on whether surrounding tiles are watered or not

        if (!upWatered && !downWatered && !rightWatered && !leftWatered)
        {
            return wateredGround[0];
        }
        else if (!upWatered && downWatered && rightWatered && !leftWatered)
        {
            return wateredGround[1];
        }
        else if (!upWatered && downWatered && rightWatered && leftWatered)
        {
            return wateredGround[2];
        }
        else if (!upWatered && downWatered && !rightWatered && leftWatered)
        {
            return wateredGround[3];
        }
        else if (!upWatered && downWatered && !rightWatered && !leftWatered)
        {
            return wateredGround[4];
        }
        else if (upWatered && downWatered && rightWatered && !leftWatered)
        {
            return wateredGround[5];
        }
        else if (upWatered && downWatered && rightWatered && leftWatered)
        {
            return wateredGround[6];
        }
        else if (upWatered && downWatered && !rightWatered && leftWatered)
        {
            return wateredGround[7];
        }
        else if (upWatered && downWatered && !rightWatered && !leftWatered)
        {
            return wateredGround[8];
        }
        else if (upWatered && !downWatered && rightWatered && !leftWatered)
        {
            return wateredGround[9];
        }
        else if (upWatered && !downWatered && rightWatered && leftWatered)
        {
            return wateredGround[10];
        }
        else if (upWatered && !downWatered && !rightWatered && leftWatered)
        {
            return wateredGround[11];
        }
        else if (upWatered && !downWatered && !rightWatered && !leftWatered)
        {
            return wateredGround[12];
        }
        else if (!upWatered && !downWatered && rightWatered && !leftWatered)
        {
            return wateredGround[13];
        }
        else if (!upWatered && !downWatered && rightWatered && leftWatered)
        {
            return wateredGround[14];
        }
        else if (!upWatered && !downWatered && !rightWatered && leftWatered)
        {
            return wateredGround[15];
        }

        return null;

        #endregion Set appropriate tile based on whether surrounding tiles are watered or not
    }

    private bool IsGridSquareWatered(int xGrid, int yGrid)
    {
        GridPropertyInfo gridPropertyDetails = GetGridPropertyDetails(xGrid, yGrid);

        if (gridPropertyDetails == null)
        {
            return false;
        }
        else if (gridPropertyDetails.sinceWatered > -1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void DisplayGridPropertyDetails()
    {
        // Loop through all grid items
        foreach (KeyValuePair<string, GridPropertyInfo> item in gridPropertyDictionary)
        {
            GridPropertyInfo gridPropertyDetails = item.Value;

            DisplayDugGround(gridPropertyDetails);

            DisplayWateredGround(gridPropertyDetails);

            DisplayPlantedCrop(gridPropertyDetails);
        }
    }

    /// <summary>
    /// Display planted crop for gridpropertyDetails
    /// </summary>
    public void DisplayPlantedCrop(GridPropertyInfo gridPropertyDetails)
    {
        if (gridPropertyDetails.seedID > -1)
        {
            // get crop details
            ResourceDetails cropDetails = so_CropDetailsList.GetResourceDetails(gridPropertyDetails.seedID);

            if (cropDetails != null)
            {
                // prefab to use
                GameObject cropPrefab;

                // instantiate crop prefab at grid location
                int growthStages = cropDetails.growthDays.Length;

                int currentGrowthStage = 0;

                for (int i = growthStages - 1; i >= 0; i--)
                {
                    if (gridPropertyDetails.growthDay >= cropDetails.growthDays[i])
                    {
                        currentGrowthStage = i;
                        break;
                    }

                }

                cropPrefab = cropDetails.growthPrefab[currentGrowthStage];

                Sprite growthSprite = cropDetails.growthSprite[currentGrowthStage];

                Vector3 worldPosition = groundDecoration2.CellToWorld(new Vector3Int(gridPropertyDetails.xGrid, gridPropertyDetails.yGrid, 0));

                worldPosition = new Vector3(worldPosition.x + Settings.gridCellSize / 2, worldPosition.y, worldPosition.z);

                GameObject cropInstance = Instantiate(cropPrefab, worldPosition, Quaternion.identity);

                cropInstance.GetComponentInChildren<SpriteRenderer>().sprite = growthSprite;
                cropInstance.transform.SetParent(resourceParentTransform);
                cropInstance.GetComponent<Resource>().resourceGridPos = new Vector2Int(gridPropertyDetails.xGrid, gridPropertyDetails.yGrid);
            }
        }
    }


    /// <summary>
    /// This initialises the grid property dictionary with the values from the SO_GridProperties assets and stores the values for each scene in
    /// GameObjectSave sceneData
    /// </summary>
    private void InitialiseGridProperties()
    {
        // Loop through all gridproperties in the array
        foreach (SO_GridProps so_GridProperties in so_gridPropertiesArray)
        {
            // Create dictionary of grid property details
            Dictionary<string, GridPropertyInfo> gridPropertyDictionary = new Dictionary<string, GridPropertyInfo>();

            // Populate grid property dictionary - Iterate through all the grid properties in the so gridproperties list
            foreach (GridProperty gridProperty in so_GridProperties.gridPropertyList)
            {
                GridPropertyInfo gridPropertyDetails;

                gridPropertyDetails = GetGridPropertyDetails(gridProperty.gridCoord.x, gridProperty.gridCoord.y, gridPropertyDictionary);

                if (gridPropertyDetails == null)
                {
                    gridPropertyDetails = new GridPropertyInfo();
                }

                switch (gridProperty.gridBoolProperties)
                {
                    case GridBoolProperties.isDiggable:
                        gridPropertyDetails.isDiggable = gridProperty.gridBoolVal;
                        break;

                    case GridBoolProperties.canDrop:
                        gridPropertyDetails.canDrop = gridProperty.gridBoolVal;
                        break;

                    case GridBoolProperties.canPlaceFurn:
                        gridPropertyDetails.canPlaceFurn = gridProperty.gridBoolVal;
                        break;

                    case GridBoolProperties.isPath:
                        gridPropertyDetails.isPath = gridProperty.gridBoolVal;
                        break;

                    case GridBoolProperties.isNPCObs:
                        gridPropertyDetails.isNPCObs = gridProperty.gridBoolVal;
                        break;

                    default:
                        break;
                }

                SetGridPropertyDetails(gridProperty.gridCoord.x, gridProperty.gridCoord.y, gridPropertyDetails, gridPropertyDictionary);
            }

            // Create scene save for this gameobject
            SaveScene sceneSave = new SaveScene();

            // Add grid property dictionary to scene save data
            sceneSave.gridPropertyInfoDict = gridPropertyDictionary;

            // If starting scene set the gridPropertyDictionary member variable to the current iteration
            if (so_GridProperties.sceneName.ToString() == SceneManagerController.Instance.startSceneName.ToString())
            {
                this.gridPropertyDictionary = gridPropertyDictionary;
            }

            sceneSave.boolDict = new Dictionary<string, bool>();
            sceneSave.boolDict.Add("isFirstTimeLoaded", true);


            // Add scene save to game object scene data
            saveGameObjects.sceneData.Add(so_GridProperties.sceneName.ToString(), sceneSave);
        }
    }

    private void AfterSceneLoaded()
    {

        if (GameObject.FindGameObjectWithTag(Tags.ResourceParentTransform) != null)
        {
            resourceParentTransform = GameObject.FindGameObjectWithTag(Tags.ResourceParentTransform).transform;
        }
        else
        {
            resourceParentTransform = null;
        }



        // Get Grid
        grid = GameObject.FindObjectOfType<Grid>();

        // Get tilemaps
        groundDecoration1 = GameObject.FindGameObjectWithTag(Tags.GroundDecoration1).GetComponent<Tilemap>();
        groundDecoration2 = GameObject.FindGameObjectWithTag(Tags.GroundDecoration2).GetComponent<Tilemap>();

    }

    /// <summary>
    /// Returns the gridPropertyDetails at the gridlocation for the supplied dictionary, or null if no properties exist at that location.
    /// </summary>
    public GridPropertyInfo GetGridPropertyDetails(int gridX, int gridY, Dictionary<string, GridPropertyInfo> gridPropertyDictionary)
    {
        // Construct key from coordinate
        string key = "x" + gridX + "y" + gridY;

        GridPropertyInfo gridPropertyDetails;

        // Check if grid property details exist forcoordinate and retrieve
        if (!gridPropertyDictionary.TryGetValue(key, out gridPropertyDetails))
        {
            // if not found
            return null;
        }
        else
        {
            return gridPropertyDetails;
        }
    }

    /// <summary>
    ///  Returns the Crop object at the gridX, gridY position or null if no crop was found
    /// </summary>
    public Resource GetResourceObjectAtGridLocation(GridPropertyInfo gridPropertyDetails)
    {
        Vector3 worldPosition = grid.GetCellCenterWorld(new Vector3Int(gridPropertyDetails.xGrid, gridPropertyDetails.yGrid, 0));
        Collider2D[] collider2DArray = Physics2D.OverlapPointAll(worldPosition);

        // Loop through colliders to get crop game object
        Resource resource = null;

        for (int i = 0; i < collider2DArray.Length; i++)
        {
            resource = collider2DArray[i].gameObject.GetComponentInParent<Resource>();
            if (resource != null && resource.resourceGridPos == new Vector2Int(gridPropertyDetails.xGrid, gridPropertyDetails.yGrid))
                break;
            resource = collider2DArray[i].gameObject.GetComponentInChildren<Resource>();
            if (resource != null && resource.resourceGridPos == new Vector2Int(gridPropertyDetails.xGrid, gridPropertyDetails.yGrid))
                break;
        }

        return resource;
    }

    /// <summary>
    /// Returns Crop Details for the provided seedItemCode
    /// </summary>
    public ResourceDetails GetResourceDetails(int seedItemCode)
    {
        return so_CropDetailsList.GetResourceDetails(seedItemCode);
    }



    /// <summary>
    /// Get the grid property details for the tile at (gridX,gridY).  If no grid property details exist null is returned and can assume that all grid property details values are null or false
    /// </summary>
    public GridPropertyInfo GetGridPropertyDetails(int gridX, int gridY)
    {
        return GetGridPropertyDetails(gridX, gridY, gridPropertyDictionary);
    }

    public void ISaveableDeregister()
    {
        SaveManager.Instance.iSaveableObjList.Remove(this);
       // SaveLoadManager.Instance.iSaveableObjectList.Remove(this);
    }

    public void ISaveableRegister()
    {
        SaveManager.Instance.iSaveableObjList.Add(this);
    }

    public void ISaveableRestoreScene(string sceneName)
    {
        // Get sceneSave for scene - it exists since we created it in initialise
        if (saveGameObjects.sceneData.TryGetValue(sceneName, out SaveScene sceneSave))
        {
            // get grid property details dictionary - it exists since we created it in initialise
            if (sceneSave.gridPropertyInfoDict != null)
            {
                gridPropertyDictionary = sceneSave.gridPropertyInfoDict;
            }

            if(sceneSave.boolDict !=null && sceneSave.boolDict.TryGetValue("isFirstTimeLoaded",out bool storedisFirstTimeLoaded))
            {
                isFirstTimeLoaded = storedisFirstTimeLoaded;
            }
            if (isFirstTimeLoaded)
            {
                EventHandler.CallInstantiateResourcePrefabsEvent();
            }

            // If grid properties exist
            if (gridPropertyDictionary.Count > 0)
            {
                // grid property details found for the current scene destroy existing ground decoration
                ClearDisplayGridPropertyDetails();

                // Instantiate grid property details for current scene
                DisplayGridPropertyDetails();
            }

            if(isFirstTimeLoaded == true)
            {
                isFirstTimeLoaded = false;
            }
        }
    }

    public void ISaveableStoreScene(string sceneName)
    {
        // Remove sceneSave for scene
        saveGameObjects.sceneData.Remove(sceneName);

        // Create sceneSave for scene
        SaveScene sceneSave = new SaveScene();

        // create & add dict grid property details dictionary
        sceneSave.gridPropertyInfoDict = gridPropertyDictionary;

        sceneSave.boolDict = new Dictionary<string, bool>();
        sceneSave.boolDict.Add("isFirstTimeLoaded", isFirstTimeLoaded);

        // Add scene save to game object scene data
        saveGameObjects.sceneData.Add(sceneName, sceneSave);
    }

    /// <summary>
    /// Set the grid property details to gridPropertyDetails for the tile at (gridX,gridY) for current scene
    /// </summary>
    public void SetGridPropertyDetails(int gridX, int gridY, GridPropertyInfo gridPropertyDetails)
    {
        SetGridPropertyDetails(gridX, gridY, gridPropertyDetails, gridPropertyDictionary);
    }

    /// <summary>
    /// Set the grid property details to gridPropertyDetails for the tile at (gridX,gridY) for the gridpropertyDictionary.
    /// </summary>
    public void SetGridPropertyDetails(int gridX, int gridY, GridPropertyInfo gridPropertyDetails, Dictionary<string, GridPropertyInfo> gridPropertyDictionary)
    {
        // Construct key from coordinate
        string key = "x" + gridX + "y" + gridY;

        gridPropertyDetails.xGrid = gridX;
        gridPropertyDetails.yGrid = gridY;

        // Set value
        gridPropertyDictionary[key] = gridPropertyDetails;
    }

    private void AdvanceDay(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek, int gameHour, int gameMinute, int gameSecond)
    {
        // Clear Display All Grid Property Details
        ClearDisplayGridPropertyDetails();

        // Loop through all scenes - by looping through all gridproperties in the array
        foreach (SO_GridProps so_GridProperties in so_gridPropertiesArray)
        {
            // Get gridpropertydetails dictionary for scene
            if (saveGameObjects.sceneData.TryGetValue(so_GridProperties.sceneName.ToString(), out SaveScene sceneSave))
            {
                if (sceneSave.gridPropertyInfoDict != null)
                {
                    for (int i = sceneSave.gridPropertyInfoDict.Count - 1; i >= 0; i--)
                    {
                        KeyValuePair<string, GridPropertyInfo> item = sceneSave.gridPropertyInfoDict.ElementAt(i);

                        GridPropertyInfo gridPropertyDetails = item.Value;

                        #region Update all grid properties to reflect the advance in the day

                        // If a crop is planted
                        if (gridPropertyDetails.growthDay > -1)
                        {
                            gridPropertyDetails.growthDay += 1;
                        }

                        // If ground is watered, then clear water
                        if (gridPropertyDetails.sinceWatered > -1)
                        {
                            gridPropertyDetails.sinceWatered = -1;
                        }

                        // Set gridpropertydetails
                        SetGridPropertyDetails(gridPropertyDetails.xGrid, gridPropertyDetails.yGrid, gridPropertyDetails, sceneSave.gridPropertyInfoDict);

                        #endregion Update all grid properties to reflect the advance in the day
                    }
                }
            }
        }

        // Display grid property details to reflect changed values
        DisplayGridPropertyDetails();
    }
}
