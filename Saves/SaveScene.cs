using System.Collections.Generic;

[System.Serializable]
public class SaveScene
{
    public List<SceneItems> listSceneItems; //saves the items in the scene
    public Dictionary<string, GridPropertyInfo> gridPropertyInfoDict; //saves state of the info of the grid
    //public Dictionary<string, List<SceneItems>> listSceneItemsDict;

    public Dictionary<string, bool> boolDict;
}
