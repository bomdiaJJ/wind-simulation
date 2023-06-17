using UnityEngine;
using Sirenix.OdinInspector;

public class AirSimulation : MonoBehaviour {
    [SerializeField] private Vector2Int _gridSize;
    [SerializeField] private float _initialAmountOfAir;
    [SerializeField] private Vector2 _initialVelocity;

    public Node[,] Nodes { get; set; } = new Node[0,0];

    private void Start() {
        GenerateGrid();
    }

    private void FixedUpdate() {
        Step();
    }

    [Button("Step Simulation")]
    public void Step() {

        for (int i = 0; i < _gridSize.x; i++) {
            for (int j = 0; j < _gridSize.y; j++) {
                float averageDensityBetweenNodeAndNeighbours = Nodes[i, j].Density;
                int nodesContributing = 1;
                
                // node da esquerda
                if (i > 0) {
                    averageDensityBetweenNodeAndNeighbours += Nodes[i-1, j].Density;
                    nodesContributing++;
                }
                
                // node da direita
                if (i < _gridSize.x - 1) {
                    averageDensityBetweenNodeAndNeighbours += Nodes[i + 1, j].Density;
                    nodesContributing++;
                }

                // node de cima
                if (j > 0) {
                    averageDensityBetweenNodeAndNeighbours += Nodes[i, j - 1].Density;
                    nodesContributing++;
                }   
                
                // node de baixo
                if (j < _gridSize.y - 1) {
                    averageDensityBetweenNodeAndNeighbours += Nodes[i, j+ 1].Density;
                    nodesContributing++;
                }

                averageDensityBetweenNodeAndNeighbours /= nodesContributing;

                Nodes[i, j].Density = averageDensityBetweenNodeAndNeighbours;

                if (i > 0)
                    Nodes[i - 1, j].Density = averageDensityBetweenNodeAndNeighbours;
                if (i < _gridSize.x - 1)
                    Nodes[i + 1, j].Density = averageDensityBetweenNodeAndNeighbours;
                if (j > 0)
                    Nodes[i, j - 1].Density = averageDensityBetweenNodeAndNeighbours;
                if (j < _gridSize.y - 1)
                    Nodes[i, j + 1].Density = averageDensityBetweenNodeAndNeighbours;
            }
        }
    }

    [Button("Generate Node Grid")]
    public void GenerateGrid() {
        Nodes = new Node[_gridSize.x, _gridSize.y];

        for (int i = 0; i < _gridSize.x; i++) {
            for (int j = 0; j < _gridSize.y; j++) {
                Nodes[i,j].NodePosition = new Vector2Int(i, j);
                Nodes[i,j].Density = _initialAmountOfAir;
                Nodes[i,j].NodeVelocity = _initialVelocity;
            }
        }
    }


}