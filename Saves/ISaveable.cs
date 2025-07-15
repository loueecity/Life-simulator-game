public interface ISaveable
{
    string ISaveableUniqueID { get; set; }

    SaveGameObjects saveGameObjects { get; set; }

    //registers game objects with the save manager
    void ISaveableRegister();

   //deregisters game objects with the save manager
    void ISaveableDeregister();

    //saves scene data
    void ISaveableStoreScene(string sceneName);

    //restores the scene data
    void ISaveableRestoreScene(string sceneName);

}
