using UnityEngine;
using Sirenix.OdinInspector;

public class AirSimulation : MonoBehaviour {
    [SerializeField] private Vector2Int _gridSize;
    [SerializeField] private float _initialAmountOfAir;
    [SerializeField] private Vector2 _initialVelocity;

    public Node[,] Nodes { get; set; } = new Node[0,0];

    private bool _isSimulationRunning = true;

    private void Start() {
        GenerateGrid();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space))
            _isSimulationRunning = !_isSimulationRunning;
    }

    private void FixedUpdate() {
        if (_isSimulationRunning)
            Step();
    }

    [Button("Step Simulation")]
    public void Step() {
        Node[,] newNodes = Nodes.Clone() as Node[,];

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

                newNodes[i, j].Density = averageDensityBetweenNodeAndNeighbours;

                if (i > 0)
                    newNodes[i - 1, j].Density = averageDensityBetweenNodeAndNeighbours;
                if (i < _gridSize.x - 1)
                    newNodes[i + 1, j].Density = averageDensityBetweenNodeAndNeighbours;
                if (j > 0)
                    newNodes[i, j - 1].Density = averageDensityBetweenNodeAndNeighbours;
                if (j < _gridSize.y - 1)
                    newNodes[i, j + 1].Density = averageDensityBetweenNodeAndNeighbours;
            }
        }

        Nodes = newNodes.Clone() as Node[,];
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