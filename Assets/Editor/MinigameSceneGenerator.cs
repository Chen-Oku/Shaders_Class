using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class MinigameSceneGenerator : EditorWindow
{
    [MenuItem("Tools/Minigame/Crear escena museo-plaza")]
    public static void ShowWindow()
    {
        GetWindow<MinigameSceneGenerator>(true, "Generador Minijuego");
    }

    void OnGUI()
    {
        GUILayout.Label("Generar escena base para Minijuego", EditorStyles.boldLabel);
        if (GUILayout.Button("Crear escena nueva"))
        {
            CreateScene();
        }
    }

    static void CreateScene()
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
        // Camera overhead for plaza
        var camTop = new GameObject("Camera_Plaza");
        var cam = camTop.AddComponent<Camera>();
        cam.transform.position = new Vector3(0, 30, 0);
        cam.transform.rotation = Quaternion.Euler(90, 0, 0);
        cam.orthographic = true;
        cam.orthographicSize = 15;

        // Directional light
        var light = new GameObject("Directional Light");
        var ld = light.AddComponent<Light>();
        ld.type = LightType.Directional;
        light.transform.rotation = Quaternion.Euler(50, -30, 0);

        // Museo (simple cube as placeholder)
        var museo = GameObject.CreatePrimitive(PrimitiveType.Cube);
        museo.name = "Museo";
        museo.transform.localScale = new Vector3(20, 6, 12);
        museo.transform.position = new Vector3(-25, 3, 0);

        // Plaza (plane)
        var plaza = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plaza.name = "Plaza";
        plaza.transform.localScale = new Vector3(3,1,3);
        plaza.transform.position = new Vector3(5, 0, 0);

        // Add some placeholder slots and broken pieces
        for (int i = 0; i < 4; i++)
        {
            var slotGO = new GameObject($"Slot_{i}");
            var slot = slotGO.AddComponent<RepairSlot>();
            slot.expectedPieceId = $"piece_{i}";
            slot.acceptDistance = 1.5f;
            slotGO.transform.position = new Vector3(2 + i*3, 0.5f, -2 + (i%2==0? -2:2));
            var snap = new GameObject("Snap");
            snap.transform.parent = slotGO.transform;
            snap.transform.localPosition = Vector3.zero;
            slot.snapTarget = snap.transform;

            // create a broken piece elsewhere
            var piece = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            piece.name = $"Piece_{i}";
            piece.transform.localScale = Vector3.one * 0.8f;
            piece.transform.position = new Vector3(8 + i*2, 0.5f, -3 + i);
            piece.AddComponent<RepairPiece>().pieceId = $"piece_{i}";
            piece.AddComponent<DragAndDrop>();
            var rb = piece.AddComponent<Rigidbody>();
            rb.useGravity = false;
        }

        // Manager
        var mgrGO = new GameObject("MinigameManager");
        mgrGO.AddComponent<MinigameManager>();

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene, "Assets/Scenes/Minigame_Museo_Plaza.unity");
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("Escena creada", "Se creó Assets/Scenes/Minigame_Museo_Plaza.unity con objetos placeholder.", "OK");
    }
}
