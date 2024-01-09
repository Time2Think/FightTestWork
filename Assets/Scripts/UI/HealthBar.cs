using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField]
        private Health _health;
        [SerializeField]
        private GameObject _healthbar;
        [SerializeField]
        private Image _healthField;
        [SerializeField] 
        private Gradient _gradient;

        private Camera _camera;
        private void Awake()
        {
            _camera = Camera.main;
            _health.OnHealthChanged += ChangeHealthBar;
            _healthbar.SetActive(false);
        }

        private void OnDestroy()
        {
            _health.OnHealthChanged -= ChangeHealthBar;
        }

        private void ChangeHealthBar(float fillAmount)
        {
            _healthbar.SetActive(fillAmount > 0);
            _healthField.fillAmount = fillAmount;
            _healthField.color = _gradient.Evaluate(fillAmount);
            if (fillAmount <= 0)
            {
                DeactivateHealthBar();
            }
        }

        private void LateUpdate()
        { 
            _healthbar.transform.LookAt(new Vector3(_camera.transform.position.x,_healthbar.transform.parent.position.y,_camera.transform.position.z));
            _healthbar.transform.Rotate(180,0,0);
        }
        
        private void DeactivateHealthBar()
        {
            gameObject.SetActive(false);
        }
    }
}