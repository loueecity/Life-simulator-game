using UnityEngine;
[System.Serializable]
public class ItemDetails
{
    public int itemID;
    public ItemType itemType;
    public string itemDesc;
    public Sprite itemIcon;
    public string itemLongDesc;
    public short itemGridRadius;
    public float itemUseRadius;
    public bool isStartItem;
    public bool canBePicked;
    public bool canDrop;
    public bool isEdible;
    public bool canCarry;
}
