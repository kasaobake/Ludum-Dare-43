using System.Collections;
using System.Collections.Generic;
using Kameosa.Collections.IList;
using UnityEngine;

namespace Kameosa
{
    namespace Components
    {
        [ExecuteInEditMode]
        public class RandomSpriteComponent : MonoBehaviour
        {
            #region Inspector Variables
            [SerializeField]
            private List<Sprite> sprites;

            [Header("References")]
            [SerializeField]
            private SpriteRenderer spriteRenderer;
            #endregion

            #region MonoBehaviour Functions
            private void Awake()
            {
                if (this.spriteRenderer == null)
                {
                    this.spriteRenderer = GetComponent<SpriteRenderer>();
                }
            }

            private void Start()
            {
                this.spriteRenderer.sprite = sprites.GetRandom();
            }
            #endregion
        }
    }
}
