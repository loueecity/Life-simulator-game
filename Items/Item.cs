using UnityEngine;

public class Item : MonoBehaviour
{[ItemIDDesc]
 [SerializeField]
 
 private int _itemID;
 public SpriteRenderer spriteRenderer;
 public SpriteRenderer sr {get { return spriteRenderer;} set{ spriteRenderer = value;}}
 public int ItemID {get {return _itemID;} set {_itemID = value;}}

 private void Awake()
 {
     spriteRenderer = GetComponentInChildren<SpriteRenderer>();
 }

 private void Start()
 {
     if (ItemID != 0){
         InitialiseItem(ItemID);
     }
 }

 public void InitialiseItem(int itemIDP)
 {
     if (itemIDP != 0)
        {
            ItemID = itemIDP;

            ItemDetails itemDetails = InvManage.Instance.GetItemDetails(ItemID);

            spriteRenderer.sprite = itemDetails.itemIcon;

            // If item type is reapable then add nudgeable component
           // if (itemDetails.itemType == ItemType.Reapable_scenary)
          //  {
           //     gameObject.AddComponent<ItemNudge>();
          //  }
        }

 }
}
