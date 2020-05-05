using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*Bailey*/
public class MainMenu : MonoBehaviour { 

    public void PlayGame()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
     #if UNITY_EDITOR
          UnityEditor.EditorApplication.isPlaying = false;
     #else
          Application.Quit();
     #endif
     }
}
