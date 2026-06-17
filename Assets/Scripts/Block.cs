using UnityEngine;
using System;
using System.Collections.Generic;
using DG.Tweening;
using System.Linq;

[Serializable]
public class IntPair
{
    public int IdX;
    public int IdY;
}

public class Block : MonoBehaviour
{
    [SerializeField]
    private List<IdBlock> _blockShow = new List<IdBlock>();

    private List<GameObject> _gridContacts = new List<GameObject>();
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("BoardGrid"))
        {
            _gridContacts.Add(other.gameObject);
            UpdateToBoard();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("BoardGrid"))
        {
            _gridContacts.Remove(other.gameObject);
            if (gameObject.activeInHierarchy)
                UpdateToBoard();
        }
    }

    public void UpdateToBoard()
    {
        if (_gridContacts.Count == 0)
        {
            GridManager.Instance.IgnorePlacer();
        }
        else
        {
            GridManager.Instance.ChooseGrid(_gridContacts.First().GetComponent<GridCell>().Id, _blockShow);
        }
    }
}
