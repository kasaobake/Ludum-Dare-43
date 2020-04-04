using UnityEngine;
using System.Collections;
using UnityEditor;
using DungeonGeneration;

[CustomEditor(typeof(DungeonGenerationComponent))]
public class DungeonGenerationComponentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        DungeonGenerationComponent dungeonGenerationComponent = this.target as DungeonGenerationComponent;

        if (DrawDefaultInspector())
        {
            dungeonGenerationComponent.Generate();
            dungeonGenerationComponent.DrawGizmos();
        }

        if (GUILayout.Button("Regenerate"))
        {
            dungeonGenerationComponent.Generate();
            dungeonGenerationComponent.DrawGizmos();
        }
    }
}
