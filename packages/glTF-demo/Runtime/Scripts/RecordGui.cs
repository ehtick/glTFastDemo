using System;
using System.IO;
using UnityEngine;
using UnityGLTF;
using UnityGLTF.Timeline;

public class RecordGui : MonoBehaviour
{
    public ExportScreenshot fileExport;
    private Transform root;

    public void SetSceneRoot(Transform root)
    {
        this.root = root;
    }
    
    private void OnGUI()
    {
        var w = Screen.width;
        var h = Screen.height;

        if ((recorder == null || !recorder.IsRecording) && GUI.Button(new Rect(w - 150, h - 100, 150, 20), "Start Recording"))
        {
            StartRecording();
        }
        if ((recorder != null && recorder.IsRecording) && GUI.Button(new Rect(w - 150, h - 100, 150, 20), "Stop & Download"))
        {
            StopRecordingAndDownloadAsGlb();
        }
        if (GUI.Button(new Rect(w - 150, h - 100 + 20, 150, 20), "Export"))
        {
            DownloadAsGlb();
        }
        if (GUI.Button(new Rect(w - 150, h - 100 + 40, 150, 20), "Export with Animation"))
        {
            RecordAllAnimationAndDownloadAsGlb();
        }
    }

    private void RecordAllAnimationAndDownloadAsGlb()
    {
        var animation = root.GetComponentInChildren<Animation>();
        animation.Rewind();
        var length = animation.clip.length;
        recorder = new GLTFRecorder(root, true);
        for (var t = 0f; t <= length; t += 1 / 60f)
        {
            foreach (AnimationState state in animation)
                state.time = t;
            
            animation.Sample();
            
            if(t == 0) 
                recorder.StartRecording(0);
            else
                recorder.UpdateRecording(t);
        }
        StopRecordingAndDownloadAsGlb();
    }

    private void DownloadAsGlb()
    {
        var previousExportDisabledState = GLTFSceneExporter.ExportDisabledGameObjects;
        var previousExportAnimationState = GLTFSceneExporter.ExportAnimations;
        GLTFSceneExporter.ExportDisabledGameObjects = true;
        GLTFSceneExporter.ExportAnimations = false;

        var exporter = new GLTFSceneExporter(new Transform[] { root }, new ExportOptions()
        {
            ExportInactivePrimitives = true,
        });
        string filename = "todo.glb";
            
        var data = exporter.SaveGLBToByteArray(filename);

        GLTFSceneExporter.ExportDisabledGameObjects = previousExportDisabledState;
        GLTFSceneExporter.ExportAnimations = previousExportAnimationState;
        
        fileExport.DownloadGLB("todo.glb", data);
    }

    private void StopRecordingAndDownloadAsGlb()
    {
        // TODO add binary callback so we can use this on WebGL
        using (var stream = new MemoryStream()) {
            recorder.EndRecording(stream);
            fileExport.DownloadGLB("todo.glb", stream.GetBuffer());
        }
    }

    private GLTFRecorder recorder;
    
    private void StartRecording()
    {
        recorder = new GLTFRecorder(root, true);
        recorder.StartRecording(Time.realtimeSinceStartupAsDouble);
    }

    private void LateUpdate()
    {
        if(recorder != null && recorder.IsRecording)
            recorder.UpdateRecording(Time.realtimeSinceStartupAsDouble);
    }
}
