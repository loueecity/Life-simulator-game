using System.Collections.Generic;
[System.Serializable]
public class SaveGameObjects
{
    // dictionary for each scene to save items in each scene
    public Dictionary<string, SaveScene> sceneData;

    public SaveGameObjects(){
        sceneData = new Dictionary<string, SaveScene>();
    }
    public SaveGameObjects(Dictionary<string, SaveScene> sceneData){
        this.sceneData = sceneData;
    }
}
