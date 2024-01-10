using UnityEngine;
using UnityEngine.Serialization;

namespace Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        public float Damage;
        public float AtackSpeed;
        public float AttackRange;
        public bool IsAttacking;
        public abstract void EnabledWeaponCollider(bool status);
     
    }
}