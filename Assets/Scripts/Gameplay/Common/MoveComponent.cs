using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public enum MoveState
{
    Idle,
    Walk,
    Run,
    Attack
}

[RequireComponent(typeof(Rigidbody))]
public class MoveComponent : GameplayEventListener 
{
    #region Inspector Variables
    [SerializeField]
    private float maxVelocity = 5f;
    [SerializeField]
    private bool isNormalizingInput = true;
    [SerializeField]
    private bool isAlwaysFacingCursor = false;
    [Space(10)]

    [Header("Animation")]
    [SerializeField]
    private string idleAnimation = "Idle";
    [SerializeField]
    private string walkAnimation = "Walk";
    [SerializeField]
    private string runAnimation = "Run";
    [SerializeField]
    private string attackAnimation = "Attack";
    [Space(10)]

    [Header("References")]
    [SerializeField]
    private SkeletonAnimation frontSkeletonAnimation;
    [SerializeField]
    private SkeletonAnimation backSkeletonAnimation;
    [SerializeField]
    private SkeletonAnimation rightSkeletonAnimation;
    [SerializeField]
    private CrosshairController crosshair;
    #endregion

    #region Private Variables
    private new Rigidbody rigidbody;
    private MoveState moveState;
    private SkeletonAnimation currentSkeletonAnimation;
    private bool isCurrentSkeletonAnimationChanged;
    #endregion

    #region MonoBehaviour Functions
    protected override void Awake()
    {
        base.Awake();

        this.rigidbody = GetComponent<Rigidbody>();
    }

    protected override void Start() 
    {
        base.Start();

        this.moveState = MoveState.Idle;
        this.currentSkeletonAnimation = this.frontSkeletonAnimation;
    }

    private void Update() 
    {
        if (this.isAlwaysFacingCursor)
        {
            Vector3 heightCorrectedPoint = new Vector3(this.crosshair.Position.x, this.transform.position.y, this.crosshair.Position.z);
            this.transform.LookAt(heightCorrectedPoint);
        }
    }
    #endregion

    #region Public Functions
    public void TryMove(Vector3 velocity)
    {
        if (this.isNormalizingInput)
        {
            velocity = velocity.normalized;
        }

        if (velocity.magnitude > 0f)
        {
            SetCurrentSkeletonAnimation(velocity);

            this.rigidbody.MovePosition(this.rigidbody.position + (velocity * this.maxVelocity * Time.fixedDeltaTime));

            if (this.isCurrentSkeletonAnimationChanged || this.moveState != MoveState.Run)
            {
                this.currentSkeletonAnimation.AnimationState.SetAnimation(0, this.runAnimation, true);
                this.moveState = MoveState.Run;
            }
        }
        else
        {
            TryIdle();
        }
    }

    public void TryIdle()
    {
        if (this.isCurrentSkeletonAnimationChanged || this.moveState != MoveState.Idle)
        {
            this.currentSkeletonAnimation.AnimationState.SetAnimation(0, this.idleAnimation, true);
            this.moveState = MoveState.Idle;
        }
    }

    public void TryAttack(Vector3 direction)
    {
        SetCurrentSkeletonAnimation(direction);

        if (this.isCurrentSkeletonAnimationChanged || this.moveState != MoveState.Attack)
        {
            this.currentSkeletonAnimation.AnimationState.SetAnimation(0, this.attackAnimation, true);
            this.moveState = MoveState.Attack;
        }
    }
    #endregion

    #region Private Functions
    private void SetCurrentSkeletonAnimation(Vector3 velocity)
    {
        this.isCurrentSkeletonAnimationChanged = false;

        if (velocity.x != 0f)
        {
            bool isFacingLeft = velocity.x < 0f;
            this.rightSkeletonAnimation.Skeleton.FlipX = isFacingLeft;
            this.currentSkeletonAnimation = this.rightSkeletonAnimation;
        }
        else if (velocity.z > 0f)
        {
            this.currentSkeletonAnimation = this.backSkeletonAnimation;
        }
        else
        {
            this.currentSkeletonAnimation = this.frontSkeletonAnimation;
        }

        if (!this.currentSkeletonAnimation.gameObject.activeInHierarchy)
        {
            this.isCurrentSkeletonAnimationChanged = true;
            this.currentSkeletonAnimation.gameObject.SetActive(true);

            this.frontSkeletonAnimation.gameObject.SetActive(this.currentSkeletonAnimation == this.frontSkeletonAnimation);
            this.backSkeletonAnimation.gameObject.SetActive(this.currentSkeletonAnimation == this.backSkeletonAnimation);
            this.rightSkeletonAnimation.gameObject.SetActive(this.currentSkeletonAnimation == this.rightSkeletonAnimation);
        }
    }
    #endregion
}
