using UnityEngine;

public class CausticsAngle : MonoBehaviour
{

    public Material causticMaterial;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        causticMaterial.SetVector("_Angle", -this.transform.forward);
    }

    [ExecuteAlways]
    // Update is called once per frame
    void Update()
    {

    }
}
