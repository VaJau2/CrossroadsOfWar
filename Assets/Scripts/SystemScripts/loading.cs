using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class loading : MonoBehaviour {

    public Transform Squirrel;
    AsyncOperation async;
    Scene activeScene;
    public int loadScene;
    float loadingProgress; 
    public Text myText;

    void Start()
    {
        launchLevel();
        if(Random.value < 0.15f)
        {
            Squirrel.transform.localEulerAngles = new Vector3(0,0,90);
        }
    }

    void launchLevel()
    {
        async = SceneManager.LoadSceneAsync(loadScene, LoadSceneMode.Single);
        async.allowSceneActivation = false;
    }

    void Update()
    {
        loadingProgress = async.progress * 100f;

        if (loadingProgress > 89)
        {
            myText.text = Lang.GetInstance().GetText("loading_complete");

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Scene nextScene = SceneManager.GetSceneByBuildIndex(loadScene);
                if (nextScene.IsValid())
                {
                    //activeScene = SceneManager.GetActiveScene();
                    //SceneManager.SetActiveScene(nextScene);
                    async.allowSceneActivation = true;                 
                }
                else
                {
                    Debug.Log("You need to throw your PC from the window right now!");
                }
            }
        }

        //if(loadingProgress > 99) SceneManager.UnloadSceneAsync(activeScene.buildIndex);
    }
}
