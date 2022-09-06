using UnityEngine;
using UnityEngine.SceneManagement;

//sends player back to hero select screen
public class GameOverBackButton : MonoBehaviour
{
    public void OnBackButtonPressed()
    {
        SceneManager.LoadScene("Title");
    }
}
