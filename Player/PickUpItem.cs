using UnityEngine;

public class PickUpItem : MonoBehaviour
{

    //if player collides in game with an item
  private void OnTriggerEnter2D(Collider2D collision)
  {
      Item item = collision.GetComponent<Item>();
      if (item !=null)
      {
          //retrieve details of item
          ItemDetails itemDetails = InvManage.Instance.GetItemDetails(item.ItemID);
          
          
          if (itemDetails.canBePicked == true)
          {
              InvManage.Instance.AddItem(InvLoc.player, item, collision.gameObject);
          }
      }
  }


}
