using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoadderManager : MonoBehaviour
{
    static SceneLoadderManager instance;
    enum eLoadingState
    {
        NONE = 0,
        ING,
    }

    [SerializeField]
    private Canvas ManagerCanvas;
    [SerializeField]
    private Image loaddingBar;

    [SerializeField]
    GameObject Ship;

    string nextScene;

    private eLoadingState eCurrnetState;

    public static SceneLoadderManager _instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        instance = this;
        eCurrnetState = eLoadingState.NONE;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        SceneLoadder("02_LobbyScene");
    }


    private void Update()
    {
        switch (eCurrnetState)
        {
            case eLoadingState.NONE:
                ManagerCanvas.gameObject.SetActive(false);
                loaddingBar.gameObject.SetActive(false);
                Ship.gameObject.SetActive(false);
                ManagerCanvas.sortingLayerName = "";
                break;
            case eLoadingState.ING:
                ManagerCanvas.gameObject.SetActive(true);
                loaddingBar.gameObject.SetActive(true);
                Ship.gameObject.SetActive(true);
                ManagerCanvas.sortingLayerName = "UI";
                break;
        }
    }

    public void SceneLoadder(string sceneName)
    {
        nextScene = sceneName;
        eCurrnetState = eLoadingState.ING;
        StartCoroutine(LoadSceneManager());
    }

    IEnumerator LoadSceneManager()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(nextScene);
        async.allowSceneActivation = false;
        float timer = 0f;

        while(!async.isDone)
        {
            yield return null;
            if (async.progress < 0.9f)
            {
                loaddingBar.fillAmount = async.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime;

                loaddingBar.fillAmount = Mathf.Lerp(0.9f, 1.0f, timer);
                if (loaddingBar.fillAmount >= 1f)
                {
                    async.allowSceneActivation = true;
                }
            }

        }
        eCurrnetState = eLoadingState.NONE;
    }

}
