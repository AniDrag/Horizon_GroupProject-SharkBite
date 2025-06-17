using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Resolution : MonoBehaviour
{
    [SerializeField] private CustomRenderTexture texture;

    private RawImage _RawImage;
    private CustomRenderTexture _resizedTexture;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _RawImage = GetComponent<RawImage>();
        int width = GameManager.instance._displayResolution.x;
        int height = GameManager.instance._displayResolution.y;
        Debug.Log(width + " " +  height);   
        _resizedTexture = new CustomRenderTexture(width, height, texture.graphicsFormat);

        _resizedTexture.material = texture.material;
        _resizedTexture.updateMode = texture.updateMode;
        _resizedTexture.initializationMode = texture.initializationMode;
        _resizedTexture.initializationSource = texture.initializationSource;
        _resizedTexture.initializationColor = texture.initializationColor;

        _resizedTexture.Create();

        texture = _resizedTexture;

        texture.Update();

        if (_RawImage != null)
        {
            RectTransform rt = _RawImage.rectTransform;
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
