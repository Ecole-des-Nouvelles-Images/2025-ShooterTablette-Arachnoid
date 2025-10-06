using UnityEngine;

namespace Code.Scripts.Enemies
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
            Debug.Log(other.name);
            Color randomColor = Random.ColorHSV();
            _renderer.material.color = randomColor;
        }
    }
}