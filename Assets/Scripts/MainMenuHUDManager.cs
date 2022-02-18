using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SceneName
{
    None = -1,
    MainMenuScene,
    FaceMakeup
}

public class MainMenuHUDManager : MonoBehaviour
{
    [SerializeField]
    private Button m_PlayButton = null;

    private void Update()
    {
        // Exit the app when the 'back' button is pressed.
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void OnPlayBtnClicked()
    {
        Loader.Instance.Show();
        StartCoroutine(GoToGamePlayScreen());
    }

    private IEnumerator GoToGamePlayScreen()
    {
        m_PlayButton.interactable = false;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(SceneName.FaceMakeup.ToString());

        while (!asyncOperation.isDone)
        {
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(0.5f); //wait until animation completes
        asyncOperation.allowSceneActivation = true;
    }
}

