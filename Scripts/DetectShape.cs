using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class DetectShape: MonoBehaviour
{
    private List<Vector3> drawingPositions = new List<Vector3>();
    public Vector3 mousePos;
    public Vector3 lastmousePos;
    public Text shapeDisplay;
    public Utils utils;
    private Vector3 zeroVector = new Vector3(1, 1, 0);
    private List<float> aux = new List<float>();


    [Header("Tolerancies")]
    public float minimumDistance = 10f;
    public float tolerance = 30f;
    private float distance = 0;
    public float startDelay = 4;
    private float delay = 0;


    [Header("Debugging")]
    public float pain = 0;
    public float angle;
    public float angleDb1;
    public float angleDb2;
    public float angleVariaton = 0;
    private float pendent;
    private float raise, run;
    public List<float> angles_drawing = new List<float>();
    public List<float> angles_Variaton = new List<float>();


    [Header("Formes")]
    private bool newAngle = true;
    private float acum = 0;
    private float asum = 0;
    private float bacum = 0;
    private float basum = 0;
    private float arekt = 0;
    public float minAcum = 20;


    [Header("Shape detecting")]
    private bool stopedDrawing;
    public static float correctShape = 0;
    public float angleTolerance = 10;
    public static float angleDiferencie = 0;

    //2 sided shapes:
    private float[] Leap = { 120 };
    private float[] Fall = { 60 };

    //3 sided shapes:
    private float[] Triangle = { 120, 120 };
    private float[] Break = { 90, 90 };

    //4 sided shapes:
    private float[] Quadrat = { 90, 90, 90 };
    private float[] Rellotge = { 120, 120, 120 };

    //5 sided shapes:
    private float[] Pentagon = { 70, 70, 70, 70 };
    private float[] Eye = { 45, 45, 120, 45 };
    private float[] Invis = { 90, 90, 90, 90 };

    void Update()
    {
        if (drawingMenu.isDrawing)
        {
            stopedDrawing = true;
            if (Input.GetMouseButtonDown(0))
            {
                correctShape = 0f;
                angles_drawing.Clear();
                angles_Variaton.Clear();
                mousePos = Input.mousePosition;
                lastmousePos = mousePos;
            }
            else if (Input.GetMouseButton(0))
            {
                mousePos = Input.mousePosition;
                distance = Vector3.Distance(mousePos, lastmousePos);
                if (distance >= minimumDistance)
                {
                    run = mousePos.x - lastmousePos.x;
                    raise = mousePos.y - lastmousePos.y;
                    pendent = raise / run;

                    angle = utils.GetAngleFromVector(raise, run);

                    //Angle variation
                    angleVariaton = utils.GetAngleDiference(angle, arekt);

                    //Angle detection
                    if (newAngle || angleVariaton < tolerance)
                    {
                        acum++;
                        asum += angle;
                        arekt = asum / acum;
                        newAngle = false;
                    }
                    else if (acum > minAcum)
                    {
                        if (angleVariaton > tolerance)
                            angles_drawing.Add(asum / acum);
                        basum = asum;
                        bacum = acum;
                        asum = angle;
                        acum = 1;
                        newAngle = true;
                    }
                    else
                    {
                        acum = 1;
                        asum = angle;
                        newAngle = true;
                    }
                    lastmousePos = mousePos;
                }
            }
        }
        else if (stopedDrawing)
        {
            stopedDrawing = false;
            angles_drawing.Add(asum / acum);
            detectVariationOnVertex(angles_drawing);
            CalculateShapeVariation(angles_Variaton);

            string Stext;
            Stext = correctShape.ToString("0.0");
            shapeDisplay.text = Stext;

            angles_Variaton.Clear();
            angles_drawing.Clear();
            delay = startDelay;
            newAngle = true;
            arekt = 0;
            asum = 0;
            acum = 0;
        }
    }

    void detectVariationOnVertex(List<float> angles)
    {
        for (int i = 0; i < angles.Count - 1; i++)
        {
            float angleVar = utils.GetAngleDiference(angles[i + 1], angles[i]);
            angles_Variaton.Add(angleVar);
        }
    }

    //Shape detecting 
    void CalculateShapeVariation(List<float> angles_Variaton_Shape)
    {
        int size = angles_Variaton_Shape.Count;
        switch (size)
        {
            case 1:
                GetCorrectShapeByAngles(size, Leap, angles_Variaton_Shape, 2.1f);
                if (correctShape == 2.1f)
                    break;
                GetCorrectShapeByAngles(size, Fall, angles_Variaton_Shape, 2.2f);
                break;

            case 2:
                GetCorrectShapeByAngles(size, Triangle, angles_Variaton_Shape, 3.1f);
                if (correctShape == 3.1f)
                    break;
                GetCorrectShapeByAngles(size, Break, angles_Variaton_Shape, 3.2f);
                break;

            case 3:
                GetCorrectShapeByAngles(size, Quadrat, angles_Variaton_Shape, 4.1f);
                if (correctShape == 4.1f)
                    break;
                GetCorrectShapeByAngles(size, Rellotge, angles_Variaton_Shape, 4.2f);
                break;

            case 4:
                GetCorrectShapeByAngles(size, Pentagon, angles_Variaton_Shape, 5.1f);
                if (correctShape == 5.1f)
                    break;
                GetCorrectShapeByAngles(size, Eye, angles_Variaton_Shape, 5.2f);
                if (correctShape == 5.2f)
                    break;
                GetCorrectShapeByAngles(size, Invis, angles_Variaton_Shape, 5.3f);
                break;
        }
    }

    void GetCorrectShapeByAngles(int size, float[] shape, List<float> aVShape, float shapeId)
    {
        angleDiferencie = 0;
        for (int i1 = 0; i1 < size; i1++)
        {
            float var = utils.GetAngleDiference(aVShape[i1], shape[i1]);
            if (var < angleTolerance)
            {
                angleDiferencie += var;
                correctShape = shapeId;
            }
            else
            {
                correctShape = 0;
                break;
            }
        }
    }
}
