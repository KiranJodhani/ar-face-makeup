using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using GoogleARCore.Examples.AugmentedFaces;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEditor;

public enum FilterType
{
    None = -1,
    Makeup,
    Jwellery,
    HatsAndCrown,
    GogglesAndEyeMask,
    Other
}

public enum FilterPanels
{
    None = -1,
    MakeupFIlterPanel,
    JwelleryFilterPanel,
    HatsAndCrown,
    GogglesAndEyeMask,
    OtherFaceFilters
}

public class GameHUDManager : MonoBehaviour
{
    private const string IMAGE_NAME = "Makeup.png";
    private const string SHARE_MESSAGE = "Now It is so easy to be beautiful, " +
                                         "Before trying a makeup on your face, why don't you try it virtually using this application? \nI have tried this Augmented Reality Face Makeup application and it's really cool! Go for it!" +
                                         "\nHere is the link, download it now! \n";

    private const string GOOGLE_PLAY_STORE_LINK = "https://play.google.com/store/apps/details?id=";

    [SerializeField]
    private Canvas m_UICanvas = default;

    [SerializeField]
    private GameObject m_FilterUI = default;

    [SerializeField]
    private GameObject m_InfoText = null;

    [SerializeField]
    private Transform m_FaceParent = null;

    [SerializeField]
    private GameObject[] m_FilterPanels = null;

    private int m_LastSelectdFilterPanel = 0;
    private bool m_IsShareClicked = false;

    private void OnEnable()
    {
        AugmentedFacesExampleController.FaceDetected += OnFaceDetected;
        AugmentedFacesExampleController.FaceNotDetected += OnFaceNotDetected;
    }

    private void OnDisable()
    {
        AugmentedFacesExampleController.FaceDetected -= OnFaceDetected;
        AugmentedFacesExampleController.FaceNotDetected -= OnFaceNotDetected;
    }

    private void OnFaceNotDetected()
    {
        m_InfoText.SetActive(true);
        m_FilterUI.SetActive(false);
    }

    private void OnFaceDetected()
    {
        m_InfoText.SetActive(false);
        m_FilterUI.SetActive(true);
    }

    private void Update()
    {
        // Exit the app when the 'back' button is pressed.
        if (Input.GetKey(KeyCode.Escape))
        {
            OnHomeBtnClicked();
        }
    }

    private void LateUpdate()
    {
        if (m_IsShareClicked)
        {
            StartCoroutine(TakeSS());
        }
    }

    public void OnHomeBtnClicked()
    {
        SceneManager.LoadSceneAsync(SceneName.MainMenuScene.ToString());
    }

    public void OnShare()
    {
        m_IsShareClicked = true;
    }

    private IEnumerator TakeSS()
    {
        m_IsShareClicked = false;
        NativeShare nativeShare = new NativeShare();

        yield return new WaitForSeconds(0.3f);
        m_UICanvas.gameObject.SetActive(false);
        string l_ShareMsg = SHARE_MESSAGE + GOOGLE_PLAY_STORE_LINK + Application.identifier;

        string l_Path = Path.Combine(Application.persistentDataPath, IMAGE_NAME);

        if (File.Exists(l_Path))
        {
            File.Delete(l_Path);
        }

        yield return new WaitForEndOfFrame();
        var texture = ScreenCapture.CaptureScreenshotAsTexture();

        //enable canvas after taking SS
        m_UICanvas.gameObject.SetActive(true);

        byte[] bytes = texture.EncodeToPNG();

        File.WriteAllBytes(l_Path, bytes);

        while (!File.Exists(l_Path))
        {
            yield return new WaitForEndOfFrame();
        }

        // share image
        if (File.Exists(l_Path))
        {
            nativeShare.AddFile(l_Path).SetText(l_ShareMsg).Share();
        }
    }

    public void OnFilterPanelSelection(int filterPanelIndex)
    {
        if (m_LastSelectdFilterPanel == filterPanelIndex)
        {
            return;
        }

        m_LastSelectdFilterPanel = filterPanelIndex;

        for (int i = 0; i < m_FilterPanels.Length; i++)
        {
            m_FilterPanels[i].SetActive(false);
        }

       GameObject l_SelectedPanel =  m_FilterPanels[filterPanelIndex];
       l_SelectedPanel.SetActive(true);
    }
}
