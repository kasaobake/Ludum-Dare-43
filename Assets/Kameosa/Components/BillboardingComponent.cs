using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kameosa
{
    namespace Components
    {
        [ExecuteInEditMode]
        public class BillboardingComponent : MonoBehaviour
        {
            #region Inspector Variables
            [SerializeField]
            private bool isFacingCameraEveryUpdate = true;
            [SerializeField]
            private UnityEngine.Vector3 angleOffSet = UnityEngine.Vector3.zero;

            [Header("References")]
            [SerializeField]
            private Camera mainCamera;
            #endregion

            #region MonoBehaviour Functions
            private void Awake()
            {
                this.mainCamera = Camera.main;
            }

            private void Start()
            {
                if (!this.isFacingCameraEveryUpdate)
                {
                    UnityEngine.Vector3 directionToLookAt = this.transform.position - this.mainCamera.transform.position;
                    directionToLookAt.x = 0;
                    Quaternion rotation = Quaternion.LookRotation(directionToLookAt);
                    this.transform.eulerAngles = rotation.eulerAngles + this.angleOffSet;
                }
            }

            private void LateUpdate()
            {
                if (this.isFacingCameraEveryUpdate)
                {
                    UnityEngine.Vector3 directionToLookAt = this.transform.position - this.mainCamera.transform.position;
                    Quaternion rotation = Quaternion.LookRotation(directionToLookAt);
                    this.transform.eulerAngles = rotation.eulerAngles + this.angleOffSet;
                }
            }
            #endregion
        }
    }
}
