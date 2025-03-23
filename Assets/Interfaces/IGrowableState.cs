using UnityEngine;

public interface IGrowableState
{
    GameObject GhostPrefab { get; set; }
    GameObject SeedPrefab { get; set; }
    GameObject GrowingPrefab { get; set; }
    GameObject ReadyPrefab { get; set; }
    GrowState CurrentState { get; set; }
    float SeedDuration { get; set; }
    float GrowingDuration { get; set; }
    float GrowthTime { get; set; }
    float Delay { get; set; }

    bool IsGhost();
    bool IsSeed();
    bool IsGrowing();
    bool IsReady();
}

public enum GrowState
{
    Ghost,
    Seed,
    Growing,
    Ready
}
