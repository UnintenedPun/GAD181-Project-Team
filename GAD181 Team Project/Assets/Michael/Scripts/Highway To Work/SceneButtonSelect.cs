using UnityEngine;

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
