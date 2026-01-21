using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class Terrainhandler : EditorWindow
{
    private Terrain terrain;
    private Vector2 scrollPos;
    private List<GameObject> assetsToSnap = new List<GameObject>();

    [MenuItem("Tools/Snap Assets To Terrain")]
    public static void ShowWindow()
    {
        GetWindow<Terrainhandler>("Snap Assets To Terrain");
    }

    private void OnGUI()
    {
        GUILayout.Label("Snap Assets To Terrain", EditorStyles.boldLabel);

        // Terrain field
        terrain = (Terrain)EditorGUILayout.ObjectField("Terrain", terrain, typeof(Terrain), true) as Terrain;

        GUILayout.Space(10);

        // Drag-and-drop area
        GUILayout.Label("Drag & Drop Assets Here:");
        Rect dropArea = GUILayoutUtility.GetRect(0f, 100f, GUILayout.ExpandWidth(true));
        GUI.Box(dropArea, "Drop GameObjects Here");

        Event evt = Event.current;
        switch (evt.type)
        {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if (!dropArea.Contains(evt.mousePosition))
                    break;

                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                if (evt.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();

                    foreach (Object draggedObj in DragAndDrop.objectReferences)
                    {
                        GameObject go = draggedObj as GameObject;
                        if (go != null && !assetsToSnap.Contains(go))
                        {
                            assetsToSnap.Add(go);
                        }
                    }
                }
                Event.current.Use();
                break;
        }

        // Scrollable list of assets
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(150));
        for (int i = 0; i < assetsToSnap.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            assetsToSnap[i] = EditorGUILayout.ObjectField(assetsToSnap[i], typeof(GameObject), true) as GameObject;
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                assetsToSnap.RemoveAt(i);
                i--;
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();

        GUILayout.Space(10);

        if (GUILayout.Button("Snap Assets"))
        {
            if (terrain == null)
            {
                EditorUtility.DisplayDialog("Error", "Please assign a terrain!", "OK");
                return;
            }

            SnapAllAssets();
        }
    }

    private void SnapAllAssets()
    {
        int count = 0;
        foreach (GameObject obj in assetsToSnap)
        {
            if (obj == null) continue;

            Vector3 pos = obj.transform.position;
            pos.y = terrain.SampleHeight(pos) + terrain.transform.position.y;
            obj.transform.position = pos;
            count++;
        }

        EditorUtility.DisplayDialog("Success", $"Snapped {count} assets to terrain!", "OK");
    }
}
