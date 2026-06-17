using System.Collections.Generic;
using UnityEngine;

public class PoolingTemp : SceneSingleton<PoolingTemp>
{
    [SerializeField]
    private GameObject _shapeOne, _shapeTwo;

    private List<GameObject> _listShapeOne = new List<GameObject>();
    private List<GameObject> _listShapeTwo = new List<GameObject>();

    public GameObject GetItemOne()
    {
        foreach (GameObject item in _listShapeOne)
        {
            if (!item.activeSelf)
            {
                item.SetActive(true);
                return item;
            }
        }
        GameObject newShape = Instantiate(_shapeOne);
        newShape.transform.SetParent(transform);
        _listShapeOne.Add(newShape);
        return newShape;
    }

    public GameObject GetItemTwo()
    {
        foreach (GameObject item in _listShapeTwo)
        {
            if (!item.activeSelf)
            {
                item.SetActive(true);
                return item;
            }
        }
        GameObject newShape = Instantiate(_shapeTwo);
        newShape.transform.SetParent(transform);
        _listShapeTwo.Add(newShape);
        return newShape;
    }
}