using System;
using System.Collections;
using System.Collections.Generic;
using GoogleARCore.Examples.AugmentedFaces;
using UnityEngine;
using UnityEngine.UI;

public class IFilterSelection : MonoBehaviour
{
    [SerializeField]
    private Transform m_FaceParent = null;

    [SerializeField]
    private GameObject m_FilterGO = null;

    [SerializeField]
    private FilterType m_FilterType = FilterType.None;

    private Toggle m_Toggle = null;
    private GameObject m_LastFilter = null;

    private void Awake()
    {
        m_Toggle = GetComponent<Toggle>();
        m_Toggle.onValueChanged.AddListener(OnToggleChanged);
    }

    private void OnEnable()
    {
        m_Toggle.isOn = false;
        OnToggleChanged(false);
    }

    private void OnToggleChanged(bool isOn)
    {
        if (isOn)
        {
            if (m_LastFilter == m_FilterGO)
            {
                return;
            }

            if (m_LastFilter == null)
            {
                RemoveOtherFilters();
                m_LastFilter = Instantiate(m_FilterGO, m_FaceParent);
            }
        }
        else
        {
            if (m_LastFilter != null)
            {
                Destroy(m_LastFilter);
            }
        }
    }

    private void RemoveOtherFilters()
    {
        ARCoreAugmentedFaceRig[] l_FaceFiltersGOs = m_FaceParent.GetComponentsInChildren<ARCoreAugmentedFaceRig>();
        ARCoreAugmentedFaceMeshFilter[] l_FaceMeshFiltersGOs = m_FaceParent.GetComponentsInChildren<ARCoreAugmentedFaceMeshFilter>();

        for (int i = 0; i < l_FaceFiltersGOs.Length; i++)
        {
            if (l_FaceFiltersGOs[i].CompareTag(m_FilterType.ToString()))
            {
                DestroyImmediate(l_FaceFiltersGOs[i].gameObject);
            }
        }

        for (int i = 0; i < l_FaceMeshFiltersGOs.Length; i++)
        {
            if (l_FaceMeshFiltersGOs[i].CompareTag(m_FilterType.ToString()))
            {
                DestroyImmediate(l_FaceMeshFiltersGOs[i].gameObject);
            }
        }
    }
}
