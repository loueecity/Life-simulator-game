using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public class SceneTeleport : MonoBehaviour
{
    [SerializeField] private SceneManagement sceneGoTo = SceneManagement.Scene1_Farm;
    [SerializeField] private Vector3 scenePosGoTo = new Vector3();
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
       Player player = collision.GetComponent<Player>();

       if(player !=null)
       {
           //calc new pos
           float xPos = Mathf.Approximately(scenePosGoTo.x, 0f) ? player.transform.position.x : scenePosGoTo.x;
           float yPos = Mathf.Approximately(scenePosGoTo.y, 0f) ? player.transform.position.y : scenePosGoTo.y;
           float zPos = 0f;
           SceneManagerController.Instance.FadeAndLoadScene(sceneGoTo.ToString(), new Vector3(xPos,yPos,zPos));
       }
    }
}
