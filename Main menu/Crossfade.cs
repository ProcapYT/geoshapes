using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Crossfade : MonoBehaviour
{
    private Animator crossfadeAnim;

    private void Start()
    {
        crossfadeAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(string sceneName)
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1f;
        }

        StartCoroutine(LoadSceneEnum(sceneName));
    }

    IEnumerator LoadSceneEnum(string sceneName)
    {
        crossfadeAnim.SetTrigger("Start");

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(sceneName);
    }
}
