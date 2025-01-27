using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

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
