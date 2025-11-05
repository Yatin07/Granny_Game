using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;


public class ReplacePlanesWithCubes : EditorWindow
{
    [MenuItem("Tools/Replace Planes With Cubes")]
    public static void ShowWindow()
    {
        GetWindow<ReplacePlanesWithCubes>("Replace Planes");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Replace Selected Planes"))
        {
            ReplaceSelectedPlanes();
        }
    }

    private static void ReplaceSelectedPlanes()
    {
        foreach (GameObject go in Selection.gameObjects)
        {
            if (go.GetComponent<MeshFilter>() != null &&
                go.GetComponent<MeshFilter>().sharedMesh != null &&
                go.GetComponent<MeshFilter>().sharedMesh.name == "Plane")
            {
                Vector3 position = go.transform.position;
                Quaternion rotation = go.transform.rotation;
                Vector3 scale = go.transform.localScale;

                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = position;
                cube.transform.rotation = rotation;

                // Scale cube: preserve X/Z, give small thickness in Y
                cube.transform.localScale = new Vector3(scale.x * 10f, 0.2f, scale.z * 10f);

                // Keep the cube in the same parent
                cube.transform.parent = go.transform.parent;

                Undo.RegisterCreatedObjectUndo(cube, "Create Cube Wall");
                Undo.DestroyObjectImmediate(go);
            }
        }

        Debug.Log("Selected planes replaced with cubes.");
    }
}
