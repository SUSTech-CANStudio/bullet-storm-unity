
using CANStudio.BulletStorm.Util;
using CANStudio.BulletStorm.XNodes.ShapeNodes;
using XNode;

namespace CANStudio.BulletStorm.XNodes
{
    public static class Utils
    {
        public const string ColorMathNoun = "#442200";
        public const string ColorMathVerb = "#c7c4b8";

        /// <summary>
        /// 
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
            BulletStormLogger.LogError($"Node {port.node}: port {port.fieldName} not connected");
            return false;
        }
    }
}