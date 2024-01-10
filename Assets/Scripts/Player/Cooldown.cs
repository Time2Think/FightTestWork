
    public class Cooldown 
    {
        private float _cooldownAttackTime = 0.5f;
        private float _cooldownDoubleAttackTime = 2f;
        
        public bool IsCoolDownAttack;
        public bool IsCoolDownDoubleAttack;

        public float CooldownAttackTime => _cooldownAttackTime;

        public float CooldownDoubleAttackTime => _cooldownDoubleAttackTime;

        public void StartCooldownAttack()
        {
            IsCoolDownAttack = true;
        }
        public void StopCooldownAttack()
        {
            IsCoolDownAttack = false;
        }

        public void StartCooldownDoubleAttack()
        {
            IsCoolDownDoubleAttack = true;
        }
        public void StopCooldownDoubleAttack()
        {
            IsCoolDownDoubleAttack = false;
        }
    }

