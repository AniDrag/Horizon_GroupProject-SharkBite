using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Settings : MonoBehaviour
{
    private int frameCount = 0;
    private float totalTime = 0f;
    private float minFPS = float.MaxValue;
    private float maxFPS = 0f;
    private float currentFPS = 0f;

    [SerializeField] private TextMeshProUGUI fpsDisplay; // Assign in inspector if you want UI output

    void Start()
    {
        SetVSync(false); // Default VSync ON
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        currentFPS = 1f / Time.unscaledDeltaTime;

        frameCount++;
        totalTime += Time.unscaledDeltaTime;

        if (currentFPS < minFPS) minFPS = currentFPS;
        if (currentFPS > maxFPS) maxFPS = currentFPS;

        if (totalTime >= 0.5f)
        {
            float avgFPS = frameCount / totalTime;
            string fpsText = $"FPS: {avgFPS:F1} (Min: {minFPS:F1}, Max: {maxFPS:F1})";

            if (fpsDisplay != null)
                fpsDisplay.text = fpsText;
            else
                Debug.Log(fpsText);

            // Reset counters
            frameCount = 0;
            totalTime = 0f;
            minFPS = float.MaxValue;
            maxFPS = 0f;
        }
    }

    private void SetVSync(bool enabled)
    {
        QualitySettings.vSyncCount = enabled ? 1 : 0;
        Debug.Log("Vsync is " + enabled);
    }

    public void ToggleVSync()
    {
        SetVSync(QualitySettings.vSyncCount == 0);
    }
}