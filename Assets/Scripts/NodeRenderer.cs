using UnityEngine;
using Shapes;
using Sirenix.OdinInspector;

[ExecuteAlways]
public class NodeRenderer : ImmediateModeShapeDrawer {
    // [SerializeField] private float _velocityLineLength = .35f;
    // [SerializeField] private AnimationCurve _nodeRadiusToAmountOfAir;
    // [SerializeField] private Gradient _nodeColorToAmountOfAir;
    [Title("Settings", "", TitleAlignments.Centered)]
    [SerializeField] private float _physicalDistance = .5f;
    [SerializeField] private float _nodeRadius = .25f;

    [SerializeField] private AirSimulation _airSimulation;

    public override void DrawShapes(Camera cam) {
        using(Draw.Command(cam)) {
            if (_airSimulation == null) return;
            if (_airSimulation.Nodes == null) return;

            DrawNodes();
        }
    }

    private void DrawNodes() {
        for (int i = 0; i < _airSimulation.GridSize.x; i++) {
            for (int j = 0; j < _airSimulation.GridSize.y; j++) {
                int nodeIndex = _airSimulation.IX(i, j);

                Vector3 nodePositionInWorld = new Vector3 {
                    x = i * _physicalDistance,
                    y = j * _physicalDistance,
                    z = 0f,
                };

                Draw.Color = new Color(1f, 1f, 1f, _airSimulation.Nodes[nodeIndex].Density);

                // Density
                Draw.Disc(
                    nodePositionInWorld,
                    _nodeRadius
                );

                // // Velocity
                // Draw.Line(
                //     nodePositionInWorld,
                //     nodePositionInWorld + Vector3.ClampMagnitude((Vector3) _airSimulation.Nodes[i, j].NodeVelocity, _velocityLineLength),
                //     _nodeColorToAmountOfAir.Evaluate(_airSimulation.Nodes[i, j].NodeVelocity.magnitude)
                // );
            }
        }
    }
}