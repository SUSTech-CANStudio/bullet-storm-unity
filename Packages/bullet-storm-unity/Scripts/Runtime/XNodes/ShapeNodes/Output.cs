using CANStudio.BulletStorm.Emission;
using CANStudio.BulletStorm.Util;
using XNode;

namespace CANStudio.BulletStorm.XNodes.ShapeNodes
{
    [CreateNodeMenu("BulletStorm/Shape/Output"), NodeTint(Utils.ColorShapeOutput), NodeWidth(80)]
    public class Output : Node
    {
        [Input(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Inherited)]
        public Shape shape;

        public void Build()
        {
            var port = GetInputPort(nameof(shape));
            if (!port.IsConnected)
            {
                BulletStormLogger.LogWarning("The output node has no input value, check if you forget to connect it.");
                return;
            }

            if (!(port.Connection.node is ShapeNode lastNode) || !lastNode)
            {
                BulletStormLogger.LogError($"Unknown output type {port.Connection.node.GetType().FullName}, expected {typeof(ShapeNode).FullName}");
                return;
            }
            
            lastNode.RecursiveGenerate();
            if (!(graph is ShapeGraph shapeGraph) || !shapeGraph)
            {
                BulletStormLogger.LogError($"Unknown graph type {graph.GetType().FullName}, this node is output node for {typeof(ShapeAsset).FullName}");
                return;
            }

            shapeGraph.shape = lastNode.GetShape();
        }
    }
}