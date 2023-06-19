using System;
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

        for (int x = 0; x < _gridSize.x; x++) {
            for (int y = 0; y < _gridSize.y; y++) {
                // 10   11
                //    v - Node velocity vector
                // 00   01
                int leftIndex = Nodes[x, y].NodePosition.x + Mathf.FloorToInt(Nodes[x, y].NodeVelocity.x);
                int rightIndex = leftIndex + 1;
                int bottomIndex = Nodes[x, y].NodePosition.y + Mathf.FloorToInt(Nodes[x, y].NodeVelocity.y);
                int topIndex = bottomIndex + 1;

                Node? nodeBottomLeft = 
                    leftIndex < _gridSize.x && bottomIndex < _gridSize.y ? Nodes[leftIndex, bottomIndex] : null;
                Node? nodeBottomRight = 
                    rightIndex < _gridSize.x && bottomIndex < _gridSize.y ? Nodes[rightIndex, bottomIndex] : null;
                Node? nodeTopLeft = 
                    leftIndex < _gridSize.x && topIndex < _gridSize.y ? Nodes[leftIndex, topIndex] : null;
                Node? nodeTopRight = 
                    rightIndex < _gridSize.x && topIndex < _gridSize.y ? Nodes[rightIndex, topIndex] : null;

                
            }
        }

        #region Old
        // for (int i = 0; i < _gridSize.x; i++) {
        //     for (int j = 0; j < _gridSize.y; j++) {
        //         float averageDensityBetweenNodeAndNeighbours = Nodes[i, j].Density;
        //         int nodesContributing = 1;
                
        //         // node da esquerda
        //         if (i > 0) {
        //             averageDensityBetweenNodeAndNeighbours += Nodes[i-1, j].Density;
        //             nodesContributing++;
        //         }
                
        //         // node da direita
        //         if (i < _gridSize.x - 1) {
        //             averageDensityBetweenNodeAndNeighbours += Nodes[i + 1, j].Density;
        //             nodesContributing++;
        //         }

        //         // node de cima
        //         if (j > 0) {
        //             averageDensityBetweenNodeAndNeighbours += Nodes[i, j - 1].Density;
        //             nodesContributing++;
        //         }   
                
        //         // node de baixo
        //         if (j < _gridSize.y - 1) {
        //             averageDensityBetweenNodeAndNeighbours += Nodes[i, j+ 1].Density;
        //             nodesContributing++;
        //         }

        //         averageDensityBetweenNodeAndNeighbours /= nodesContributing;

        //         Nodes[i, j].Density = averageDensityBetweenNodeAndNeighbours;

        //         if (i > 0)
        //             Nodes[i - 1, j].Density = averageDensityBetweenNodeAndNeighbours;
        //         if (i < _gridSize.x - 1)
        //             Nodes[i + 1, j].Density = averageDensityBetweenNodeAndNeighbours;
        //         if (j > 0)
        //             Nodes[i, j - 1].Density = averageDensityBetweenNodeAndNeighbours;
        //         if (j < _gridSize.y - 1)
        //             Nodes[i, j + 1].Density = averageDensityBetweenNodeAndNeighbours;
        //     }
        // }
        #endregion
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