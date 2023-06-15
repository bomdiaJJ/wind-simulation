using UnityEngine;
using Sirenix.OdinInspector;

public class AirSimulation : MonoBehaviour {
    [SerializeField] private Vector2Int _gridSize;
    [SerializeField] private float _initialAmountOfAir;

    public Node[,] Nodes { get; set; } = new Node[0,0];

    [Button("Step Simulation")]
    public void Step() {
        for (int i = 0; i < _gridSize.x; i++) {
            for (int j = 0; j < _gridSize.y; j++) {
                // if (i+1 >= Nodes.GetLength(0)) continue;
                // if (i-1 <= 0) continue;

                
            }
        }
    }

    [Button("Generate Node Grid")]
    public void GenerateGrid() {
        Nodes = new Node[_gridSize.x, _gridSize.y];

        for (int i = 0; i < _gridSize.x; i++) {
            for (int j = 0; j < _gridSize.y; j++) {
                Nodes[i,j].NodePosition = new Vector2Int(i, j);
                Nodes[i,j].AmountOfAir = _initialAmountOfAir;
                Nodes[i,j].NodeVelocity = Vector2.zero;
            }
        }
    }


}