using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCasting : MonoBehaviour
{
    public PlayerMovement playerMove;
    public TrackKeeper trackKeeper;
    public BlockCasting blockCasting;
    public Shooting shooting;

    void Start()
    {
        
    }

    void Update()
    {
        if (DetectShape.correctShape != 0.0)
        {
            switch (DetectShape.correctShape)
            {
                case 2.1f:
                    playerMove.Fall(DetectShape.angleDiferencie);
                    break;
                case 2.2f:
                    playerMove.Leap(DetectShape.angleDiferencie);
                    break;
                case 3.1f:
                    shooting.TriangleShooting();
                    break;
                case 3.2f:
                    break;
                case 4.1f:
                    blockCasting.evoqueBlock();
                    break;
                case 4.2f:
                    trackKeeper.goBackInTime(1);
                    break;
            }
            DetectShape.correctShape = 0.0f;
        }
    }
}
