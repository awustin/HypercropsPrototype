using UnityEngine;
using System;

public class UIManager : MonoBehaviour
{
    [HideInInspector]
    public GameState State;
    [HideInInspector]
    public GameEventSender Sender;

    [Header("UI Interaction Details")]
    [SerializeField] GameObject LastSelected;

    private void OnButtonEvent(object sender, ButtonEventArguments e)
    {
        LastSelected = e.Target;

        if (LastSelected.CompareTag("SeedCard"))
        {
            // Start farming mode
            string cropName = e.ActionName;
            Sender.BroadcastFarmingModeEvent(new Vector3(0, 0, 0), cropName);
        }
        else if (e.ActionName == "AdvanceTime")
        {
            Sender.BroadcastAdvanceTimeEvent();
        }
    }

    void OnEnable()
    {
        State = GameState.Instance;
        Sender = GameEventSender.Instance;

        if (Sender != null)
        {
            Sender.ButtonEvent += OnButtonEvent;
        }
        else
        {
            Debug.LogWarning("No GameEventSender detected");
        }
    }

    void OnDisable()
    {
        Sender.ButtonEvent -= OnButtonEvent;
    }
}
