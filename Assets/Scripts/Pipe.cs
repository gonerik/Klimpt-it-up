using System.Collections;
using System.Collections.Generic;
using Intertables;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pipe : MonoBehaviour
{
    private Tilemap tilemap;
    
    void Start()
    {
        tilemap = CharacterController2D.Instance.getTileMap();
    }
    

    public void SpawnPuddle()
    {
        Vector3Int cellPosition = tilemap.WorldToCell(transform.position);
        cellPosition.x += 1;
        TileBase tileAtPosition = tilemap.GetTile(cellPosition);
    }
}
