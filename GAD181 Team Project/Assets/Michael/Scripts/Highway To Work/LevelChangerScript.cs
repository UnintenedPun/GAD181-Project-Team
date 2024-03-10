using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChangerScript : MonoBehaviour
{
    public Animator Animator;

    private int LevelToLoad;

    public void LoadNewLevel(int levelToload)
    {
        LevelToLoad = levelToload;
        Animator.SetTrigger("FadeOut");
    }

    public void OnAnimationComplete()
    {
        SceneManager.LoadScene(LevelToLoad);
    }
}
