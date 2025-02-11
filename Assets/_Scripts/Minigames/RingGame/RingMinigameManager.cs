using System.Collections.Generic;
using UnityEngine;

public class RingMinigameManager : MonoBehaviour
{
    public static RingMinigameManager Instance;
    [SerializeField] private List<GameObject> _ringList = new List<GameObject>();

    private GameObject _ringActive = null;

    private void Awake() => Instance = this;

    private void Start()
    {
        _ringActive = _ringList[0];
        _ringActive.SetActive(true);
    }

    public void NextRing()
    {
        if (_ringActive == _ringList[2])
        {
            _ringActive.SetActive(false);
            _ringActive = _ringList[0];
            _ringActive.SetActive(true);
        }
        else
        {
            _ringActive.SetActive(false);
            _ringActive = _ringList[_ringList.IndexOf(_ringActive) + 1];
            _ringActive.SetActive(true);
        }
    }

}
