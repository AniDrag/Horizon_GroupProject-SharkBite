using UnityEngine;

[RequireComponent(typeof(Camera))]
public class DepthTextureEnabler : MonoBehaviour
{
    void Start()
    {
        this.gameObject.GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
    }
}
