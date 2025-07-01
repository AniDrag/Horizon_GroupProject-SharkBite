using UnityEngine;

public class FceCamera : MonoBehaviour
{
    [SerializeField]    [Range(30, 60)] int rotation = 50;
    private void Start()
    {
        transform.eulerAngles = Vector3.zero;
        transform.rotation = Quaternion.Euler(rotation, 0f, 0f);       
    }
}
