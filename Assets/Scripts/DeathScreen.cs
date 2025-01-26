using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{

    [SerializeField] private string sceneName = "SampleScene";

    public void ReloadScene()
    {
        // Get the active scene and reload it
        SceneManager.LoadScene(sceneName);
    }

}
