using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="so_Items", menuName ="Scriptable Objects/Item/Items")]

//used for item creation
public class SO_Items : ScriptableObject
{
    [SerializeField]
    public List<ItemDetails> itemDetails;
}
