using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Wave", menuName = "Data/Waves")]
[Serializable]
public class Wave : ScriptableObject
{
    [FormerlySerializedAs("Characters")] public GameObject[] Enemies;
}
