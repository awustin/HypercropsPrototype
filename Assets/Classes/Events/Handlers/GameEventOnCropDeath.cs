using UnityEngine;

public class GameEventOnCropDeath : MonoBehaviour
{
    public FarmManager Farm;
    public GameEventSender Sender;

    void Awake()
    {
        if (Farm == null)
        {
            Farm = GetComponentInParent<FarmManager>();
        }
    }
    
    public void OnCropDeath(object sender, CropDeathArguments e)
    {
        Farm.KillCrop(e.Crop);
    }

    void OnEnable()
    {
        Sender = GameEventSender.Instance;

        Sender.CropDeathEvent += OnCropDeath;
    }

    void OnDisable()
    {
        Sender.CropDeathEvent -= OnCropDeath;
    }
}
