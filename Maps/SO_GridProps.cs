using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "so_GridProps", menuName = "Scriptable Objects/Grid Properties")]
public class SO_GridProps : ScriptableObject
{
    public SceneManagement sceneName; //scene enum
    public int gridWidth; //scene width
    public int gridHeight; //scene height
    public int originX; //origin tile X
    public int originY; //origin tile y

    [SerializeField]
    public List<GridProperty> gridPropertyList; //list of the properties for all the tiles for the inspector
}