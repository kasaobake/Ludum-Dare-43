using System;
using Kameosa.Cartesian;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace DungeonGeneration
{
    [RequireComponent(typeof(DungeonGenerationComponent))]
    public class DungeonController : MonoBehaviour
    { 
        #region Private Variables
        private DungeonGenerationComponent dungeonGenerationComponent;
        private Dungeon dungeon;
        private NavMeshSurface navMeshSurface;
        #endregion

        #region Actions
        public event Action OnDungeonGenerated;
        #endregion

        #region Properties
        public Coordinate StartCoordinate
        {
            get
            {
                return this.dungeon.StartCoordinate;
            }
        }

        public Dungeon Dungeon
        {
            get
            {
                return this.dungeon;
            }
        }
        #endregion

        #region MonoBehaviour Functions
        private void Awake()
        {
            this.dungeonGenerationComponent = GetComponent<DungeonGenerationComponent>();
            this.navMeshSurface = GetComponent<NavMeshSurface>();
        }

        private void Start()
        {
            Generate();
            UpdateNavMesh();
        }

        private void Update()
        {
        }
        #endregion

        #region Public Functions
        public void Generate()
        {
            this.dungeon = this.dungeonGenerationComponent.Generate();

            if (OnDungeonGenerated != null)
            {
                OnDungeonGenerated();
            }
        }
        #endregion

        #region Private Functions
        private void UpdateNavMesh()
        {
            this.navMeshSurface.BuildNavMesh();
        }
        #endregion
    }
}