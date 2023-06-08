using UnityEngine;
using Sirenix.OdinInspector;

public class AirSimulation : MonoBehaviour {
    [SerializeField] private Vector2Int _gridSize;
    [SerializeField] private float _initialAmountOfAir;

    public Node[,] Nodes { get; set; }

    [Button("Generate Grid")]
    public void GenerateGrid() {
        Nodes = new Node[_gridSize.x, _gridSize.y];

        for (int i = 0; i < _gridSize.x; i++) {
            for (int j = 0; j < _gridSize.y; j++) {
                Nodes[i,j].NodePosition = new Vector2Int(i, j);
                Nodes[i,j].AmountOfAir = _initialAmountOfAir;
            }
        }
    }


}