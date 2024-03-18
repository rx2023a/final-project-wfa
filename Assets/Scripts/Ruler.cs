using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Ruler : MonoBehaviour
{
    public LineRenderer rulerLine;
    public Transform pos1;
    public Transform pos2;
    public TMP_Text lenghtText;

    void Start()
    {
        rulerLine.positionCount = 2;
    }

    void Update()
    {
        rulerLine.SetPosition(0, pos1.position);
        rulerLine.SetPosition(1, pos2.position);

        float length = Vector3.Distance(pos1.position, pos2.position);
        float lengthCm = length * 100; // Assuming 1 Unity unit = 1 m

        lenghtText.text = lengthCm.ToString("F2") + " cm";
    }
}
