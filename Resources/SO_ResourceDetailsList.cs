using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourceDetailsList", menuName = "Scriptable Objects/Resources/Resource Details list")]
public class SO_ResourceDetailsList : ScriptableObject
{
    [SerializeField]
    public List<ResourceDetails> resourceDetails;

    public ResourceDetails GetResourceDetails(int resourceItemID){
        return resourceDetails.Find(x => x.resourceItemID == resourceItemID);}
}

