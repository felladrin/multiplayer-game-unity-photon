using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuCanvasController : MonoBehaviour {
    public void OnBlurTextField(string value)
    {
        GlobalStorage.Save("PlayerName", value);
    }

    public void OnClickStartGameButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
