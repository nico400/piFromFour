using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PointList))]
public class GIDPoints : Editor
{
    private PointList pointList;

    private void OnEnable()
    {
        pointList = (PointList)target;
    }

    public override void OnInspectorGUI()
    {
        // Desenha o Inspector padrão (lista de pontos)
        DrawDefaultInspector();

        GUILayout.Space(10);

        // Botão para adicionar novo ponto
        if (GUILayout.Button("➕ Adicionar Ponto"))
        {
            Undo.RecordObject(pointList, "Add Point");

            // Cria um ponto na frente do objeto
            Vector3 newPoint = pointList.transform.position + pointList.transform.forward * 2f;
            pointList.points.Add(pointList.transform.InverseTransformPoint(newPoint));

            EditorUtility.SetDirty(pointList);
        }
    }

    private void OnSceneGUI()
    {
        for (int i = 0; i < pointList.points.Count; i++)
        {
            Vector3 worldPos = pointList.transform.TransformPoint(pointList.points[i]);
            Vector3 newWorldPos = Handles.PositionHandle(worldPos, Quaternion.identity);

            if (newWorldPos != worldPos)
            {
                Undo.RecordObject(pointList, "Move Point");
                pointList.points[i] = pointList.transform.InverseTransformPoint(newWorldPos);
                EditorUtility.SetDirty(pointList);
            }

            // Label com botão de excluir
            Handles.Label(newWorldPos + Vector3.up * 0.3f, $"P{i}");
        }
    }
}
