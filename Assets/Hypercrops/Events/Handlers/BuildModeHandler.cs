using UnityEngine;

using Assets.Hypercrops.System;
using Assets.Hypercrops.System.CommonSerializable;
using Assets.Hypercrops.State;
using Assets.Hypercrops.Model.Buildables;
using Assets.Hypercrops.Model.Cards;

namespace Assets.Hypercrops.Events.Handlers
{
    public class BuildModeHandler : MonoBehaviour
    {
        public GameEventSender Sender;
        public GameState State;
        public ObjectFactory Factory;
        public BuildablesManager Manager;
        public GameObject GhostObject;
        public CardsManager Cards;

        private BuildableDescriptor _currentDescriptor;
        private BuildableGhost _buildableGhost;

        void OnEnable()
        {
            Sender = GameEventSender.Instance;
            State = GameState.Instance;
            Factory = ObjectFactory.Instance;
            Cards = CardsManager.Instance;
            _buildableGhost = GhostObject.GetComponent<BuildableGhost>();

            if (Manager == null)
            {
                Manager = GameObject.Find("BuildablesManager").GetComponent<BuildablesManager>();
            }

            Sender.StartBuildMode += OnStartBuildMode;
            Sender.TryPlaceBuilding += OnTryPlacebuilding;
            Sender.CancelBuildMode += OnCancelBuildMode;
        }

        void OnDisable()
        {
            Sender.StartBuildMode -= OnStartBuildMode;
            Sender.TryPlaceBuilding -= OnTryPlacebuilding;
            Sender.CancelBuildMode -= OnCancelBuildMode;
        }

        void Update()
        {
            if (State.IsUIInteraction)
            {
                return;
            }
        }

        private void OnStartBuildMode(object sender, StartBuildModeArguments e)
        {
            _currentDescriptor = e.Descriptor;
            _buildableGhost.Activate(_currentDescriptor.LayoutType);

            State.SetBuildingGameMode();
        }

        private void OnTryPlacebuilding()
        {
            State.SetDefaultGameMode();

            if (_buildableGhost.IsAllowed && _currentDescriptor != null)
            {
                Manager.PlaceBuildable(_buildableGhost.ActionPoint, _currentDescriptor);
                Cards.DiscardLastUsed();
            }

            Clear();
        }

        private void OnCancelBuildMode()
        {
            if (State.IsBuildingGameMode())
            {
                State.SetLastCardSelected(null);
                State.SetDefaultGameMode();
                Clear();
            }
        }

        private void Clear()
        {
            _currentDescriptor = null;
            _buildableGhost.Deactivate();
        }
    }
}