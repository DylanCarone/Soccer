
using UnityEngine;
using UnityEditor;

public class DistanceFinder : MonoBehaviour
{
    [Header("Targets")] public Transform player1;
    public Transform player2;
    
    [Header("Settings")]
    public Color gizmoColor = Color.green;

    [Range(10, 50)] public int fontSize = 14;



    private void OnDrawGizmos()
    {
        if (player1 == null || player2 == null) return;
        
        Vector3 player1Pos = player1.position;
        Vector3 player2Pos = player2.position;
        float dist =  Vector3.Distance(player1Pos, player2Pos);
        
        Gizmos.color = gizmoColor;
        Gizmos.DrawLine(player1Pos, player2Pos);
        
        Gizmos.DrawSphere(player1Pos, 0.1f);
        Gizmos.DrawSphere(player2Pos, 0.1f);
        
        GUIStyle style = new GUIStyle();
        style.normal.textColor = gizmoColor;
        style.fontSize = fontSize;
        style.fontStyle = FontStyle.Bold;
        style.alignment = TextAnchor.MiddleCenter;
        
        Vector3 midpoint = Vector3.Lerp(player1Pos, player2Pos, 0.5f);

        // Handles.Label draws 3D text in the Scene View
        Handles.Label(midpoint, $"{dist:F2}m", style);
    }
}
