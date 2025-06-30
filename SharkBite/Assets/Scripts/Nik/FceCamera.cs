using UnityEngine;

public class FceCamera : MonoBehaviour
{
    private void Start()
    {
        transform.eulerAngles = Vector3.zero;
        transform.rotation = Quaternion.Euler(50, 0f, 0f);       
    }
}
