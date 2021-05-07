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

    public Toggle upperLeftToggle, upperRightToggle, lowerLeftToggle, lowerRightToggle;
    private Node upperLeft, upperRight, lowerLeft, lowerRight;
    public Text caseText;

    private Vector3 startVertex, mousePos;
    public Material mat;

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
