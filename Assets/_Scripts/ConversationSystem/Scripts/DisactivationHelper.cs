using UnityEngine;

public class DisactivationHelper : MonoBehaviour
{
    
    void Start() => DisactivationObject();
    public void DisactivationObject() => gameObject.SetActive(false);

}
