using UnityEngine;

using Assets.Hypercrops.Common.Enums;
using Assets.Hypercrops.Model.Crops;
using Assets.Hypercrops.System;

public class CropPhases : MonoBehaviour
{
    public ObjectFactory Factory;
    public Crop CropScript;
    public CropHealth Health;
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
        if
        (
            Current == CropPhase.Ready ||
            Current == CropPhase.Dead ||
            Current == CropPhase.Harvested
        )
        {
            return;
        }

        Current ++;
    }

    public bool IsLast()
    {
        return Current == CropPhase.Ready;
    }

    public void SetHarvested()
    {
        Current = CropPhase.Harvested;
    }

    private void TrackVariables()
    {
        if (_currentTracker != Current)
        {
            _currentTracker = Current;
            UpdateVisuals();

            if (Current == CropPhase.Ready)
            {
                Health.SetReadyDamageFactor();
            }
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
