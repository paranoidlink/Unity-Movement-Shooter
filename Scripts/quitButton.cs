using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class quitButton : MonoBehaviour
{
    public void QuitGame()
    {
        #if UNITY_EDITOR
            // If the game is running in the editor, stop playing
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            // If the game is running as a standalone build, quit the application
            Application.Quit();
        #endif
    }
}
