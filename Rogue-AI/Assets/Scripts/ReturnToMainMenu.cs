using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMainMenu : MonoBehaviour
{
    public void OnButtonLoadMainMenu()
    {
        SceneManager.LoadScene("0_StartMenuScene");
    }
}
