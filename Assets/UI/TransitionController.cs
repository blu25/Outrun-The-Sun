using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TransitionController : MonoBehaviour
{
	public Image fade;
	float fadeAmt = 1f;

	bool isTransitioning = false;
    string SceneToLoad;

    // Start is called before the first frame update
    void Start()
    {
        DynamicGI.UpdateEnvironment();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTransitioning)
		{
			if (fadeAmt > 0)
				fadeAmt -= Time.deltaTime;
		} else
		{
			if (fadeAmt < 1)
				fadeAmt += Time.deltaTime;
            else
                SceneManager.LoadScene(SceneToLoad, LoadSceneMode.Single);
        }


        fade.color = new Color(0, 0, 0, fadeAmt);
    }

    public void StartTransition(string sceneName) {
        SceneToLoad = sceneName;
        isTransitioning = true;
    }

    public bool IsTransitioning() {
        return isTransitioning;
    }


}
