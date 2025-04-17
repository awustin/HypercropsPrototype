using UnityEngine;

public class CropHealth : MonoBehaviour
{
    // TODO: Control life of Crops, and CropDisease
    public Crop CropScript;
    [Range(0f, 1f)]
    public float Life;
    public CropHealthReductionType LifeReductionType;
    private float _lifeReductionRate;

    void Start()
    {
        if (CropScript == null)
        {
            CropScript = GetComponentInParent<Crop>();
        }
    }

    void Update()
    {
        _lifeReductionRate = CropUtils.Instance.GetCropHealthReduction(LifeReductionType);
        if (Life > 0)
        {
            Life -= _lifeReductionRate * Time.deltaTime;
            Life = Mathf.Clamp01(Life);
        }
    }
}
