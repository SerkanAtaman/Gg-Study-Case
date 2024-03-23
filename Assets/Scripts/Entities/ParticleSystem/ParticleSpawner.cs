using Gameframe.ServiceProvider;
using System;
using UnityEngine;

namespace GG.Entities.ParticleSystem
{
    public class ParticleSpawner : MonoBehaviour
    {
        [SerializeField] private ParticleGroup[] _particleGroups;

        private void Awake()
        {
            ServiceCollection.Current.AddSingleton(this);
        }

        public void PlayParticle(int id, Action callback = null)
        {
            if(id < 0 ||  id >= _particleGroups.Length)
            {
                Debug.LogError("Particle id is out of the bounds of the particle groups array");
                return;
            }

            _particleGroups[id].Play(callback);
        }
    }
}