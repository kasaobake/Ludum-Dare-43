using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(MoveComponent))]
public class FollowTargetComponent : MonoBehaviour 
{
    #region Inspector Variables
    [SerializeField]
    private float velocity;
    [Space(10)]

    [Header("References")]
    [SerializeField]
    private GameplayController gameplay;
    [SerializeField]
    private MoveComponent moveComponent;
    [SerializeField]
    private Transform target;
    #endregion

    #region Private Variables
    private NavMeshAgent navMeshAgent;
    private float refreshPathRate = 0.2f;
    #endregion

    #region Properties
    public Transform Target
    {
        get
        {
            return this.target;
        }

        set
        {
            this.target = value;
        }
    }

    public Vector3 NextMove
    {
        get
        {
            return (this.navMeshAgent.nextPosition - this.transform.position);
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

        if (this.moveComponent == null)
        {
            this.moveComponent = GetComponent<MoveComponent>();
        }

        this.navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start() 
    {
        this.navMeshAgent.speed = this.velocity;
        this.navMeshAgent.updatePosition = false;
    }
    #endregion

    #region Public Functions
    public void FollowTarget(Transform target)
    {
        this.target = target;
        StartCoroutine(UpdatePath());
    }

    public void StopFollowTarget()
    {
        this.target = null;
        this.moveComponent.TryIdle();
    }
    #endregion

    #region Private Functions
    private IEnumerator UpdatePath()
    {
        while (this.target != null)
        {
            Vector3 directionToTarget = (this.target.position - this.transform.position).normalized;
            Vector3 targetPosition = this.target.transform.position - directionToTarget;
            this.navMeshAgent.SetDestination(targetPosition);

            yield return new WaitForSeconds(this.refreshPathRate);
        }
    }
    #endregion
}
