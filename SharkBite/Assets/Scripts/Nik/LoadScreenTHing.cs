
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class LoadScreenTHing : MonoBehaviour
{
    public Slider LoadingBarSlider;
    [SerializeField] private float loadingSpeed = 1f;
    public GameObject button;

    float fillSpeed;
    private bool loadingDone = false;
    private void Start()
    {
        LoadingBarSlider.value = 0;
        fillSpeed = LoadingBarSlider.maxValue / loadingSpeed;
        button.SetActive(false);
    }

    void Update()
    {
        if (loadingDone) return;

        if (LoadingBarSlider.value < LoadingBarSlider.maxValue - 0.01f)
        {
            LoadingBarSlider.value += Time.deltaTime * fillSpeed;
        }
        else if(LoadingBarSlider.value >= LoadingBarSlider.maxValue)
        {
            LoadingBarSlider.value = LoadingBarSlider.maxValue;
            loadingDone = true;
            button.SetActive(true);
        }
    }
}
