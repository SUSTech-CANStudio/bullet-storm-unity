using System;
using System.Collections.Generic;
using System.Linq;
using XNode;

namespace CANStudio.BulletStorm.XNodes
{
    /// <summary>
    ///     A tool for managing <see cref="XNode.Node" /> dynamic ports.
    ///     When using this class, don't manage dynamic ports yourself.
    /// </summary>
    /// <typeparam name="T">Key type.</typeparam>
    public class PortRegistry<T>
    {
        private readonly Dictionary<T, Action<NodePort[]>> actionDict = new Dictionary<T, Action<NodePort[]>>();
        private readonly Node.ConnectionType inputConnectionType;
        private readonly Node node;
        private readonly Node.ConnectionType outputConnectionType;
        private readonly Dictionary<T, (Type, string, bool)[]> portDict = new Dictionary<T, (Type, string, bool)[]>();
        private readonly Node.TypeConstraint typeConstraint;

        public PortRegistry(Node node, Node.ConnectionType inputConnectionType = Node.ConnectionType.Override,
            Node.ConnectionType outputConnectionType = Node.ConnectionType.Multiple,
            Node.TypeConstraint typeConstraint = Node.TypeConstraint.Inherited)
        {
            this.node = node;
            this.inputConnectionType = inputConnectionType;
            this.outputConnectionType = outputConnectionType;
            this.typeConstraint = typeConstraint;
        }

        /// <summary>
        ///     Currently activate key.
        /// </summary>
        public T CurrentKey { get; private set; }

        /// <summary>
        ///     Register a key with ports.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="ports">Type, fieldName and isInput.</param>
        public void RegisterPorts(T key, params (Type, string, bool)[] ports)
        {
            portDict.Add(key, ports);
        }

        /// <summary>
        ///     Register a key with actions.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="actions">Takes ports as input.</param>
        public void RegisterActions(T key, params Action<NodePort[]>[] actions)
        {
            actionDict.Add(key, actions.Aggregate((first, second) => first + second));
        }

        /// <summary>
        ///     Activate ports registered with <see cref="key" /> and close all other dynamic ports.
        /// </summary>
        /// <param name="key"></param>
        public void Activate(T key)
        {
            if (!portDict.TryGetValue(key, out var ports))
            {
                node.ClearDynamicPorts();
                return;
            }

            var toRemove = node.DynamicPorts.Where(port => ports.All(tuple => tuple.Item2 != port.fieldName)).ToArray();

            foreach (var port in toRemove) node.RemoveDynamicPort(port);

            foreach (var (type, fieldName, isInput) in ports)
            {
                if (node.DynamicPorts.Any(port => port.fieldName == fieldName)) continue;

                if (isInput) node.AddDynamicInput(type, inputConnectionType, typeConstraint, fieldName);
                else node.AddDynamicOutput(type, outputConnectionType, typeConstraint, fieldName);
            }

            CurrentKey = key;
        }

        /// <summary>
        ///     Invoke actions in <see cref="CurrentKey" />.
        /// </summary>
        public void Invoke()
        {
            if (!actionDict.TryGetValue(CurrentKey, out var action)) return;

            if (portDict.TryGetValue(CurrentKey, out var port) && !(port is null))
            {
                action.Invoke(port.Select(tuple => node.GetPort(tuple.Item2)).ToArray());
                return;
            }

            action.Invoke(null);
        }
    }
}