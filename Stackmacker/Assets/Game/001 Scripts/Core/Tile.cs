using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tile : MonoBehaviour
{
    [SerializeField] private new Renderer renderer;
    [System.NonSerialized] public TileData data;

    public void Initialize()
    {
        renderer.sharedMaterial = data.tileMaterial;
    }
}
