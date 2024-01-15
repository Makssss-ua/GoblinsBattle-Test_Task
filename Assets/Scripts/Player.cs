using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    public Health hp { get; private set; }
    [SerializeField] private Animator animator;

    [Header("Attack")]
    [SerializeField] private KeyCode attack = KeyCode.Mouse0;
    [SerializeField] private AttackTriger attackTriger; 
    [SerializeField] private float damage;
    public float attackSpeed;
    [SerializeField] private float attackRange = 2;
    [SerializeField] private Vector2 timeRange;

    [Header("Super Attack")]
    [SerializeField] private EnemiFinder enemiFinder;
    [SerializeField] private KeyCode superAttack = KeyCode.Space;
    [SerializeField] private float superDamage;
    public float superSpeed;
    [SerializeField] private float superRange = 2;
    [SerializeField] private Vector2 superTimeRange;

    [Header("Movement")]
    [Space(5f)]
    [SerializeField] private float speed = 1f;
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private KeyCode up = KeyCode.W;
    [SerializeField] private KeyCode left = KeyCode.A;
    [SerializeField] private KeyCode down = KeyCode.S;
    [SerializeField] private KeyCode right = KeyCode.D;

    public float lastAttackTime { get; private set; } = 0;
    public float lastSuperAttackTime { get; private set; } = 0;
    private bool isDead = false;
    private Vector3 moveVector = Vector3.zero;
    private Quaternion rotateVector;
    private Rigidbody rigidbody;

    public Action playerDie;
    public Action playerAttack;
    public Action playerSuperAttack;

    private void Awake()
    {
        hp = GetComponent<Health>();
        rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (InactiveCheck())
            return;

        Move();
    }

    private void Update()
    {
        if (InactiveCheck())
            return;

        Attack();
    }

    private void Move()
    {
        moveVector = Vector3.zero;
        if (Input.GetKey(up))
        {
            moveVector += Vector3.forward;
            rotateVector = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 0), rotationSpeed * Time.deltaTime);
        }

        if (Input.GetKey(down))
        {
            moveVector += -Vector3.forward;
            rotateVector = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 180, 0), rotationSpeed * Time.deltaTime);
        }

        if (Input.GetKey(right))
        {
            moveVector += Vector3.right;
            rotateVector = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 90, 0), rotationSpeed * Time.deltaTime);
            if (Input.GetKey(up))
                rotateVector = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 45, 0), rotationSpeed * Time.deltaTime);
            else if (Input.GetKey(down))
                    rotateVector = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 135, 0), rotationSpeed * Time.deltaTime);
        }

        if (Input.GetKey(left))
        {
            moveVector += -Vector3.right;
            rotateVector = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, -90, 0), rotationSpeed * Time.deltaTime);
            if (Input.GetKey(up))
                rotateVector = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, -45, 0), rotationSpeed * Time.deltaTime);
            else if (Input.GetKey(down))
                rotateVector = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, -135, 0), rotationSpeed * Time.deltaTime);
        }

        moveVector = Vector3.ClampMagnitude(moveVector, 1f);

        rigidbody.MovePosition(transform.position + moveVector * speed * Time.fixedDeltaTime);
        transform.rotation = rotateVector;
        animator.SetFloat("Speed", moveVector.magnitude * speed);
    }

    private void Attack()
    {
        if (Input.GetKey(superAttack) && enemiFinder.enemiesInRange)
        {
            if ((Time.time - lastSuperAttackTime > superSpeed) && (Time.time - lastAttackTime > attackSpeed))
            {
                lastSuperAttackTime = Time.time;
                lastAttackTime = Time.time;
                animator.SetTrigger("SuperAttack");

                StartCoroutine(attackTriger.Attack(superRange, superDamage, timeRange));
                playerSuperAttack?.Invoke();
            }
        }

        if (Input.GetKey(attack))
        {
            if (Time.time - lastAttackTime > attackSpeed)
            {
                lastAttackTime = Time.time;
                animator.SetTrigger("Attack");

                StartCoroutine(attackTriger.Attack(attackRange, damage, superTimeRange));
                playerAttack?.Invoke();
            }
        }
    }

    private void Die()
    {
        isDead = true;
        animator.SetTrigger("Die");

        playerDie?.Invoke();
    }

    private bool InactiveCheck()
    {
        if (isDead)
        {
            return true;
        }

        if (hp.health <= hp.minHealth)
        {
            Die();
            return true;
        }

        return false;
    }
}
