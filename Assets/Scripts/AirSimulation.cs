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

        if (Input.GetKeyDown(KeyCode.Return))
            Step();
    }

    private void FixedUpdate() {
        if (_isSimulationRunning)
            Step();
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

    [Button("Step Simulation")]
    public void Step() {
        ChangeVelocitiesBasedOnDensityDifferences();
        ChangeDensitiesBasedOnVelocities();
    }

    [Button("Step (1) - Velocities"), HorizontalGroup("Steps")]
    public void ChangeVelocitiesBasedOnDensityDifferences() {
        Node[,] newNodes = Nodes.Clone() as Node[,];

        for (int i = 0; i < _gridSize.x; i++) {
            for (int j = 0; j < _gridSize.y; j++) {
                
                Vector2 finalVelocity = Vector2.zero;

                //0.70710678f é igual 1/sqrt(2)
                if (i > 0) { // esquerda
                    if (j < _gridSize.y - 1) {
                        float densityDifferenceTopLeft = Nodes[i, j].Density - Nodes[i - 1, j + 1].Density;
                        finalVelocity += densityDifferenceTopLeft * 0.70710678f * (Vector2.left + Vector2.up).normalized;
                    }

                    float densityDifferenceLeft = Nodes[i, j].Density - Nodes[i - 1, j].Density;
                    finalVelocity += Vector2.left * densityDifferenceLeft;
                    
                    if (j > 0) {
                        float densityDifferenceBottomLeft = Nodes[i, j].Density - Nodes[i - 1, j - 1].Density;
                        finalVelocity += densityDifferenceBottomLeft * 0.70710678f * (Vector2.left + Vector2.down).normalized;
                    }
                }

                if (i < _gridSize.x - 1) { // direita
                    if (j < _gridSize.y - 1) {
                        float densityDifferenceTopRight = Nodes[i, j].Density - Nodes[i + 1, j + 1].Density;
                        finalVelocity += densityDifferenceTopRight * 0.70710678f * (Vector2.right + Vector2.up).normalized;
                    }

                    float densityDifferenceRight = Nodes[i, j].Density - Nodes[i + 1, j].Density;
                    finalVelocity += Vector2.right * densityDifferenceRight;
                    
                    if (j > 0) {
                        float densityDifferenceBottomRight = Nodes[i, j].Density - Nodes[i + 1, j - 1].Density;
                        finalVelocity += densityDifferenceBottomRight * 0.70710678f * (Vector2.right + Vector2.down).normalized;
                    }
                }

                if (j > 0) { // baixo
                    float densityDifferenceBottom = Nodes[i, j].Density - Nodes[i, j - 1].Density;
                    finalVelocity += Vector2.down * densityDifferenceBottom;
                }

                if (j < _gridSize.y - 1) { // cima
                    float densityDifferenceTop = Nodes[i, j].Density - Nodes[i, j + 1].Density;
                    finalVelocity += Vector2.up * densityDifferenceTop;
                }

                newNodes[i, j].NodeVelocity = finalVelocity;
            }
        }

        Nodes = newNodes.Clone() as Node[,];
    }

    [Button("Step (2) - Densities"), HorizontalGroup("Steps")]
    public void ChangeDensitiesBasedOnVelocities() {
        Node[,] newNodes = Nodes.Clone() as Node[,];

        for (int i = 0; i < _gridSize.x; i++) {
            for (int j = 0; j < _gridSize.y; j++) {
                Vector2 velocityVector = Nodes[i, j].NodeVelocity + Nodes[i, j].NodePosition;

                int topIndex = Mathf.CeilToInt(velocityVector.y);
                int bottomIndex = Mathf.FloorToInt(velocityVector.y);
                int rightIndex = Mathf.CeilToInt(velocityVector.x);
                int leftIndex = Mathf.FloorToInt(velocityVector.x);
            }
        }

        Nodes = newNodes.Clone() as Node[,];
    }

}