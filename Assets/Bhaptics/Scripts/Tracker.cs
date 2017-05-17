using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class Tracker : MonoBehaviour
{
    [SerializeField] public GameObject trackerAttachedModel;
    [HideInInspector] public GameObject defaultModel;

    private string DEFAULT_MODEL = "Default Model";
    private bool enableDefaultModel;

    void Awake()
    {
        if (trackerAttachedModel == null)
        {
            FindDefaultModel(DEFAULT_MODEL);
            if (defaultModel == null)
            {
                print("Can't find default model");
                return;
            }
        }
    }

    private void FindDefaultModel(string modelName)
    {
        foreach (Transform each in transform)
        {
            if (each != null && each.name == DEFAULT_MODEL)
            {
                enableDefaultModel = true;
                defaultModel = each.gameObject;
                defaultModel.SetActive(true);
                print(defaultModel);
            }
        }
    }

    /*private SteamVR_Controller.Device controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    private SteamVR_TrackedObject trackedObj;

    void Start()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    void Update()
    {
        if (controller == null)
        {
            Debug.Log("controller not initialized");
        }
        else if (controller.GetPress(SteamVR_Controller.ButtonMask.Trigger))
        {
            Debug.Log("trigger pressed");
        }
    }*/
}
