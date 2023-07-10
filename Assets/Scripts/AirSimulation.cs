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
                // 🟢 O VENTO SÓ MANDA DENSIDADE PARA OS NÓS AO REDOR
                // QUANTO MAIS VELOCIDADE NA DIREÇÃO DELE, MAIS DENSIDADE ELE MANDA
                // A DENSIDADE TOTAL ENVIADA DEPENDE DA MAGNITUDE DO VETOR DE VELOCIDADE

                float horizontalVelocity = Mathf.Abs(newNodes[i, j].NodeVelocity.x);
                float verticalVelocity = Mathf.Abs(newNodes[i, j].NodeVelocity.y);

                // 🟡 velocidade total parece um nome errado para isso aqui
                float totalVelocity = horizontalVelocity + verticalVelocity;

                float horizontalPartOfTotalVelocity = horizontalVelocity / totalVelocity;
                float verticalPartOfTotalVelocity = verticalVelocity / totalVelocity;

                Vector2Int? topNode =
                    j < _gridSize.y - 1
                    ? new Vector2Int(i, j + 1)
                    : null;
                Vector2Int? bottomNode =
                    j > 0
                    ? new Vector2Int(i, j - 1)
                    : null;

                Vector2Int? rightNode =
                    i < _gridSize.x - 1
                    ? new Vector2Int(i + 1, j)
                    : null;
                Vector2Int? leftNode =
                    i > 0
                    ? new Vector2Int(i - 1, j)
                    : null;

                if (rightNode.HasValue && Nodes[i, j].NodeVelocity.x > 0) {
                    newNodes[rightNode.Value.x, rightNode.Value.y].Density += horizontalPartOfTotalVelocity * Nodes[i, j].NodeVelocity.magnitude;
                } else if (leftNode.HasValue && Nodes[i, j].NodeVelocity.x < 0) {
                    newNodes[leftNode.Value.x, leftNode.Value.y].Density += horizontalPartOfTotalVelocity * Nodes[i, j].NodeVelocity.magnitude;
                }

                if (topNode.HasValue && Nodes[i, j].NodeVelocity.y > 0) {
                    newNodes[topNode.Value.x, topNode.Value.y].Density += verticalPartOfTotalVelocity * Nodes[i, j].NodeVelocity.magnitude;
                } else if (bottomNode.HasValue && Nodes[i, j].NodeVelocity.y < 0) {
                    newNodes[bottomNode.Value.x, bottomNode.Value.y].Density += verticalPartOfTotalVelocity * Nodes[i, j].NodeVelocity.magnitude;
                }

            }
        }

        Nodes = newNodes.Clone() as Node[,];
    }

}