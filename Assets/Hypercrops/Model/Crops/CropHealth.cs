using UnityEngine;

using Assets.Hypercrops.Events;

public class CropHealth : MonoBehaviour
{
    public const float FactorScale = 1000f;
    public const float WaterFactor = 20f;
    public GameEventSender Sender;
    [Range(0f, 1f)] public float Life;
    public DamageSpeed DamageSpeedFactor = DamageSpeed.Normal;

    [Header("HealthStats")]
    public bool IsDead = false;
    public bool IsWatered = false;

    private float _damageFactor;
    private float _waterFactor;
    // Trackers
    private DamageSpeed _damageSpeedTracker;
    private bool _isWateredTracker;
    private readonly int _framesBetweenUpdates = 100;
    private int _frameCount = 0;

    void Start()
    {
        Sender = GameEventSender.Instance;

        SetDamageFactor();
        SetWaterFactor();
    }

    void Update()
    {
        TrackVariables();

        if (IsDead)
        {
            return;
        }

        if (_frameCount < _framesBetweenUpdates)
        {
            _frameCount ++;
            return;
        }

        if (Life > 0)
        {
            Life -= _damageFactor / _waterFactor * Time.deltaTime * _framesBetweenUpdates;
            Life = Mathf.Clamp01(Life);
        }
        else
        {
            IsDead = true;
            StartCropDeath();
        }

        _frameCount = 0;
    }

    public void StartCropDeath()
    {
        Sender.BroadcastEvent("CropDeath", gameObject);
    }

    private void TrackVariables()
    {
        if (_damageSpeedTracker != DamageSpeedFactor)
        {
            _damageSpeedTracker = DamageSpeedFactor;
            SetDamageFactor();
        }

        if (_isWateredTracker != IsWatered)
        {
            _isWateredTracker = IsWatered;
            SetWaterFactor();
        }
    }

    private void SetDamageFactor()
    {
        _damageFactor = (float) DamageSpeedFactor / FactorScale;
    }

    private void SetWaterFactor()
    {
        _waterFactor = IsWatered ? WaterFactor : 1;
    }
}
