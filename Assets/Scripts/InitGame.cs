using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class InitGame : MonoBehaviour
{
    public SceneChanger sceneChanger;

    // Start is called before the first frame update
    void Awake()
    {
        this.StartCoroutine(this.Init());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Init()
    {
        yield return new WaitForSeconds(3.5f);
        this.sceneChanger.FadeToNextScene();
        yield return null;
    }
}
