using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public Animator fadeAnimator;

    private int sceneIndexToLoad;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeToNextScene()
    {
        this.FadeToScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void FadeToScene(int index)
    {
        this.sceneIndexToLoad = index;
        this.fadeAnimator.SetTrigger("FadeOut");
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(this.sceneIndexToLoad);
    }
}
