using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager instance;
    [SerializeField] private Material causticsMat;
    [SerializeField] private Canvas loadingScreen;


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        AdjustCaustics(SceneManager.GetActiveScene().buildIndex);
        instance = this;
    }
    public void UI_SelectScene(int sceneNumber)
    {
        Instantiate(loadingScreen);
        SceneManager.LoadScene(sceneNumber);

    }

    public void UI_LoadMainMenu()
    {
        Instantiate(loadingScreen);
        SceneManager.LoadSceneAsync(0);
    }


    private void AdjustCaustics(int sceneNumber)
    {
        if (sceneNumber == 0)
        {
            causticsMat.SetFloat("_Brightness", 0);
        }
        if (sceneNumber == 1)
        {
            causticsMat.SetFloat("_Brightness", 7.64f);
        }
    }
    public void UI_RestartScene()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneIndex);
    }
    public void UI_EndScene()
    {
                SceneManager.LoadSceneAsync(3);
    }
    public void UI_QuitGame()
    {

        Application.Quit();//
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stops play mode in editor NIk_ also isnt called when a build is made! use with caution.
#else
            Application.Quit(); // Quits the application in a build
#endif
    }
}

