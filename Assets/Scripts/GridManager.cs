using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;

public class GridManager : SceneSingleton<GridManager>
{
    public List<List<GridCell>> AllBlockGrids;
    public bool IsBusy = false;

    [SerializeField]
    private float _delayTime = 0.4f;

    private HashSet<GridCell> _gainAbleGrids = new HashSet<GridCell>();
    private List<GridCell> _placeHighLight = new List<GridCell>();
    private List<int> rowHighLight = new List<int>();
    private List<int> colHighLight = new List<int>();
    private int _pointPre = 0;
    private IntPair _centerAnchor;

    public void InitGrid(List<List<GridCell>> param)
    {
        AllBlockGrids = param;
    }

    public bool OnDropBoard()
    {
        if (_placeHighLight.Count > 0)
        {
            PlaceBlock();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ChooseGrid(IntPair gridId, List<IdBlock> data)
    {
        IgnorePlacer();
        //Check
        foreach (IdBlock item in data)
        {
            if (!CheckPlaceAble(gridId.IdX + item.Id.IdX, gridId.IdY + item.Id.IdY))
            {
                foreach (IdBlock sub in data)
                {
                    sub.ChangeColorNormal();
                }
                IgnorePlacer();
                return;
            }
            _placeHighLight.Add(AllBlockGrids[gridId.IdX + item.Id.IdX][gridId.IdY + item.Id.IdY]);
        }
        _centerAnchor = gridId;
        CheckGainAble();

        //Hight Block
        foreach (IdBlock block in data)
        {
            if (colHighLight.Contains(block.Id.IdX - gridId.IdX) || rowHighLight.Contains(block.Id.IdY - gridId.IdY))
            {
                block.ChangeColorGainAble();
            }
            else
            {
                block.ChangeColorNormal();
            }
        }
    }

    private void CheckGainAble()
    {
        rowHighLight.Clear();
        colHighLight.Clear();
        HashSet<int> listRow = new HashSet<int>();
        HashSet<int> listCol = new HashSet<int>();
        //HighLight
        foreach (GridCell item in _placeHighLight)
        {
            item.ChooseColor();
            listRow.Add(item.Id.IdY);
            listCol.Add(item.Id.IdX);
        }
        //Row
        foreach (int row in listRow)
        {
            List<GridCell> temp = GetRow(row);
            bool checker = temp.Count > 0;
            foreach (GridCell item in temp)
            {
                if (item.Available && !item.IsChoosing)
                {
                    checker = false;
                }
            }
            if (checker)
            {
                foreach (var cell in temp)
                {
                    _gainAbleGrids.Add(cell);
                    cell.GainColor();
                }
                rowHighLight.Add(row);
            }    
        }
        //Col
        foreach (int col in listCol)
        {
            List<GridCell> temp = GetCol(col);
            bool checker = temp.Count > 0;
            foreach (GridCell item in temp)
            {
                if (item.Available && !item.IsChoosing)
                {
                    checker = false;
                }
            }
            if (checker)
            {
                foreach (var cell in temp)
                {
                    _gainAbleGrids.Add(cell);
                    cell.GainColor();
                }
                colHighLight.Add(col);
            }    
        }
    }

    private bool CheckPlaceAble(int x, int y)
    {
        if (x >= 0 && x < AllBlockGrids.Count && y >= 0 && y < AllBlockGrids[x].Count)
        {
            return AllBlockGrids[x][y].Available;
        }
        return false;
    }

    private List<GridCell> GetCol(int x)
    {
        return AllBlockGrids[x];
    }

    private List<GridCell> GetRow(int y)
    {
        List<GridCell> temp = new List<GridCell>();
        foreach (List<GridCell> items in AllBlockGrids)
        {
            temp.Add(items[y]);
        }
        return temp;
    }

    private void PlaceBlock()
    {

        foreach (GridCell item in _placeHighLight)
        {
            item.ShowBlock();
        }

        if (_gainAbleGrids.Count > 0)
        {
            IsBusy = true;
            Invoke("GainBlocks", _delayTime);
        }

    }

    private void GainBlocks()
    {
        foreach (GridCell item in _gainAbleGrids)
        {
            _pointPre++;
            item.HideBlock(_centerAnchor);
        }
        Invoke("ReadyForNew", 1.5f);
    }

    private void ReadyForNew()
    {
        IgnorePlacer();
        IsBusy = false;
    }

    public void IgnorePlacer()
    {Debug.Log("delete here");
        foreach (GridCell item in _gainAbleGrids)
        {
            item.NoBlockColor();
        }
        foreach (GridCell item in _placeHighLight)
        {
            item.DefaultColor();
        }

        _gainAbleGrids.Clear();
        _placeHighLight.Clear();

        _pointPre = 0;
    }
}