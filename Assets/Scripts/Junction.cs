using System.Linq;
using UnityEngine;

public class Junction : MonoBehaviour
{
    [SerializeField]
    private PathNode node;

    private PathNode[] nextNodes;
    private int index = 0;

    void Start()
    {
        nextNodes = node.Next.ToArray();
        if(nextNodes.Length > 0)
        {
            foreach (PathNode next in nextNodes)
            {
                next.NodeEnabled = false;
            }

            nextNodes[0].NodeEnabled = true;
        }
    }

    void OnClick()
    {
        Debug.Log("On click");

        nextNodes[index].NodeEnabled = false;
        index = (index + 1) % nextNodes.Length;
        nextNodes[index].NodeEnabled = true;
    }
}
