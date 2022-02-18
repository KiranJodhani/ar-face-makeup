using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loader : MonoBehaviour
{
    [SerializeField]
    private GameObject m_Loader = null;

    public static Loader Instance = null;

    private void Start()
    {
        Instance = this;
    }

    public void Show()
    {
        m_Loader.SetActive(true);
    }

    public void Hide()
    {
        m_Loader.SetActive(false);
    }
}
