using UnityEngine;

namespace Scripts.Player
{
    public class ThirdPersonAnimation : MonoBehaviour
    {
        private Animator _animator;
        private Rigidbody _rb;
        private float _maxSpeed = 5f;

       
        private void Start()
        {
            _animator = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody>();
        }

        
        private void Update()
        {
            _animator.SetFloat("movementSpeed", _rb.velocity.magnitude / _maxSpeed);
        }
    }
}