using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AOTEditor.Tools.SceneWindow
{
    [CreateAssetMenu(fileName = "scene-container", menuName ="Scene Overview/Create Container (make sure in Resources Folder)")]
    public class SceneContainer : ScriptableObject
    {
        public List<Data> scenes = new List<Data>();

        [System.Serializable]
        public class Data
        {
            public string name;
            public SceneAsset scene;
            public Texture2D screenshot;
        }
    }

}