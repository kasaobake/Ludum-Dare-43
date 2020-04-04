using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputComponent : GameplayEventListener 
{
    #region Inspector Variables
    [Header("References")]
    [SerializeField]
    private MoveComponent moveComponent;
    #endregion

    #region Private Variables
    private Vector3 velocity;
    #endregion

    #region MonoBehaviour Functions
    protected override void Awake()
    {
        base.Awake();

        if (this.moveComponent == null)
        {
            this.moveComponent = GetComponent<MoveComponent>();
        }
    }

    private void Update() 
    {
        Vector3 input = new Vector3(Input.GetAxisRaw(Kameosa.Constants.Input.HORIZONTAL), 0f, Input.GetAxisRaw(Kameosa.Constants.Input.VERTICAL));
        this.moveComponent.TryMove(input);

    }
    #endregion

    #region Private Functions
    #endregion
}
