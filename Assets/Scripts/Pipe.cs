using System.Collections;
using System.Collections.Generic;
using Intertables;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pipe : MonoBehaviour
{
    private Tilemap _tilemap;
    private GameObject currentPuddle;
    public void SpawnPuddle()
    {
        if (currentPuddle != null) return;
        _tilemap = CharacterController2D.Instance.getTileMap();
        Vector3Int cellPosition = _tilemap.WorldToCell(transform.position);
        cellPosition.x += 1;
        Vector3 tileCenterPosition = _tilemap.GetCellCenterWorld(cellPosition);
        GameObject puddle = CharacterController2D.Instance.getPuddle();
        currentPuddle = Instantiate(puddle, tileCenterPosition, Quaternion.identity);
    }
}
