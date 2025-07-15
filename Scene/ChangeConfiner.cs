
using UnityEngine;
using Cinemachine;

public class ChangeConfiner : MonoBehaviour
{
    // Start is called before the first frame update
   private void OnEnable(){
     EventHandler.AfterSceneLoadEvent += ChangeConfinerBox;
   }

   private void OnDisable(){
     EventHandler.AfterSceneLoadEvent -= ChangeConfinerBox;
   }


 
 //Changes the collider for each scene
  private void ChangeConfinerBox(){
    //getting the collider
    PolygonCollider2D polygonCollider2D = GameObject.FindGameObjectWithTag(Tags.BoundaryConfiner).GetComponent<PolygonCollider2D>();
    CinemachineConfiner cinemachineConfiner = GetComponent<CinemachineConfiner>();
    cinemachineConfiner.m_BoundingShape2D = polygonCollider2D;

    //CLEARING CACHE
    cinemachineConfiner.InvalidatePathCache();
  }
}
