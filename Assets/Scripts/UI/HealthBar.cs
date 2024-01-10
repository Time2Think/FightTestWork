using System.Collections;
using TMPro;
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
        [SerializeField]
        private TextMeshProUGUI _damageText;
        private Coroutine _damageCoroutine;
        
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

        private void ChangeHealthBar(float fillAmount, float damage)
        {
            ShowDamage(damage);
            _healthbar.SetActive(fillAmount > 0);
            _healthField.fillAmount = fillAmount;
            _healthField.color = _gradient.Evaluate(fillAmount);
            if (fillAmount <= 0)
            {
                if (_damageCoroutine != null)
                {
                    StopCoroutine(_damageCoroutine);
                }
                DeactivateHealthBar();
            }
        }

        private void LateUpdate()
        { 
            _healthbar.transform.LookAt(new Vector3(_camera.transform.position.x,_healthbar.transform.parent.position.y,_camera.transform.position.z));
            _healthbar.transform.Rotate(0,0,0);
        }
        
        private void ShowDamage(float _damage)
        {
            if (_damageCoroutine != null)
            {
                StopCoroutine(_damageCoroutine);
            }
            _damageCoroutine =  StartCoroutine(LookDamage(_damage));
        }

        private IEnumerator LookDamage(float _damage)
        {
            _damageText.gameObject.SetActive(true);

            float startScale = 0.5f;
            float maxScale = 1f;
            Vector3 startPos = new Vector3(0f, 0f, 0f);
            Vector3 MaxPos = new Vector3(0f, 1f, 0f);

            _damageText.text = "-" + _damage;

            for (float i = 0; i < 1; i += Time.deltaTime)
            {
                yield return null;
                float currentScale = Mathf.Lerp(startScale, maxScale, i);
                _damageText.transform.localScale = Vector3.one * currentScale;

                float currentAplha = 1 - i;
                _damageText.alpha = currentAplha;

                Vector3 currentPos = Vector3.Lerp(startPos, MaxPos, i);
                _damageText.transform.localPosition = currentPos;
            }
            _damageText.gameObject.SetActive(false);
        }
        
        private void DeactivateHealthBar()
        {
            _healthbar.gameObject.SetActive(false);
        }
    }
}