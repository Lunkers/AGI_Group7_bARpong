using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(1); //Scenemanager.getcurrentscene()+1 TYP SÅ FÖR ATT GÅ TILL NÄSTA I KÖ

    }

}
