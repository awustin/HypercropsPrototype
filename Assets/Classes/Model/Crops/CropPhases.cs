using UnityEngine;

using Assets.Classes.Model.Crops;
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
        DeactivatePrevious();

        if (currentInstance != null)
        {
            return;
        }

        if (Current == CropPhase.Ready)
        {
            // Instance phase of species
            Factory.MakeCropPhaseForSpecies
            (
                CropScript.Species,
                Current,
                new Vector3
                (
                    transform.position.x,
                    transform.position.y + 0.01f,
                    transform.position.z
                ),
                currentPhaseVisuals.transform
            )
                .SetActive(true);
        }
        else
        {
            // Instance phase of farming method
            Factory.MakeCropPhaseForFarmingMethod
            (
                CropScript.FarmingMethod,
                Current,
                new Vector3
                (
                    transform.position.x,
                    transform.position.y + 0.01f,
                    transform.position.z
                ),
                currentPhaseVisuals.transform
            )
                .SetActive(true);
        }
    }

    private void DeactivatePrevious()
    {
        if (Current == CropPhase.Seed)
        {
            return;
        }

        GameObject previousPhaseVisuals = GetPhaseVisuals(Current - 1);
        GameObject previousInstance = GameObjectExtensions.GetFirstChild(previousPhaseVisuals);

        if (previousInstance != null)
        {
            previousInstance.SetActive(false);
        }
    }

    private GameObject GetPhaseVisuals(CropPhase phase)
    {
        return PhaseVisuals.transform.Find(phase.ToString()).gameObject;
    }
}
