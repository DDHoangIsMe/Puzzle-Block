using UnityEngine;
using System;
using System.Collections.Generic;
using DG.Tweening;

public class BlockInteract : MonoBehaviour
{
    [SerializeField]
    private float _maxAngle = 30, _decreaseAngle = 10;
    [SerializeField]
    private float _minSpeed = 5, _maxSpeed = 40, _moveSpeed = 14;

    private List<GameObject> _allBlocks = new List<GameObject>();
    private Vector3 _offSet, _targetPos, _lastPost, _orgPos;
    private Vector3 _velocity;
    private bool _allowDrag = true;

    void Awake()
    {
        foreach (Transform child in transform)
        {
            _allBlocks.Add(child.gameObject);
        }
        _targetPos = transform.position;
        _orgPos = transform.position;
    }

    public void InitBlock(Vector3 pos)
    {
        _targetPos = pos;
        transform.position = pos;
        _orgPos = pos;
    }

    void OnMouseDown()
    {
        if (GridManager.Instance.IsBusy)
        {
            _allowDrag = false;
            return;
        }
        transform.DOKill();
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _offSet = transform.position - new Vector3(mousePos.x, mousePos.y, transform.position.z);
    }

    void OnMouseDrag()
    {
        if (!_allowDrag)
        {
            return;
        }
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _targetPos = new Vector3(mousePos.x, mousePos.y, transform.position.z) + _offSet;
    }

    void OnMouseUp()
    {   
        if (!_allowDrag)
        {
            _allowDrag = true;
            return;
        }
        bool state = GridManager.Instance.OnDropBoard();
        if (!state)
        {
            _targetPos = _orgPos;
        }
        gameObject.SetActive(!state);
        
        BlockManager.Instance.RenewBlocks();
    }

    void Update()
    {
        if (_targetPos == transform.position)
        {
            return;
        }
        
        float dynamicSpeed = Mathf.Clamp(Vector3.Distance(transform.position, _targetPos) * _moveSpeed, _minSpeed, _maxSpeed);

        transform.position = Vector3.Lerp(transform.position, _targetPos, Time.deltaTime * dynamicSpeed);

        _velocity = (transform.position - _lastPost) / (Time.deltaTime * _decreaseAngle);
        _lastPost = transform.position;

        if (_velocity.magnitude > 0.01f)
        {
            float angleZ = Mathf.Clamp(_velocity.x * _maxAngle, -_maxAngle, _maxAngle);
            float angleX = Mathf.Clamp(-_velocity.y * _maxAngle, -_maxAngle, _maxAngle);

            Quaternion targetRot = Quaternion.Euler(angleX, angleZ, 0);
            foreach (GameObject item in _allBlocks)
            {
                item.transform.rotation = targetRot;
            }
        }
        else
        {
            foreach (GameObject item in _allBlocks)
            {
                item.transform.rotation = Quaternion.identity;
            }
        }
    }

    void OnDisable()
    {
        transform.DOKill();
    }
}
