using UnityEngine;

public class EellShader : MonoBehaviour
{
    [Header("Shared Material")]
    public Material sharedMat;
    SpriteRenderer sr;
    MaterialPropertyBlock block;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        block = new MaterialPropertyBlock();

        // Make sure the SpriteRenderer is using the sharedMat
        sr.sharedMaterial = sharedMat;
        SetValue(0);
        SetBloom(1);
    }

    /// <summary>
    /// Reset values to default ones if "killed"
    /// </summary>
    private void OnDisable()
    {
        SetValue(0);
        SetBloom(1);
    }


    void ApplyProperties()
    {
        // always GetPropertyBlock first in case something else set it
        sr.GetPropertyBlock(block);
        block.SetFloat("_Value", currentValue);
        block.SetFloat("_Bloom", currentBloom);
        sr.SetPropertyBlock(block);
    }

    public float currentValue = 1f, currentBloom = 50f;
    public void SetValue(float v)
    {
        currentValue = v;
        ApplyProperties();
    }
    public void SetBloom(float b)
    {
        currentBloom = b;
        ApplyProperties();
    }
}
