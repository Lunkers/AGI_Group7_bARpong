using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwithToMainMenu : MonoBehaviour
{
    public void SwitchScene()
    {
        gameObject.SetActive(false);
        SceneManager.LoadScene("MainMenu");
    }
}
