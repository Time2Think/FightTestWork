using Infrastructure;
using UnityEngine;
using UnityEngine.AI;
using Weapons;
using Zenject;

public class Enemy : MonoBehaviour
{
    
    public EnemyType TypeEnemy;
    [SerializeField]
    private Weapon _weapon;
    [SerializeField]
    private Health _health;

    public Animator AnimatorController;
    public NavMeshAgent Agent;

    private float lastAttackTime = 0;
    private bool isDead = false;

    private float _healPlayerAfterDie;
    public Health Health => _health;
   

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
        //_battleController.AddEnemy(this);
        Health.InitHp();
        _healPlayerAfterDie = _health.CurrentHealth / 10;
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
            //Agent.isStopped = true;
            _weapon.IsAttacking = true;
            if (Time.time - lastAttackTime > _weapon.AtackSpeed)
            {
                lastAttackTime = Time.time;
                AnimatorController.SetTrigger("Attack");
               
            }
        }
        else
        {
            _weapon.IsAttacking = false;
            AnimatorController.SetTrigger("Follow");
            Agent.isStopped = false;
            transform.LookAt(_player.transform.position);
            Agent.SetDestination(_player.transform.position);
        }
    }

    private void Die()
    {
        _player.Health.TakeHeal(_healPlayerAfterDie);
        Agent.isStopped = true;
        _battleController.RemoveEnemy(this);
        isDead = true;
        AnimatorController.SetTrigger("Die");
    }
    
}
