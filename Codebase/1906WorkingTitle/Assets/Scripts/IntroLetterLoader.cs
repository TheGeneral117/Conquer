using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroLetterLoader : MonoBehaviour
{

    private Slider progress;
    [SerializeField] GameObject continueText = null;
    bool done = false;

    void Start()
    {
        progress = transform.GetChild(0).GetChild(0).GetComponent<Slider>();
        continueText.SetActive(false);
        progress.enabled = false;
    }

    private void Update()
    {
        if (done)
        {
            continueText.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
                done = false;
            }
        }
    }

    public IEnumerator LoadNewScene(int scene)
    {
        // This line waits for 1.5 seconds before executing the next line in the coroutine.
        yield return new WaitForSeconds(1.5f);

        // Async load passed in scene
        AsyncOperation async = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);

        // update progress while loading
        while (!async.isDone)
        {
            yield return null;
            progress.value = async.progress * 100f;
        }
        done = true;
    }
}
