using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GenGUID))]
public class SceneItemManager : SingletonMonoBehaviour<SceneItemManager>, ISaveable
{
    private Transform parentItem;
    [SerializeField] private GameObject itemPrefab = null;

    private string _iSaveableUniqueID;
    public string ISaveableUniqueID { get { return _iSaveableUniqueID; } set { _iSaveableUniqueID = value; } }

    private SaveGameObjects  _saveGameObjects;
    public SaveGameObjects  saveGameObjects  { get { return _saveGameObjects; } set { _saveGameObjects = value; } }


    private void AfterSceneLoad()
    {
        parentItem = GameObject.FindGameObjectWithTag(Tags.ItemsParentTransform).transform;
    }

    protected override void Awake()
    {
        base.Awake();

        ISaveableUniqueID = GetComponent<GenGUID>().GUID;
        saveGameObjects  = new SaveGameObjects();
    }

    /// <summary>
    /// Destroy items currently in the scene
    /// </summary>
    private void DestroySceneItems()
    {
        // Get all items in the scene
        Item[] itemsInScene = GameObject.FindObjectsOfType<Item>();

        // Loop through all scene items and destroy them
        for (int i = itemsInScene.Length - 1; i > -1; i--)
        {
            Destroy(itemsInScene[i].gameObject);
        }
    }

    public void InstantiateSceneItem(int itemID, Vector3 itemPosition)
    {
        GameObject itemGameObject = Instantiate(itemPrefab, itemPosition, Quaternion.identity, parentItem);
        Item item = itemGameObject.GetComponent<Item>();
        item.InitialiseItem(itemID);
    }

    private void InstantiateSceneItems(List<SceneItems> sceneItemList)
    {
        GameObject itemGameObject;

        foreach (SceneItems sceneItem in sceneItemList)
        {
            itemGameObject = Instantiate(itemPrefab, new Vector3(sceneItem.pos.x, sceneItem.pos.y, sceneItem.pos.z), Quaternion.identity, parentItem);

            Item item = itemGameObject.GetComponent<Item>();
            item.ItemID = sceneItem.itemID;
            item.name = sceneItem.itemName;
        }
    }

    private void OnDisable()
    {
        ISaveableDeregister();
        EventHandler.AfterSceneLoadEvent -= AfterSceneLoad;
    }

    private void OnEnable()
    {
        ISaveableRegister();
        EventHandler.AfterSceneLoadEvent += AfterSceneLoad;
    }

    public void ISaveableDeregister()
    {
        SaveManager.Instance.iSaveableObjList.Remove(this);
    }

    public void ISaveableRestoreScene(string sceneName)
    {
        if (saveGameObjects.sceneData.TryGetValue(sceneName, out SaveScene saveScene))
        {
            if (saveScene.listSceneItems != null)
            {
                // scene list items found - destroy existing items in scene
                DestroySceneItems();

                // now instantiate the list of scene items
                InstantiateSceneItems(saveScene.listSceneItems);
            }
        }
    }

    public void ISaveableRegister()
    {
        SaveManager.Instance.iSaveableObjList.Add(this);
    }

    public void ISaveableStoreScene(string sceneName)
    {
        // Remove old scene save for gameObject if exists
        saveGameObjects.sceneData.Remove(sceneName);

        // Get all items in the scene
        List<SceneItems> sceneItemList = new List<SceneItems>();
        Item[] itemsInScene = FindObjectsOfType<Item>();

        // Loop through all scene items
        foreach (Item item in itemsInScene)
        {
            SceneItems sceneItem = new SceneItems();
            sceneItem.itemID = item.ItemID;
            sceneItem.pos = new Vector3Serializable(item.transform.position.x, item.transform.position.y, item.transform.position.z);
            sceneItem.itemName = item.name;

            // Add scene item to list
            sceneItemList.Add(sceneItem);
        }

        // Create list scene items in scene save and set to scene item list
        SaveScene sceneSave = new SaveScene();
        sceneSave.listSceneItems = sceneItemList;

        // Add scene save to gameobject
        saveGameObjects.sceneData.Add(sceneName, sceneSave);
    }
}
