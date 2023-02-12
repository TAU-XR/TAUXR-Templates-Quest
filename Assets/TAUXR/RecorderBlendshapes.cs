using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class RecorderBlendshapes : MonoBehaviour
{
    public bool IsRecording;
    private bool isFirstRecordingFrame = false;

    public string PathBlendshapes;
    StreamWriter writer;

    void Awake()
    {
        PathBlendshapes = $"Assets/Resources/TransformRecordingData/{gameObject.name}_BlendshapesRecording.csv";
        IsRecording = false;
    }

    void Update()
    {
        
    }

    public void StartRecording()
    {
        if (isFirstRecordingFrame)
        {
            // initiate the new writer
            writer = new StreamWriter(PathBlendshapes, false);
            isFirstRecordingFrame = false;
        }

        IsRecording = true;
    }

    private void RecordDataToLists()
    {
        
    }

}
