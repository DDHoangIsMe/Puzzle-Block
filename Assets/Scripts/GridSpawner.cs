using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;

public class GridSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject blockPrefab; // Prefab khối 3D
    [SerializeField]
    private int width = 8, height = 8;
    [SerializeField]
    private float spacing = 1.1f; // khoảng cách giữa các khối

    void Start()
    {
        SpawnGrid();
    }

    void SpawnGrid()
    {
        List<List<GridCell>> AllBlockGrids = new List<List<GridCell>>();
        for (int x = 0; x < width; x++)
        {
            List<GridCell> newRow = new List<GridCell>(); 
            for (int y = 0; y < height; y++)
            {
                Vector3 pos = new Vector3(x * spacing, y * spacing, 1) + transform.position;
                GridCell newCell = Instantiate(blockPrefab, pos, Quaternion.identity, transform).GetOrAddComponent<GridCell>();
                newCell.InitId(x, y);
                newRow.Add(newCell);
            }
            AllBlockGrids.Add(newRow);
        }
        GridManager.Instance.InitGrid(AllBlockGrids);

        //Destroy(this);
    }
}
