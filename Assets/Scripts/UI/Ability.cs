using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;


namespace UI
{
    public class Ability : MonoBehaviour
    {
        [SerializeField] 
        private Image _iconAttack;
        [SerializeField] 
        private Image _iconDoubleAttack;
       
        private Cooldown _cooldown;

        [Inject]
        private void Construct (Cooldown cooldown)
        {
            _cooldown = cooldown;
        }
        
        private void Awake()
        {
            _iconAttack.fillAmount = 0;
            _iconDoubleAttack.fillAmount = 0;
        }
        
        public void StartAttackCooldown()
        {
            if (_cooldown.IsCoolDownAttack) return;
            _cooldown.StartCooldownAttack();
            _iconAttack.fillAmount = 1;
            _iconAttack.DOFillAmount(0, _cooldown.CooldownAttackTime).OnComplete(()=>
            {
                _cooldown.StopCooldownAttack();
            });
        }
        public void StartDoubleAttackCooldown()
        {
            if (_cooldown.IsCoolDownDoubleAttack) return;
            _cooldown.StartCooldownDoubleAttack();
            _iconDoubleAttack.fillAmount = 1;
            _iconDoubleAttack.DOFillAmount(0, _cooldown.CooldownDoubleAttackTime).OnComplete(()=>
            {
                _cooldown.StopCooldownDoubleAttack();
            });
        }
    }
}