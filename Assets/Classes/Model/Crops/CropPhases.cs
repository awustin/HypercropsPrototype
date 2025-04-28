using System.Collections.Generic;
using UnityEngine;

public class CropPhases : MonoBehaviour
{
    public ObjectFactory Factory;
    public Crop CropScript;
    public CropPhase Current = CropPhase.Seed;
    public GameObject PhaseVisuals;

    private CropPhase _currentTracker;

    void Start()
    {
        Factory = ObjectFactory.Instance;
        UpdateVisuals();
    }

    void Update()
    {
        TrackVariables();
    }

    public void NextPhase()
    {
        if (Current == CropPhase.Ready)
        {
            return;
        }

        Current ++;
    }

    public bool IsLast()
    {
        return Current == CropPhase.Ready;
    }

    private void TrackVariables()
    {
        if (_currentTracker != Current)
        {
            _currentTracker = Current;
            UpdateVisuals();
        }
    }

    private void UpdateVisuals()
    {
        GameObject phaseVisuals = GetPhaseVisuals();

        GameObject instance = Factory.MakeCropPhase(
            CropScript.CropName,
            Current.ToString(),
            transform.position,
            phaseVisuals.transform
        );
    }

    private GameObject GetPhaseVisuals()
    {
        return PhaseVisuals.transform.Find(Current.ToString()).gameObject;
    }
}
