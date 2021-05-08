using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LookUpGenerator : MonoBehaviour
{

    // Node of a square required for the marching squares algorithm
    public class Node
    {
        public Vector3 position { get; set; }
        public bool isActive { get; set; }

        public Node(Vector3 _pos, bool _isActive)
        {
            position = _pos;
            isActive = _isActive;
        }

        public override string ToString()
        {
            return $"{position}, {isActive}";
        }
    }

    public class NodeSquare
    {
        readonly Node upperLeft, upperRight, lowerLeft, lowerRight;
        Vector3 midLeft, midDown, midRight, midUp;

        int caseNumber;

        public NodeSquare(Node uLeft, Node uRight, Node lLeft, Node lRight)
        {
            upperLeft = uLeft;
            upperRight = uRight;
            lowerLeft = lLeft;
            lowerRight = lRight;

            midLeft = CalcMidPoint(upperLeft, lowerLeft);
            midDown = CalcMidPoint(lowerLeft, lowerRight);
            midRight = CalcMidPoint(upperRight, lowerRight);
            midUp = CalcMidPoint(upperLeft, upperRight);

            caseNumber = 0;
        }

        private Vector3 CalcMidPoint(Node n1, Node n2)
        {
            return Vector3.Lerp(n1.position, n2.position, 0.5f);
        }

        private Vector3 CalcMidPoint(Node n1, Node n2, float iso)
        {
            return Vector3.Lerp(n1.position, n2.position, iso);
        }

        public void ChangeNodeActivation(Toggle change, Node n, Text text)
        {
            n.isActive = change.isOn;
            caseNumber = CalcCaseNumber();
            text.text = $"Case: {caseNumber}";
        }

        public int CalcCaseNumber()
        {
            int caseNumber = 0;
            if (upperLeft.isActive) caseNumber |= 8;
            if (upperRight.isActive) caseNumber |= 4;
            if (lowerRight.isActive) caseNumber |= 2;
            if (lowerLeft.isActive) caseNumber |= 1;

            return caseNumber;
        }

        public Mesh CalcLineMesh()
        {
            Mesh mesh = new Mesh();
            Vector3[] vertices = CalcLineVertices();
            int[] indices = new int[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                indices[i] = i;
            }

            mesh.vertices = vertices;
            mesh.SetIndices(indices, MeshTopology.Lines, 0);
            return mesh;
        }

        private Vector3[] CalcLineVertices()
        {
            List<Vector3> vertices = new List<Vector3>();
            switch(caseNumber)
            {
                // no node active
                case 0:
                    break;

                // 1 node active 
                case 1:
                    vertices.Add(midLeft);
                    vertices.Add(midDown);
                    break;
                case 2:
                    vertices.Add(midDown);
                    vertices.Add(midRight);
                    break;
                case 4:
                    vertices.Add(midUp);
                    vertices.Add(midRight);
                    break;
                case 8:
                    vertices.Add(midUp);
                    vertices.Add(midLeft);
                    break;

                // 2 nodes active
                case 3:
                    vertices.Add(midLeft);
                    vertices.Add(midRight);
                    break;
                case 6:
                    vertices.Add(midDown);
                    vertices.Add(midUp);
                    break;
                case 9:
                    vertices.Add(midDown);
                    vertices.Add(midUp);
                    break;
                case 12:
                    vertices.Add(midLeft);
                    vertices.Add(midRight);
                    break;

                case 5:
                    vertices.Add(midLeft);
                    vertices.Add(midUp);
                    vertices.Add(midDown);
                    vertices.Add(midRight);
                    break;
                case 10:
                    vertices.Add(midLeft);
                    vertices.Add(midDown);
                    vertices.Add(midUp);
                    vertices.Add(midRight);
                    break;

                // 3 nodes active
                case 7:
                    vertices.Add(midLeft);
                    vertices.Add(midUp);
                    break;
                case 11:
                    vertices.Add(midUp);
                    vertices.Add(midRight);
                    break;
                case 13:
                    vertices.Add(midDown);
                    vertices.Add(midRight);
                    break;
                case 14:
                    vertices.Add(midLeft);
                    vertices.Add(midDown);
                    break;

                // All nodes active
                case 15:
                    break;
            }

            return vertices.ToArray();
        }
    }

    public Toggle upperLeftToggle, upperRightToggle, lowerLeftToggle, lowerRightToggle;
    private Node upperLeft, upperRight, lowerLeft, lowerRight;
    private NodeSquare nSquare;
    public Text caseText;

    public new Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        upperLeft = new Node(upperLeftToggle.transform.position, false);
        upperRight = new Node(upperRightToggle.transform.position, false);
        lowerLeft = new Node(lowerLeftToggle.transform.position, false);
        lowerRight = new Node(lowerRightToggle.transform.position, false);

        nSquare = new NodeSquare(upperLeft, upperRight, lowerLeft, lowerRight);

        upperLeftToggle.onValueChanged.AddListener(delegate { nSquare.ChangeNodeActivation(upperLeftToggle, upperLeft, caseText); });
        upperRightToggle.onValueChanged.AddListener(delegate { nSquare.ChangeNodeActivation(upperRightToggle, upperRight, caseText); });
        lowerLeftToggle.onValueChanged.AddListener(delegate { nSquare.ChangeNodeActivation(lowerLeftToggle, lowerLeft, caseText); });
        lowerRightToggle.onValueChanged.AddListener(delegate { nSquare.ChangeNodeActivation(lowerRightToggle, lowerRight, caseText); });

        upperLeftToggle.onValueChanged.AddListener(RecalculateLineMesh);
        upperRightToggle.onValueChanged.AddListener(RecalculateLineMesh);
        lowerLeftToggle.onValueChanged.AddListener(RecalculateLineMesh);
        lowerRightToggle.onValueChanged.AddListener(RecalculateLineMesh);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void RecalculateLineMesh(bool isOn)
    {
        Mesh mesh = nSquare.CalcLineMesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }
}
