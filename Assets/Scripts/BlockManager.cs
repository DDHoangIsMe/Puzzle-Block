using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using TMPro;

public class BlockManager : SceneSingleton<BlockManager>
{
    [SerializeField]
    private List<Transform> _placeHolder = new List<Transform>();
    [SerializeField]
    private TextMeshProUGUI pointer;

    private List<GameObject> _shapeBlocks = new List<GameObject>();
    private int _pointCurrent = 0;

    protected override void Awake()
    {
        base.Awake();
        RenewBlocks();
    }

    public void RenewBlocks()
    {
        foreach (GameObject item in _shapeBlocks)
        {
            if (item.activeInHierarchy)
            {
                return;
            }
        }

        _shapeBlocks.Clear();
        for (int i = 0; i < _placeHolder.Count; i++)
        {
            GameObject temp;
            int randomNum = Random.Range(1, 3);
            switch (randomNum)
            {
                case 2:
                    temp = PoolingTemp.Instance.GetItemTwo();
                break;
                default: 
                    temp = PoolingTemp.Instance.GetItemOne();
                break;
            }
            _shapeBlocks.Add(temp);
            // temp.transform.localPosition = _placeHolder[i].position;
            temp.GetComponent<BlockInteract>().InitBlock(_placeHolder[i].position);
        }
    }
    
    public void AddPoint()
    {
        _pointCurrent++;
        pointer.text = _pointCurrent.ToString();
    }
}