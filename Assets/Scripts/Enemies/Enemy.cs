using UnityEngine;
using UnityEngine.AI;
using Weapons;
using Zenject;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Weapon _weapon;
    [SerializeField]
    private Health _health;

    public Animator AnimatorController;
    public NavMeshAgent Agent;

    private float lastAttackTime = 0;
    private bool isDead = false;

    public Health Health
    {
        get => _health;
        set => _health = value;
    }

    private BattleController _battleController;
    private Player _player;

    [Inject]
    private void Construct (BattleController battleController, Player player)
    {
        _battleController = battleController;
        _player = player;
    }
    
    private void Start()
    {
        _battleController.AddEnemy(this);
        Health.InitHp();
        Agent.SetDestination(_player.transform.position);
    }

    private void Update()
    {
        if(isDead)
        {
            return;
        }

        if (_health.CurrentHealth <= 0)
        {
            Die();
            return;
        }

        var distance = Vector3.Distance(transform.position, _player.transform.position);
        
        if (distance <= _weapon.AttackRange)
        {
            Agent.isStopped = true;
            if (Time.time - lastAttackTime > _weapon.AtackSpeed)
            {
                lastAttackTime = Time.time;
                _player.Health.TakeDamage(_weapon.Damage);
                AnimatorController.SetTrigger("Attack");
            }
        }
        else
        {
            AnimatorController.SetTrigger("Follow");
            Agent.isStopped = false;
            transform.LookAt(_player.transform.position);
            Agent.SetDestination(_player.transform.position);
        }
    }

    private void Die()
    {
        Agent.isStopped = true;
        _battleController.RemoveEnemy(this);
        isDead = true;
        AnimatorController.SetTrigger("Die");
    }
    
}
