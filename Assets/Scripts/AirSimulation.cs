using UnityEngine;
using Sirenix.OdinInspector;

public class AirSimulation : MonoBehaviour {
    [SerializeField] private Vector2Int _gridSize;
    [SerializeField] private float _initialDensity;
    [SerializeField] private Vector2 _initialVelocity;

    private bool _isSimulationRunning = true;

    public Node[] Nodes { get; set; }
    private Node[] previousNodes { get; set; }

    [Button("Generate Node Grid")]
    public void GenerateNodeGrid() {
        Nodes = new Node[(_gridSize.x + 2) * (_gridSize.y + 2)];
        previousNodes = new Node[(_gridSize.x + 2) * (_gridSize.y + 2)];
        
        for (int i = 0; i < Nodes.Length; i++) {
            Nodes[i].NodeVelocity = _initialVelocity;
            Nodes[i].Density = _initialDensity;    
        }
    }

    [Button("Step Simulation")]
    private void Step() {
        Diffuse(0, .5f, .5f);
    }

    private void Diffuse (int b, float diff, float dt ) {
        float a = dt * diff * _gridSize.x * _gridSize.y;

        for (int k = 0; k < 20; k++) {
            for (int i = 1; i <= _gridSize.x; i++) {
                for (int j = 1; j <= _gridSize.y; j++) {
                    // x[IX(i,j)] =
                    //     (x0[IX(i,j)] +
                    //     a*(x[IX(i-1,j)] +
                    //     x[IX(i+1,j)] +
                    //     x[IX(i,j-1)] +
                    //     x[IX(i,j+1)]))/(1+4*a);

                    Nodes[IX(i, j)].Density = 
                        (previousNodes[IX(i, j)].Density + 
                        a * (Nodes[IX(i - 1, j)].Density +
                        Nodes[IX(i + 1, j)].Density + 
                        Nodes[IX(i, j - 1)].Density + 
                        Nodes[IX(i, j + 1)].Density)) / (1 + 4 * a);
                }
            }

            SetBoundaries (b);
        }
    }

    void SetBoundaries (int b) {

        for (int i = 1; i <= _gridSize.x; i++) {
            Nodes[IX(0, i)].Density
                = b == 1
                ? -Nodes[IX(1, i)].Density
                : Nodes[IX(1, i)].Density;

            Nodes[IX(_gridSize.x + 1, i)].Density
                = b == 1
                ? -Nodes[IX(_gridSize.x, i)].Density
                : Nodes[IX(_gridSize.x, i)].Density;

            Nodes[IX(i, 0)].Density
                = b == 2
                ? -Nodes[IX(i, 1)].Density
                : Nodes[IX(i, 1)].Density;

            Nodes[IX(i, _gridSize.x + 1)].Density
                = b == 2
                ? -Nodes[IX(i, _gridSize.x)].Density
                : Nodes[IX(i, _gridSize.x)].Density;

            // x[IX(0 ,i)] = b==1 ? –x[IX(1,i)] : x[IX(1,i)];
            // x[IX(N+1,i)] = b==1 ? –x[IX(N,i)] : x[IX(N,i)];
            // x[IX(i,0 )] = b==2 ? –x[IX(i,1)] : x[IX(i,1)];
            // x[IX(i,N+1)] = b==2 ? –x[IX(i,N)] : x[IX(i,N)];
        }

        Nodes[IX(0, 0)].Density = .5f * (Nodes[IX(1, 0)].Density + Nodes[IX(0, 1)].Density);
        Nodes[IX(0, _gridSize.y + 1)].Density = .5f * (Nodes[IX(1, _gridSize.y + 1)].Density + Nodes[IX(0, _gridSize.y)].Density);
        Nodes[IX(_gridSize.x + 1, 0)].Density = .5f * (Nodes[IX(_gridSize.x, 0)].Density + Nodes[IX(_gridSize.x + 1, 1)].Density);
        Nodes[IX(_gridSize.x + 1, _gridSize.y + 1)].Density = .5f * (Nodes[IX(_gridSize.x, _gridSize.y + 1)].Density + Nodes[IX(_gridSize.x + 1, _gridSize.y)].Density);

        // x[IX(0 ,0 )] = 0.5*(x[IX(1,0 )]+x[IX(0 ,1)]);
        // x[IX(0 ,N+1)] = 0.5*(x[IX(1,N+1)]+x[IX(0 ,N )]);
        // x[IX(N+1,0 )] = 0.5*(x[IX(N,0 )]+x[IX(N+1,1)]);
        // x[IX(N+1,N+1)] = 0.5*(x[IX(N,N+1)]+x[IX(N+1,N )]);
    }


    private int IX (int xPos, int yPos) {
        return yPos + (_gridSize.y + 2) * xPos;
    }

}