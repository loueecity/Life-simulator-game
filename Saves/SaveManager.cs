using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SaveManager : SingletonMonoBehaviour<SaveManager>
{
    //ISaveable interface
    public List<ISaveable> iSaveableObjList;
    protected override void Awake(){
        base.Awake();
        iSaveableObjList = new List<ISaveable>();
    }
    
    //loops through all objects which are savable and stores them
    public void StoreCurrentSceneData(){
        foreach (ISaveable iSaveableObj in iSaveableObjList){
            iSaveableObj.ISaveableStoreScene(SceneManager.GetActiveScene().name);
        }
    }
    
    //loops through the objects which have been saved to restore them
    public void RestoreCurrentSceneData(){
        foreach (ISaveable iSaveableObj in iSaveableObjList){
            iSaveableObj.ISaveableRestoreScene(SceneManager.GetActiveScene().name);
        }
    }
}
