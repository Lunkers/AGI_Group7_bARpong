using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ReturnToMainMenu : MonoBehaviour
{
    // Update is called once per frame
    public void SwitchScene()
    {
        gameObject.SetActive(false);
        SceneManager.LoadScene("MainMenu");
    }
}
