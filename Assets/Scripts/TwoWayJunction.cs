using UnityEngine;

public class TwoWayJunction : MonoBehaviour
{
    public enum Direction
    {
        FirstNode,
        SecondNode
    }

    [System.Serializable]
    public class NodeInfo
    {
        public PathNode node;
        public float rotation;
    }

    [SerializeField]
    private NodeInfo node1;

    [SerializeField]
    private NodeInfo node2;

    [SerializeField]
    private Direction direction = Direction.FirstNode;

    void Start()
    {
        SetStateForDirection(direction);
    }

    void OnClick()
    {
        direction = direction == Direction.FirstNode ? Direction.SecondNode : Direction.FirstNode;
        SetStateForDirection(direction);
    }

    private void SetStateForDirection(Direction direction)
    {
        node1.node.NodeEnabled = false;
        node2.node.NodeEnabled = false;

        NodeInfo info = NodeInfoForDirection(direction);
        if (info != null)
        {
            Vector3 rotation = transform.eulerAngles;
            rotation.y = info.rotation;
            transform.eulerAngles = rotation;

            info.node.NodeEnabled = true;
        }
    }

    private NodeInfo NodeInfoForDirection(Direction direction)
    {
        switch(direction)
        {
            case Direction.FirstNode:
                return node1;
            case Direction.SecondNode:
                return node2;
            default:
                Debug.Log("Invalid direction");
                return null;
        }
    }
}
