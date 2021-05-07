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
        Node upperLeft, upperRight, lowerLeft, lowerRight;
        Vector3 midLeft, midDown, midRight, midUp;

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
        }

        private Vector3 CalcMidPoint(Node n1, Node n2)
        {
            return Vector3.Lerp(n1.position, n2.position, 0.5f);
        }

        private Vector3 CalcMidPoint(Node n1, Node n2, float iso)
        {
            return Vector3.Lerp(n1.position, n2.position, iso);
        }
    }

    public Toggle upperLeftToggle, upperRightToggle, lowerLeftToggle, lowerRightToggle;
    private Node upperLeft, upperRight, lowerLeft, lowerRight;
    public Text caseText;

    public new Camera camera;

    private int caseNumber;

    // Start is called before the first frame update
    void Start()
    {
        upperLeft = new Node(upperLeftToggle.transform.position, false);
        upperRight = new Node(upperRightToggle.transform.position, false);
        lowerLeft = new Node(lowerLeftToggle.transform.position, false);
        lowerRight = new Node(lowerRightToggle.transform.position, false);

        upperLeftToggle.onValueChanged.AddListener(delegate { ChangeNodeActivation(upperLeftToggle, upperLeft); });
        upperRightToggle.onValueChanged.AddListener(delegate { ChangeNodeActivation(upperRightToggle, upperRight); });
        lowerLeftToggle.onValueChanged.AddListener(delegate { ChangeNodeActivation(lowerLeftToggle, lowerLeft); });
        lowerRightToggle.onValueChanged.AddListener(delegate { ChangeNodeActivation(lowerRightToggle, lowerRight); });

        caseNumber = 0;

        Vector3 start = new Vector3(0, 2, 0);
        Vector3 end = new Vector3(5, 0, 0);
        //float thickness = 2.0f;

        Vector3 directionLine = end - start;
        Vector3 directionCamera = camera.transform.position - start;

        Vector3 cross = Vector3.Cross(directionLine, directionCamera);

        Vector3[] vertices = { start, cross, start, end };
        int[] indices = { 0, 1, 2, 3 };

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.SetIndices(indices, MeshTopology.Lines, 0);

        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void ChangeNodeActivation(Toggle change, Node n)
    {
        n.isActive = change.isOn;
        caseNumber = calcCase();
        caseText.text = $"Case: {caseNumber}";
    }

    private int calcCase()
    {
        int _caseNumber = 0;
        if (upperLeft.isActive) _caseNumber |= 8;
        if (upperRight.isActive) _caseNumber |= 4;
        if (lowerRight.isActive) _caseNumber |= 2;
        if (lowerLeft.isActive) _caseNumber |= 1;
        
        return _caseNumber;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
