using UnityEngine;

namespace Weapons
{
    public class Axe : Weapon
    {
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("other.Collider = " + other.name);
        }
    }
}