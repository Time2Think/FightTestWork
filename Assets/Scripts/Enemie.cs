using UnityEngine;
using UnityEngine.AI;
using Weapons;

public class Enemie : MonoBehaviour
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


    private void Start()
    {
        SceneManager.Instance.AddEnemie(this);
        Health.InitHp();
        Agent.SetDestination(SceneManager.Instance.Player.transform.position);
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
            Agent.isStopped = true;
            return;
        }

        var distance = Vector3.Distance(transform.position, SceneManager.Instance.Player.transform.position);
     
        if (distance <= _weapon.AttackRange)
        {
            Agent.isStopped = true;
            if (Time.time - lastAttackTime > _weapon.AtackSpeed)
            {
                lastAttackTime = Time.time;
                SceneManager.Instance.Player.Health.TakeDamage(_weapon.Damage);
                AnimatorController.SetTrigger("Attack");
            }
        }
        else
        {
            Agent.SetDestination(SceneManager.Instance.Player.transform.position);
        }
        AnimatorController.SetFloat("Speed", Agent.speed);
    }

    private void Die()
    {
        SceneManager.Instance.RemoveEnemie(this);
        isDead = true;
        AnimatorController.SetTrigger("Die");
    }
    
}
