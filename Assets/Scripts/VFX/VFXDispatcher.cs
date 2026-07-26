using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace DefaultNamespace.VFX
{
    public enum VFXMainTypes
    {
        BombExplode,
        BombPlace,
        BombTrail
    }
    
    public class VFXDispatcher : MonoBehaviour
    {
        private readonly List<VisualEffect> activeVFX = new List<VisualEffect>();

        [SerializeField]
        private VisualEffectAsset BombExplode;
        [SerializeField]
        private VisualEffectAsset BombPlace;
        [SerializeField]
        private VisualEffectAsset BombTrail;

        private VisualEffect CreateVFX(VisualEffectAsset asset, Vector3 position, Quaternion rotation)
        {
            GameObject vfxObject = new GameObject("VFX_" + asset.name);
            vfxObject.transform.position = position;
            vfxObject.transform.rotation = rotation;
            VisualEffect vfx = vfxObject.AddComponent<VisualEffect>();
            vfx.visualEffectAsset = asset;
            vfx.Play();
            activeVFX.Add(vfx);
            return vfx;
        }

        public void RequestVFX(Vector3 position, VFXMainTypes type)
        {
            VisualEffectAsset selectedAsset = type switch
            {
                VFXMainTypes.BombExplode => BombExplode,
                VFXMainTypes.BombPlace => BombPlace,
                VFXMainTypes.BombTrail => BombTrail,
                _ => null
            };
            if (!selectedAsset)
            {
                throw new NullReferenceException($"VFX asset for type {type} is not assigned.");
            }
            
            CreateVFX(selectedAsset, position, Quaternion.identity);
        }

        public void RequestVFX(Transform transform, VFXMainTypes type)
        {
            VisualEffectAsset selectedAsset = type switch
            {
                VFXMainTypes.BombExplode => BombExplode,
                VFXMainTypes.BombPlace => BombPlace,
                VFXMainTypes.BombTrail => BombTrail,
                _ => null
            };
            if (!selectedAsset)
            {
                throw new NullReferenceException($"VFX asset for type {type} is not assigned.");
            }
            
            CreateVFX(selectedAsset, transform.position, transform.rotation);
        }
            
        void Update()
        {
            for (int i = activeVFX.Count - 1; i >= 0; i--)
            {
                VisualEffect vfx = activeVFX[i];
                if (!vfx.isActiveAndEnabled || !vfx.aliveParticleCount.Equals(0))
                {
                    continue;
                }

                activeVFX.RemoveAt(i);
                Destroy(vfx.gameObject);
            }
        }
    }
}