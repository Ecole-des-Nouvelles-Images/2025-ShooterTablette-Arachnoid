using UnityEngine;

namespace Code.Scripts
{
    public class ToDeleteEnemyHit : MonoBehaviour
    {
        private Renderer _renderer;

        private void Start()
        {
            _renderer = GetComponent<Renderer>();
        }

        private void OnParticleCollision(GameObject other)
        {
            Color randomColor = Random.ColorHSV();
            _renderer.material.color = randomColor;
        }
    }
}