using System.Collections.Generic;
using UnityEngine;

public class InvManage : SingletonMonoBehaviour<InvManage>
{
   private Dictionary<int,ItemDetails> itemDetailsDict;
   public List<InvItem>[] invLists;
   private int[] currentItem;

   [HideInInspector] public int[] invListCapacityArray;
   [SerializeField] private SO_Items itemList = null;

   protected override void Awake()
   {
       base.Awake();
       
       CreateInvLists();
       //creates dict
       CreateItemDict();
       currentItem = new int[(int)InvLoc.count];
       for (int i=0; i<currentItem.Length; i++)
       {
           currentItem[i] = -1;
       }
       
   }

private void CreateInvLists(){
    invLists = new List<InvItem>[(int)InvLoc.count];
    
    
    for (int i=0; i<(int)InvLoc.count; i++){
        invLists[i] = new List<InvItem>();
    }
    invListCapacityArray = new int[(int)InvLoc.count];
    invListCapacityArray[(int)InvLoc.player] = Settings.playerStartInvCapacity;
    

}

public void AddItem(InvLoc invLoc, Item item, GameObject gameObjToDel){
    AddItem(invLoc,item);
    Destroy(gameObjToDel);
}
public void AddItem(InvLoc invLoc, Item item)
{
    int itemID = item.ItemID;
    List<InvItem> invList = invLists[(int)invLoc];

    int itemPosition = FindItemInInv(invLoc, itemID);

    if(itemPosition != -1)
    {
        AddItemAtPos(invList, itemID, itemPosition);
    }else{
        AddItemAtPos(invList,itemID);
    }
    EventHandler.CallInvUpdateEvent(invLoc,invLists[(int)invLoc]);
}

    public void AddItem(InvLoc inventoryLocation, int itemCode)
    {
        List<InvItem> inventoryList = invLists[(int)inventoryLocation];

        // Check if inventory already contains the item
        int itemPosition = FindItemInInv(inventoryLocation, itemCode);

        if (itemPosition != -1)
        {
            AddItemAtPos(inventoryList, itemCode, itemPosition);
        }
        else
        {
            AddItemAtPos(inventoryList, itemCode);
        }

        //  Send event that inventory has been updated
        EventHandler.CallInvUpdateEvent(inventoryLocation, invLists[(int)inventoryLocation]);
    }

    public int FindItemInInv(InvLoc invLoc, int itemID){
    List<InvItem> invList = invLists[(int)invLoc];
    for (int i=0; i< invList.Count;i++){
        if(invList[i].itemID == itemID)
        {
            return i;
        }
    }
    return -1;
}

private void AddItemAtPos(List<InvItem> invList, int itemID){
    InvItem invItem = new InvItem();
    invItem.itemID = itemID;
    invItem.itemQuantity = 1;
    invList.Add(invItem);

    DebugPrintInvList(invList);

}

private void AddItemAtPos(List<InvItem> invList, int itemID, int pos){
   InvItem invItem = new InvItem();

   int quantity = invList[pos].itemQuantity + 1;
   invItem.itemQuantity = quantity;
   invItem.itemID = itemID;
   invList[pos] = invItem;

   Debug.ClearDeveloperConsole();
   DebugPrintInvList(invList);

}

//goes through the list of items to create the dict
   private void CreateItemDict()
   {
       itemDetailsDict = new Dictionary<int, ItemDetails>();
       foreach (ItemDetails itemDetails in itemList.itemDetails)
       {
           itemDetailsDict.Add(itemDetails.itemID,itemDetails);
       }
   }

   public ItemDetails GetItemDetails(int itemID)
   {
       ItemDetails itemDetails;
       if (itemDetailsDict.TryGetValue(itemID, out itemDetails))
       {
           return itemDetails;
       }else {
           return null;
       }
   }

    private int GetSelectedInvItem(InvLoc invLoc)
    {
        return currentItem[(int)invLoc];
    }

    public ItemDetails GetSelectedInvItemDetails(InvLoc invLoc)
    {
        int itemID = GetSelectedInvItem(invLoc);

        if (itemID == -1)
        {
            return null;
        }else
        {
            return GetItemDetails(itemID);
        }
    }

    //for getting the type of the item this is run when using items to decide the function
   public string GetItemType(ItemType type){
       string itemTypeDescription;
       switch(type){
           case ItemType.AxeTool:
           itemTypeDescription = Settings.axeTool;
           break;

           case ItemType.PickaxeTool:
           itemTypeDescription = Settings.pickaxeTool;
           break;

           case ItemType.ScytheTool:
           itemTypeDescription = Settings.scytheTool;
           break;

           case ItemType.WateringTool:
           itemTypeDescription = Settings.waterCanTool;
           break;

           case ItemType.CollectorTool:
           itemTypeDescription = Settings.collectorTool;
           break;

           default:
           itemTypeDescription = type.ToString();
           break;
       }
       return itemTypeDescription;
   }

   public void DropItem(InvLoc invLoc, int itemID){
       List<InvItem> invList = invLists[(int)invLoc];
       int itemPosition = FindItemInInv(invLoc, itemID);

       if(itemPosition != -1){
           DeleteItemAtPos(invList, itemID, itemPosition);
       }
       EventHandler.CallInvUpdateEvent(invLoc, invLists[(int)invLoc]);
   }

   private void DeleteItemAtPos(List<InvItem> invList, int itemID, int pos){
       InvItem invItem = new InvItem();
       int quantity = invList[pos].itemQuantity - 1;

       if(quantity>0){
           invItem.itemQuantity=quantity;
           invItem.itemID = itemID;
           invList[pos]=invItem;
       }else{
           invList.RemoveAt(pos);
       }
   }

   public void SwapInvItems(InvLoc invLoc, int fromItem, int toItem){
       // if fromItem index and toItemIndex are within the bounds of the list, not the same, and greater than or equal to zero
        if (fromItem < invLists[(int)invLoc].Count && toItem < invLists[(int)invLoc].Count
             && fromItem != toItem && fromItem >= 0 && toItem >= 0)
        {
            InvItem fromInventoryItem = invLists[(int)invLoc][fromItem];
            InvItem toInventoryItem = invLists[(int)invLoc][toItem];

            invLists[(int)invLoc][toItem] = fromInventoryItem;
            invLists[(int)invLoc][fromItem] = toInventoryItem;

            //  Send event that inventory has been updated
            EventHandler.CallInvUpdateEvent(invLoc, invLists[(int)invLoc]);
        }

   }

   public void setCurrentItem(InvLoc loc, int id){
       currentItem[(int)loc]=id;
   }

   public void clearCurrentItem(InvLoc loc){
       currentItem[(int)loc] = -1;
   }




   private void DebugPrintInvList(List<InvItem> invList)
   {
       foreach (InvItem invItem in invList)
       {
           Debug.Log("Item Desc: " + InvManage.Instance.GetItemDetails(invItem.itemID).itemDesc + " Item Quantity: " + invItem.itemQuantity);
       }
       Debug.Log("------------------------------------------------");
   }
}
