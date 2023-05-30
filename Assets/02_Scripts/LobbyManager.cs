using System.Collections;
using UnityEngine;
using TMPro;
public class LobbyManager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI naviText;

    void Start()
    {
        StartCoroutine(TextBlink());
    }
    IEnumerator TextBlink()
    {
        while (true)
        {
            naviText.gameObject.SetActive(false);
            yield return new WaitForSeconds(1.0f);
            naviText.gameObject.SetActive(true);
            yield return new WaitForSeconds(1.0f);

        }
    }

    public void StartButtnClick()
    {
        SceneLoadderManager._instance.SceneLoadder("01_MainGameScene");
    }
}
