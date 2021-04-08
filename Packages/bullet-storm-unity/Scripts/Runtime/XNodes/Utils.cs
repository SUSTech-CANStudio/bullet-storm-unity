using CANStudio.BulletStorm.Util;
using CANStudio.BulletStorm.XNodes.ShapeNodes;
using XNode;

namespace CANStudio.BulletStorm.XNodes
{
    public static class Utils
    {
        public const string ColorMathNoun = "#32519A";
        public const string ColorMathVerb = "#C7C4B8";
        public const string ColorShapeOperation = "#666699";
        public const string ColorShapeOperationSpecial = "#663399";
        public const string ColorShapeGenerator = "#CC6600";
        public const string ColorShapeOutput = "#CC3333";

        public const int OrderSpecialOperation = 0;
        public const int OrderPositionOperation = 100;
        public const int OrderVelocityOperation = 200;
        public const int OrderColorOperation = 300;
        public const int OrderSizeOperation = 400;
        public const int OrderRepeatOperation = 500;

        /// <summary>
        ///     Notifies output port nodes that this node is changed.
        ///     <para />
        ///     Will invoke method `OnValidate` in nodes.
        /// </summary>
        /// <param name="node"></param>
        public static void NotifyChange(this Node node)
        {
            foreach (var port in node.Outputs)
            {
                if (!port.IsConnected) continue;
                foreach (var nextNodePort in port.GetConnections())
                    Util.System.SendMessage(nextNodePort.node, "OnValidate");
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="shapeNode"></param>
        /// <param name="inputPortName"></param>
        /// <returns>Default if failed.</returns>
        public static ShapeNode GetInputShapeNode(this ShapeNode shapeNode, string inputPortName)
        {
            var port = shapeNode.GetInputPort(inputPortName);
            if (port is null)
            {
                BulletStormLogger.LogError($"Node {shapeNode}: port 'inputShape' not found.");
                return default;
            }

            if (!port.CheckConnection()) return default;

            if (port.Connection.node is ShapeNode last) return last;

            BulletStormLogger.LogError($"Node {shapeNode}: unknown input {port.node}.");
            return default;
        }

        public static bool CheckConnection(this NodePort port)
        {
            if (port.IsConnected) return true;
            BulletStormLogger.LogWarning($"Node {port.node}: port {port.fieldName} not connected");
            return false;
        }
    }
}