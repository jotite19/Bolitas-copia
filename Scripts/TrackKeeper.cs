using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackKeeper : MonoBehaviour
{
    private float time;
    public float period;
    public float timeBack;
    [SerializeField] Transform location;
    public static List<Vector3> track = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        time += Time.deltaTime;

        if (time >= period)
        {
            time = 0.0f;
            track.Add(location.position);
            if (track.Count == timeBack)
                track.RemoveAt(0);
        }
    }

    public void goBackInTime(int seconds)
    {
        location.position = track[0];
    }
}
