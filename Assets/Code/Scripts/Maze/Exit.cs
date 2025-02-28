using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Scripts.Maze
{
    public class Exit : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                SceneManager.LoadScene("dev-prototypage-christopher");
            }
        }
    }
}
