using System;
using UnityEngine;

namespace Plugins.Packages.MazePrototypePackage.Scripts.Maze
{
    /// <summary>
    /// <c>Cell Rule</c> defines the wall configuration.
    /// </summary>
    [Serializable]
    public struct CellRule
    {
        public bool Top;
        public bool Right;
        public bool Bottom;
        public bool Left;
        public GameObject Prefab;
    }
}
