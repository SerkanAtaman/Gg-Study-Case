using System;
using System.Collections;
using UnityEngine;

namespace GG.Entities.ParticleSystem
{
    public class ParticleGroup : MonoBehaviour
    {
        [SerializeField] private float _lifeTime;

        [SerializeField] private UnityEngine.ParticleSystem[] _particles;

        public float LifeTime => _lifeTime;

        private IEnumerator _activeCoroutine = null;
        private Action _playCallback = null;

        public void Play(Action callback = null)
        {
            _playCallback = callback;

            foreach (var particle in _particles)
            {
                particle.gameObject.SetActive(true);
                particle.Play();
            }

            _activeCoroutine = WaitForParticlePlay();
            StartCoroutine(_activeCoroutine);
        }

        public void Stop()
        {
            foreach (var particle in _particles)
            {
                particle.Stop();
                particle.gameObject.SetActive(false);
            }

            if(_activeCoroutine != null)
            {
                StopCoroutine(_activeCoroutine);
                _activeCoroutine = null;
            }

            _playCallback = null;
        }

        private IEnumerator WaitForParticlePlay()
        {
            yield return new WaitForSeconds(_lifeTime);

            _activeCoroutine = null;
            _playCallback?.Invoke();
            _playCallback = null;

            Stop();
        }
    }
}