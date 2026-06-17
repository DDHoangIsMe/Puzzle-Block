using UnityEngine;
using System;
using System.Collections.Generic;
using DG.Tweening;

public class GridCell : MonoBehaviour
{
    [SerializeField]
    private Material _choosePlateMat, _defaultPlateMat, _gainBlockMat, _defaultBlockMat;
    [SerializeField]
    private GameObject _plate, _blockShow;
    [SerializeField]
    private float _delayTime = 0.3f, _fadeTime = 0.15f;

    public IntPair Id { get; private set; } = new IntPair();
    public bool Available = true;
    public bool IsChoosing = false;

    private Renderer _renderer;

    void Start()
    {
        _renderer = _plate.GetComponent<Renderer>();
        SetState(true);
    }

    public void InitId(int x, int y)
    {
        Id.IdX = x;
        Id.IdY = y;
    }

    public void ShowBlock()
    {
        SetState(false);
        DefaultColor();
        IsChoosing = false;
    }

    public void HideBlock(IntPair center)
    {
        int maxVal = Math.Max(Math.Abs(center.IdX - Id.IdX), Math.Abs(center.IdY - Id.IdY));
        _blockShow.transform.DOScale(0, _delayTime).SetDelay(maxVal * _fadeTime).OnComplete(() =>
        {
            SetState(true);
            _blockShow.transform.localScale = Vector3.one * 0.9f;
            BlockManager.Instance.AddPoint();
        });
    }

    public void ChooseColor()
    {
        _renderer.material = _choosePlateMat;
        IsChoosing = true;
    }

    public void GainColor()
    {
        _blockShow.GetComponent<Renderer>().material = _gainBlockMat;
    }

    public void NoBlockColor()
    {
        _blockShow.GetComponent<Renderer>().material = _defaultBlockMat;
    }

    public void DefaultColor()
    {
        _renderer.material = _defaultPlateMat;
        IsChoosing = false;
    }

    private void SetState(bool active)
    {
        Available = active;
        _blockShow.SetActive(!active);
    }
}
