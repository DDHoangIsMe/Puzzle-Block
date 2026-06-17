using UnityEngine;


public class IdBlock : MonoBehaviour
{
    public IntPair Id;

    [SerializeField]
    private Material _defaultM, _gainM;

    public void ChangeColorGainAble()
    {
        gameObject.GetComponent<Renderer>().material = _gainM;
    }

    public void ChangeColorNormal()
    {
        gameObject.GetComponent<Renderer>().material = _defaultM;
    }
}