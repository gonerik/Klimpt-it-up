using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Intertables;
using JetBrains.Annotations;
using UnityEngine;

public class CameraTargetSelector : MonoBehaviour
{
    public static CameraTargetSelector Instance;
    private CinemachineTargetGroup targetGroup;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
            Debug.LogError("There is more than one instance of CameraTargetSelector");
        }
        targetGroup = GetComponent<CinemachineTargetGroup>();
    }

    public void setTargetGroup(Transform target)
    {
        targetGroup.AddMember(target,3f,45f);
    }
}
