using UnityEngine;

public class CropPhases : MonoBehaviour
{
    public CropPhase Current = CropPhase.Seed;
    private CropPhase _currentTracker;

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
        }
    }
}
