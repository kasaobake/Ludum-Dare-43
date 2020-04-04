using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Kameosa.Components;
using Kameosa.Managers;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(FollowTargetComponent))]
[RequireComponent(typeof(MoveComponent))]
public class EnemyController : MonoBehaviour, IDamageable
{
    #region Inspector Variables
    [SerializeField]
    private List<string> followTargetTags = new List<string>();
    [SerializeField]
    private float attackDistance = 2f;
    [Space(10)]

    [Header("References")]
    [SerializeField]
    private GameplayController gameplay;
    [SerializeField]
    private MoveComponent moveComponent;
    [SerializeField]
    private DamageableComponent damageableComponent;
    #endregion

    #region Actions
    public static Action<int> OnDieStatic;
    #endregion

    #region Private Variables
    private Transform currentTarget = null;
    private IEnumerator checkCurrentTargetDistanceCoroutine = null;
    private float checkCurrentTargetDistanceRefreshRate = 0.1f;

    private FollowTargetComponent followTargetComponent;
    #endregion

    #region Properties
    public DamageableComponent DamageableComponent
    {
        get
        {
            return this.damageableComponent;
        }
    }
    #endregion

    #region MonoBehaviour Functions
    private void Awake()
    {
        if (this.gameplay == null)
        {
            this.gameplay = GameObject.Find("Gameplay").GetComponent<GameplayController>();
        }

        //if (this.playerTransform == null)
        //{
        //    this.playerTransform = GameObject.Find("Player").transform;
        //}

        //if (this.damageableComponent == null)
        //{
        //    this.damageableComponent = GetComponent<DamageableComponent>();
        //}

        this.followTargetComponent = GetComponent<FollowTargetComponent>();
        this.moveComponent = GetComponent<MoveComponent>();
        //this.collisionRadius = GetComponent<CapsuleCollider>().radius;
        //this.playerCollisionRadius = this.playerTransform.GetComponent<CapsuleCollider>().radius;
        //this.attackDistanceThresholdSquared = Mathf.Pow(this.attackDistanceThreshold + this.collisionRadius + this.playerCollisionRadius, 2);
        //this.followTargetComponent.PersonalSpace = this.collisionRadius + this.playerCollisionRadius;
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (this.currentTarget != null)
        {
            CheckDistanceToCurrentTarget();
        }
    }

    private void OnDestroy()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (this.followTargetTags.Contains(other.tag))
        {
            float currentTargetDistance = this.currentTarget == null ? Mathf.Infinity : Vector3.Distance(this.transform.position, this.currentTarget.position);
            float newTargetDistance = Vector3.Distance(this.transform.position, other.transform.position);

            if (newTargetDistance <= currentTargetDistance)
            {
                this.currentTarget = other.transform;
                this.followTargetComponent.FollowTarget(other.transform);
            }
        }
    }
    #endregion

    #region Private Functions
    private void CheckDistanceToCurrentTarget()
    {
        float distanceToCurrentTarget = Vector3.Distance(this.currentTarget.position, this.transform.position);

        if (distanceToCurrentTarget <= this.attackDistance)
        {
            Attack();
        }
        else
        {
            this.moveComponent.TryMove(this.followTargetComponent.NextMove);
        }
    }

    private void Attack()
    {
        Vector3 direction = this.currentTarget.position - this.transform.position;
        this.moveComponent.TryAttack(direction);
    }
    #endregion
}
