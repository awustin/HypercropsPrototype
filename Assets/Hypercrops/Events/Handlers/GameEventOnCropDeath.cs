using UnityEngine;

using Assets.Hypercrops.System.Managers;

// TODO: In the future, crop death will not call Farm. Instead the action killing crop will be added to a "daily task" list
namespace Assets.Hypercrops.Events.Handlers
{
    public class GameEventOnCropDeath : MonoBehaviour
    {
        public FarmManager Farm;
        public GameEventSender Sender;

        public void OnCropDeath(object sender, CropDeathArguments e)
        {
            Farm.KillCrop(e.Crop);
        }

        void OnEnable()
        {
            Farm = FarmManager.Instance;
            Sender = GameEventSender.Instance;

            Sender.CropDeathEvent += OnCropDeath;
        }

        void OnDisable()
        {
            Sender.CropDeathEvent -= OnCropDeath;
        }
    }
}