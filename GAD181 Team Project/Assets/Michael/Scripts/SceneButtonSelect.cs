using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButtonSelect : MonoBehaviour
{
    [SerializeField] private LevelChangerScript LevelChanger;
    [SerializeField] private int sceneLoadNumber;


    // Start is called before the first frame update
    public void LoadSelectedScene()
    {
        LevelChanger.LoadNewLevel(sceneLoadNumber);
    }
}
