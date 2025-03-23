using UnityEngine;
using System.Collections.Generic;

public enum CropPhase
{
    Ghost,
    Seed,
    Growing,
    Ready,
    Dead
}

public class Crop : MonoBehaviour
{
    [HideInInspector] public GameEventSender Sender;
    [HideInInspector] public ObjectFactory Factory;
    public string Name;
    public bool IsAllowed;
    public CropPhase CurrentPhase;
    public int CurrentSequenceIndex;
    public List<CropPhase> PhaseSequence = new List<CropPhase>();
    public GameObject CropPhaseInstance;

    public Crop(string name)
    {
        Name = name;
        CurrentSequenceIndex = 0;
        CurrentPhase = PhaseSequence[CurrentSequenceIndex];
        IsAllowed = false;
    }

    void Start()
    {
        Factory = ObjectFactory.Instance;
        PhaseSequence.Add(CropPhase.Ghost);
        PhaseSequence.Add(CropPhase.Seed);
        PhaseSequence.Add(CropPhase.Growing);
        PhaseSequence.Add(CropPhase.Ready);
        // PhaseSequence.Add(CropPhase.Dead);

        CropPhaseInstance = Factory.MakeCropPhase(Name, "Ghost", transform.position, transform);
    }

    public void SetAllowed(bool value)
    {
        if (CurrentPhase != CropPhase.Ghost)
        {
            return;
        }

        if (IsAllowed && !value)
        {
            Factory.SetGhostMaterial(CropPhaseInstance, false);
            IsAllowed = false;
            return;
        }

        if (!IsAllowed && value)
        {
            Factory.SetGhostMaterial(CropPhaseInstance, true);
            IsAllowed = true;
            return;
        }
    }

    public void IncrementCropPhase(Vector3? pos)
    {
        if (CurrentSequenceIndex + 1 >= PhaseSequence.Count)
        {
            return;
        }

        Vector3 position = (pos != null) ? pos.Value : transform.position;

        CurrentSequenceIndex++;
        CurrentPhase = PhaseSequence[CurrentSequenceIndex];

        Destroy(CropPhaseInstance);
        CropPhaseInstance = Factory.MakeCropPhase(Name, CurrentPhase.ToString(), position, transform);
    }

    public void OnAdvanceTimeEvent()
    {
        // Next stage of crops
        IncrementCropPhase(transform.position);
    }

    void OnEnable()
    {
        Sender = GameEventSender.Instance;
        Sender.AdvanceTimeEvent += OnAdvanceTimeEvent;
    }

    void OnDisable()
    {
        Sender.AdvanceTimeEvent -= OnAdvanceTimeEvent;
    }
}
