using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsButton : MonoBehaviour
{
    public GameObject main_Scene;
    public GameObject credits_Scene;


    public void Opencredits()
    {
        credits_Scene.SetActive(true);
        main_Scene.SetActive(false);
    }

    public void CloseCredits()
    {
        credits_Scene.SetActive(false);
        main_Scene.SetActive(true);
    }
}
