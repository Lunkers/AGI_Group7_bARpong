using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void StartNonARDemo()
    {
        SceneManager.LoadScene(1); //Numret här är numret i queuen under build settings

    }
    public void StartArSwipeDemo()
    {
        SceneManager.LoadScene(2); 

    }
    public void StartArButtonDemo()
    {
        SceneManager.LoadScene(3);

    }
}
