using Oculus.Interaction;
using Oculus.Interaction.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Oculus.Interaction
{
    public class PokePositionDetector : MonoBehaviour
    {
        [SerializeField, Interface(typeof(IHand))]
        private UnityEngine.Object _hand;
        public IHand Hand { get; private set; }

        [SerializeField]
        private HandJointId _indexFingerJointId;

        [SerializeField]
        private Vector3 _localPositionOffset;

        [SerializeField]
        private Quaternion _rotationOffset = Quaternion.identity;

        // Adjust this distance threshold as needed for poke detection
        [SerializeField]
        private float _pokeDistanceThreshold = 0.1f;

        protected bool _started = false;

        protected virtual void Awake()
        {
            Hand = _hand as IHand;
        }

        protected virtual void Start()
        {
            this.BeginStart(ref _started);
            this.AssertField(Hand, nameof(Hand));
            this.EndStart(ref _started);
        }

        protected virtual void OnEnable()
        {
            if (_started)
            {
                Hand.WhenHandUpdated += HandleHandUpdated;
            }
        }

        protected virtual void OnDisable()
        {
            if (_started)
            {
                Hand.WhenHandUpdated -= HandleHandUpdated;
            }
        }

        private void HandleHandUpdated()
        {
            if (!Hand.GetJointPose(_indexFingerJointId, out Pose indexFingerPose)) return;

            Vector3 positionOffsetWithHandedness =
                (Hand.Handedness == Handedness.Left ? -1f : 1f) * _localPositionOffset;
            indexFingerPose.position += _rotationOffset * indexFingerPose.rotation *
                                         positionOffsetWithHandedness * Hand.Scale;

            // Get the poke position from the index finger tip
            Vector3 pokePosition = indexFingerPose.position;

            // Now you have the poke position, you can use it for interaction with objects
            // For example, you can check the distance between the poke position and objects
            // to determine if a poke interaction has occurred.

            // For demonstration, let's just print the poke position
            Debug.Log("Poke Position: " + pokePosition);
        }
    }
}
