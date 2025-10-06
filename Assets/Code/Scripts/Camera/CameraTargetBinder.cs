using System;
using Code.Scripts.Player;
using Unity.Cinemachine;
using UnityEngine;

namespace Code.Scripts.Camera
{
    /// <summary>
    /// Automatically binds the main camera to follow the player target.
    /// <remarks>Compatible with multiple cameras if using Debug module.</remarks>
    /// </summary>
    public class CameraTargetBinder : MonoBehaviour
    {
        private void Start()
        {
            Transform player = FindFirstObjectByType<PlayerController>().transform;

            CinemachineCamera[] cameras = FindObjectsByType<CinemachineCamera>(FindObjectsSortMode.None);
        }
    }
}
