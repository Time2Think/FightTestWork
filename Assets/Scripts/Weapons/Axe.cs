using UnityEngine;

namespace Weapons
{
    public class Axe : Weapon
    {
        private Collider _collider;

        private void Start()
        {
            _collider = gameObject.GetComponent<MeshCollider>();
        }

        public override void EnabledWeaponCollider(bool enabled)
        {
            _collider.enabled = enabled;
        }
       
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy") && IsAttacking)
            {
                other.GetComponent<Enemy>().Health.TakeDamage(Damage);
            }
            else if (other.CompareTag("Player") && IsAttacking)
            {
                other.GetComponent<Player>().Health.TakeDamage(Damage);
            }
           
        }
    }
}