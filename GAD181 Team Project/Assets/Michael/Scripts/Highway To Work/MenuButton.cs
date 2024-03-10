using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    public LevelChangerScript levelchanger;
    public void Menu()
    {
        levelchanger.LoadNewLevel(0);
    }
}
