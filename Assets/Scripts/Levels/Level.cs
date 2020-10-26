using System;
using UnityEngine;

namespace enjoythevibes.Levels
{
    public class Level : MonoBehaviour
    {
        [SerializeField]
        private Transform spawnPoint = default;
        [SerializeField]
        private MeshRenderer[] meshRenderers = default;
        [SerializeField]
        private Collider[] colliders = default;
        
        public Vector3 SpawnPosition => spawnPoint.position;

        private void Awake()
        {
            Disable();
        }

        public void Enable()
        {
            foreach (var meshRenderer in meshRenderers)
            {
                meshRenderer.enabled = true;
            }
            foreach (var colldier in colliders)
            {
                colldier.enabled = true;
            }
        }

        public void Disable()
        {
            foreach (var meshRenderer in meshRenderers)
            {
                meshRenderer.enabled = false;
            }
            foreach (var colldier in colliders)
            {
                colldier.enabled = false;
            }
        }
    }
}