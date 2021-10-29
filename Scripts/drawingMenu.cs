using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drawingMenu : MonoBehaviour
{
    public static bool startedDrawing = false;
    public static bool stopedDrawing = false;
    public bool startedDrawingDB = false;
    public bool stopedDrawingDB = false;
    public static bool isDrawing = false;
    public GameObject drawingMenuUI;

    // Update is called once per frame
    void Update()
    {
        stopedDrawing = false;
        startedDrawing = false;
        if (Input.GetKey(KeyCode.E))
        {
            if (!isDrawing)
                startedDrawing = true;
            isDrawing = true;
            drawingMenuUI.SetActive(true);
            //Time.timeScale = 0.5f;
        }
        else 
        {
            if (isDrawing)
                stopedDrawing = true;
            isDrawing = false;
            drawingMenuUI.SetActive(false);
            //Time.timeScale = 1f;
        }
        startedDrawingDB = startedDrawing;
        stopedDrawingDB = stopedDrawing;
    }
}
