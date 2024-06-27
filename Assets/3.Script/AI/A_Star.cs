using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
    public bool isNotTrack;
    public Node ParentNode;
    public int x, y;
    public int G;
    public int H;
    public int F
    {
        get
        {
            return G + H;
        }
    }
    public Node(bool isNotTrack, int x, int y)
    {
        this.isNotTrack = isNotTrack;
        this.x = x;
        this.y = y;
    }
}

public class A_Star : MonoBehaviour
{
    public GameObject Startpos_ob, Endpos_ob;
    public GameObject BottomLeft_ob, TopRight_ob;

    private Vector2Int bottomLeft, topRight, start_Pos, end_Pos;
    public bool AllowDiagonal = true;
    public bool DontCrossCorner = true;

    private int Size_X, Size_Y;
    private Node[,] NodeArray;
    private Node StartNode, EndNode, CurrentNode;

    [SerializeField] private List<Node> Final_List;
    private List<Node> OpenList, ClosedList;

    public GameObject AI;

    private void Start()
    {
        PathFinding();
        MoveAI();
    }

    public void SetPosition()
    {
        start_Pos = new Vector2Int((int)Startpos_ob.transform.position.x, (int)Startpos_ob.transform.position.y);
        end_Pos = new Vector2Int((int)Endpos_ob.transform.position.x, (int)Endpos_ob.transform.position.y);
        bottomLeft = new Vector2Int((int)BottomLeft_ob.transform.position.x, (int)BottomLeft_ob.transform.position.y);
        topRight = new Vector2Int((int)TopRight_ob.transform.position.x, (int)TopRight_ob.transform.position.y);
    }
    public void PathFinding()
    {
        SetPosition();
        Size_X = topRight.x - bottomLeft.x + 1;
        Size_Y = topRight.y - bottomLeft.y + 1;

        NodeArray = new Node[Size_X, Size_Y];
        for (int i = 0; i < Size_X; i++)
        {
            for (int j = 0; j < Size_Y; j++)
            {
                bool isnottrack = false;
                foreach (Collider2D col in Physics2D.OverlapCircleAll(new Vector2(i + bottomLeft.x, j + bottomLeft.y), 0.4f))
                {
                    if (col.gameObject.layer.Equals(LayerMask.NameToLayer("Bounds")))
                    {
                        isnottrack = true;
                    }
                }
                NodeArray[i, j] = new Node(isnottrack, i + bottomLeft.x, j + bottomLeft.y);
            }
        }

        StartNode = NodeArray[start_Pos.x - bottomLeft.x, start_Pos.y - bottomLeft.y];
        EndNode = NodeArray[end_Pos.x - bottomLeft.x, end_Pos.y - bottomLeft.y];

        OpenList = new List<Node>();
        ClosedList = new List<Node>();
        Final_List = new List<Node>();

        OpenList.Add(StartNode);

        while (OpenList.Count > 0)
        {
            CurrentNode = OpenList[0];
            for (int i = 0; i < OpenList.Count; i++)
            {
                if (OpenList[i].F <= CurrentNode.F && OpenList[i].H < CurrentNode.H)
                {
                    CurrentNode = OpenList[i];
                }
                OpenList.Remove(CurrentNode);
                ClosedList.Add(CurrentNode);

                if (CurrentNode == EndNode)
                {
                    Node target = EndNode;
                    while (target != StartNode)
                    {
                        Final_List.Add(target);
                        target = target.ParentNode;
                    }
                    Final_List.Add(StartNode);
                    Final_List.Reverse();
                    return;
                }
                if (AllowDiagonal)
                {
                    Openlist_Add(CurrentNode.x + 1, CurrentNode.y - 1);
                    Openlist_Add(CurrentNode.x - 1, CurrentNode.y + 1);
                    Openlist_Add(CurrentNode.x + 1, CurrentNode.y + 1);
                    Openlist_Add(CurrentNode.x - 1, CurrentNode.y - 1);
                }
                Openlist_Add(CurrentNode.x + 1, CurrentNode.y);
                Openlist_Add(CurrentNode.x - 1, CurrentNode.y);
                Openlist_Add(CurrentNode.x, CurrentNode.y + 1);
                Openlist_Add(CurrentNode.x, CurrentNode.y - 1);
            }
        }
    }
    public void Openlist_Add(int check_x, int check_y)
    {
        if (check_x >= bottomLeft.x && check_x < topRight.x + 1 && check_y >= bottomLeft.y && check_y < topRight.y + 1
            && !NodeArray[check_x - bottomLeft.x, check_y - bottomLeft.y].isNotTrack && !ClosedList.Contains(NodeArray[check_x - bottomLeft.x, check_y - bottomLeft.y]))
        {
            if (AllowDiagonal)
            {
                if (NodeArray[CurrentNode.x - bottomLeft.x, check_y - bottomLeft.y].isNotTrack &&
                    NodeArray[check_x - bottomLeft.x, CurrentNode.y - bottomLeft.y].isNotTrack)
                {
                    return;
                }
            }
            if (DontCrossCorner)
            {
                if (NodeArray[CurrentNode.x - bottomLeft.x, check_y - bottomLeft.y].isNotTrack ||
                   NodeArray[check_x - bottomLeft.x, CurrentNode.y - bottomLeft.y].isNotTrack)
                {
                    return;
                }
            }
            Node n_Node = NodeArray[check_x - bottomLeft.x, check_y - bottomLeft.y];
            int movecost = CurrentNode.G + (CurrentNode.x - check_x == 0 || CurrentNode.y - check_y == 0 ? 10 : 14);

            if (movecost < n_Node.G || !OpenList.Contains(n_Node))
            {
                n_Node.G = movecost;
                n_Node.H = (Mathf.Abs(n_Node.x - EndNode.x) + Mathf.Abs(n_Node.y - EndNode.y)) * 10;

                n_Node.ParentNode = CurrentNode;
                OpenList.Add(n_Node);
            }
        }
    }

    public void MoveAI()
    {
        StartCoroutine(Move_AI_co());
    }

    private void OnDrawGizmos()
    {
        if (Final_List != null)
        {
            for (int i = 0; i < Final_List.Count - 1; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(new Vector2(Final_List[i].x, Final_List[i].y), new Vector2(Final_List[i + 1].x, Final_List[i + 1].y));
            }
        }
    }

    public IEnumerator Move_AI_co()
    {
        if (Final_List == null) yield break;
        AI.transform.position = new Vector3(Final_List[0].x, Final_List[0].y, 0f);
        for (int i = 0; i < Final_List.Count; i++)
        {
            Vector3 target_pos = new Vector3(Final_List[i].x, Final_List[i].y, 0f);
            while (true)
            {
                AI.transform.position = Vector3.Lerp(AI.transform.position, target_pos, 0.01f);
                yield return null;
                if (Vector3.Distance(AI.transform.position, target_pos) <= 0.05f)
                {
                    AI.transform.position = target_pos;
                    break;
                }
            }
        }
        yield return null;
    }
}
