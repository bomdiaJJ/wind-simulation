using UnityEngine;
using Shapes;

[ExecuteAlways]
public class NodeRenderer : ImmediateModeShapeDrawer {
    [SerializeField] private AnimationCurve _nodeRadiusToAmountOfAir;
    [SerializeField] private Gradient _nodeColorToAmountOfAir;

    [SerializeField] private AirSimulation _airSimulation;

    public override void DrawShapes(Camera cam) {
        using(Draw.Command(cam)) {
            if (_airSimulation == null) return;
            if (_airSimulation.Nodes == null) return;

            DrawNodes();
        }
    }

    private void DrawNodes() {
        for (int i = 0; i < _airSimulation.Nodes.GetLength(0); i++) {
            for (int j = 0; j < _airSimulation.Nodes.GetLength(1); j++) {
                Vector3 nodePositionInWorld = new Vector3 {
                    x = _airSimulation.Nodes[i,j].NodePosition.x,
                    y = _airSimulation.Nodes[i,j].NodePosition.y,
                    z = 0f,
                };

                Draw.Disc(
                    nodePositionInWorld,
                    _nodeRadiusToAmountOfAir.Evaluate(_airSimulation.Nodes[i,j].Density),
                    _nodeColorToAmountOfAir.Evaluate(_airSimulation.Nodes[i,j].Density)
                );
            }
        }
    }
}