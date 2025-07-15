[System.Serializable]
public class SceneItems
{
    public int itemID;
    public Vector3Serializable pos;
    public string itemName;

    public SceneItems()
    {
        pos = new Vector3Serializable();
    }
}
