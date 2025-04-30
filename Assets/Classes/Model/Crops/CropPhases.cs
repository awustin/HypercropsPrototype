using UnityEngine;
using Assets.Classes.System;

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
        GameObject currentPhaseVisuals = GetPhaseVisuals(Current);
        GameObject currentInstance = GameObjectExtensions.GetFirstChild(currentPhaseVisuals);
        DestroyPrevious();

        if (currentInstance != null)
        {
            return;
        }

        GameObject instance = Factory.MakeCropPhase(
            CropScript.CropName,
            Current,
            transform.position,
            currentPhaseVisuals.transform
        );

        instance.SetActive(true);
    }

    private void DestroyPrevious()
    {
        if (Current == CropPhase.Seed)
        {
            return;
        }

        GameObject previousPhaseVisuals = GetPhaseVisuals(Current - 1);

        GameObject previousInstance = GameObjectExtensions.GetFirstChild(previousPhaseVisuals);

        if (previousInstance != null)
        {
            Destroy(previousInstance);
        }
    }

    private GameObject GetPhaseVisuals(CropPhase phase)
    {
        return PhaseVisuals.transform.Find(phase.ToString()).gameObject;
    }
}
