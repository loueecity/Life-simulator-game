using UnityEngine;
[ExecuteAlways]

public class GenGUID : MonoBehaviour
{
    [SerializeField]
    private string _gUID = ""; //globally unique identifiers 
    //means that the specific objects are saved by the GUIDs

    //get setters for the GUID
    public string GUID { get => _gUID; set => _gUID = value; }

    
    //runs while in editor 
    //checks if the GUI is empty, if it does then the GUI is assigned
    private void Awake(){
        if (!Application.IsPlaying(gameObject)){
            if (_gUID == ""){
                //gives random unique ID to the objects
                _gUID = System.Guid.NewGuid().ToString();
            }
        }
    }
}
