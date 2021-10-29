using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class drawingVisual : MonoBehaviour
{
    public Camera cam = null;
    public LineRenderer lineRenderer = null;
    private Vector3 mousePos;
    private Vector3 Pos;
    private Vector3 previousPos;
    public List<Vector3> linePositions = new List<Vector3>();
    public List<Vector3> worldlinePositions = new List<Vector3>();
    public float minimumDistance = 0.05f;
    private float distance = 0;

    // Update is called once per frame

    void Update()
    {
        if (drawingMenu.isDrawing)
        {
            if (Input.GetMouseButtonDown(0))
            {
                linePositions.Clear();
                mousePos = Input.mousePosition;
                mousePos.z = 5;
                Pos = mousePos;
                previousPos = Pos;
                linePositions.Add(Pos);
            }
            else if (Input.GetMouseButton(0))
            {
                mousePos = Input.mousePosition;
                mousePos.z = 5;
                Pos = mousePos;
                distance = Vector3.Distance(Pos, previousPos);
                if (distance >= minimumDistance)
                {
                    previousPos = Pos;
                    if (linePositions.Count > 75)
                    {
                        linePositions.Add(Pos);
                        linePositions.RemoveAt(0);
                    }
                    else
                        linePositions.Add(Pos);

                    worldlinePositions.Clear();
                    for (int i = 0; i < linePositions.Count; i++)
                    {
                        worldlinePositions.Add(cam.ScreenToWorldPoint(linePositions[i]));
                    }

                    lineRenderer.positionCount = worldlinePositions.Count;
                    lineRenderer.SetPositions(worldlinePositions.ToArray());
                }
            }
        }
        else
        {
            linePositions.Clear();
            lineRenderer.positionCount = linePositions.Count;
            lineRenderer.SetPositions(linePositions.ToArray());
        }
    }
}