using UnityEngine;

using Assets.Hypercrops.System;
using Assets.Hypercrops.System.CommonSerializable;
using Assets.Hypercrops.State;
using Assets.Hypercrops.Model.Buildables;

namespace Assets.Hypercrops.Events.Handlers
{
    public class BuildModeHandler : MonoBehaviour
    {
        public GameEventSender Sender;
        public GameState State;
        public ObjectFactory Factory;
        public BuildablesManager Manager;
        public GameObject GhostObject;

        private BuildableDescriptor _currentDescriptor;
        private BuildableGhost _buildableGhost;

        void OnEnable()
        {
            Sender = GameEventSender.Instance;
            State = GameState.Instance;
            Factory = ObjectFactory.Instance;
            _buildableGhost = GhostObject.GetComponent<BuildableGhost>();

            if (Manager == null)
            {
                Manager = GameObject.Find("BuildablesManager").GetComponent<BuildablesManager>();
            }

            Sender.StartBuildMode += OnStartBuildMode;
        }

        void OnDisable()
        {
            Sender.StartBuildMode -= OnStartBuildMode;
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
        }
    }
}