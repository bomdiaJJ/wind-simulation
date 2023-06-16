using UnityEngine;

[System.Serializable]
public struct Node {
    public Vector2Int NodePosition { get; set; }
    public Vector2 NodeVelocity { get; set; }
    public float Density { get; set; }
}