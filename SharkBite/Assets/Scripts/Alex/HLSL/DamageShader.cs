using UnityEngine;
using System.Collections;

public class DamageShader : MonoBehaviour
{
    [Header("========== Taking damage effect settings ==========")]
    [SerializeField] private Material sharedMat;

    [Tooltip("How much red to add in percentage")][SerializeField][Range(0f, 1f)] private float redAmount = .4f;
    [Tooltip("How fast should this effect last")][SerializeField][Min(0.1f)] private float speedMultiplier = 1f;
    [Tooltip("How long should I make the effect last")][SerializeField] private float timeForRed = 1f;

    SpriteRenderer sr;
    MaterialPropertyBlock block;
    Texture2D myTexture;

    private void Awake()
    {
        if (sharedMat != null)
        {
            sr = GetComponentInChildren<SpriteRenderer>();
            if (sr == null)
                Debug.LogWarning("I don't have a sprite renderer");
            block = new MaterialPropertyBlock();
            myTexture = sr.sprite.texture;
            block.SetTexture("_MyTexture", myTexture);
            sr.sharedMaterial = sharedMat;
            sr.SetPropertyBlock(block);
        }
        else
            Debug.LogWarning("I don't have reference to the shader material");
    }

    public IEnumerator DamageAnimation()
    {
        float startTime = Time.time;
        float endTime = startTime + timeForRed;


        while (Time.time < endTime)
        {
            float value = Mathf.PingPong(Time.time / (timeForRed / 2f), 1f);
            float t = Mathf.Lerp(0, redAmount, value);

            sr.GetPropertyBlock(block);
            block.SetFloat("_Factor", t);
            sr.SetPropertyBlock(block);

            yield return null;
        }
        block.SetFloat("_Factor", 0);
    }
}
