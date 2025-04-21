using UnityEngine;

public class CropHealth : MonoBehaviour
{
    public GameEventSender Sender;
    public Crop CropScript;
    [Range(0f, 1f)]
    public float Life;
    public CropHealthReductionType LifeReductionType;
    public bool IsDead = false;
    private float _lifeReductionRate;

    void Start()
    {
        if (CropScript == null)
        {
            CropScript = GetComponentInParent<Crop>();
        }

        Sender = GameEventSender.Instance;
        SetCropHealthReductionType(LifeReductionType);
    }

    void Update()
    {
        if (Life > 0)
        {
            Life -= _lifeReductionRate * Time.deltaTime;
            Life = Mathf.Clamp01(Life);
        }

        if (Life == 0 && !IsDead)
        {
            IsDead = true;
            StartCropDeath();
        }
    }

    public void SetCropHealthReductionType(CropHealthReductionType type)
    {
        _lifeReductionRate = CropUtils.Instance.GetCropHealthReduction(type);
    }

    public void StartCropDeath()
    {
        Sender.BroadcastCropDeathEvent(gameObject);
    }
}
