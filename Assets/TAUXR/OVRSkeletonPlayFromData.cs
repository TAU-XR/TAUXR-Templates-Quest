using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class OVRSkeletonPlayFromData : MonoBehaviour
{
    public OVRCustomSkeleton skeletonToPlayOn;
    public string DataPathFolderName;
    public int currentFrame = 0;
    public bool bPlay = false;

    string[] paths;

    public List<Vector3>[] BonesPositions;
    public List<Quaternion>[] BonesRotations;
    public List<Vector3>[] BonesScales;

    List<Transform> skeletonBones;
    void Start()
    {
        skeletonBones = skeletonToPlayOn.CustomBones;
        LoadRecordedDataToLists();
        bPlay = false;
    }

    void Update()
    {
        if(bPlay)
            PlaySkeletonData();
    }

    public void LoadRecordedDataToLists()
    {
        // validate a data folder was entered.
        if(DataPathFolderName == null)
        {
            Debug.LogError("Recorded skeleton data path is empty. Please select your data folder");
            return;
        }

        // get to the skeleton data folder
        string SkeletonDataFolderPath = "Assets/Resources/SkeletonRecordings/" + DataPathFolderName + "/";
        string dir = Path.GetDirectoryName(SkeletonDataFolderPath);
        if (!Directory.Exists(dir))
            Debug.LogError("Recorded skeleton data not found -> check your path");

        // init list arrays to the size of bones.
        BonesPositions = new List<Vector3>[skeletonToPlayOn.CustomBones.Count];
        BonesRotations = new List<Quaternion>[skeletonToPlayOn.CustomBones.Count];
        BonesScales = new List<Vector3>[skeletonToPlayOn.CustomBones.Count];

        paths = new string[skeletonToPlayOn.CustomBones.Count];


        for (int boneIndex = 0; boneIndex < skeletonToPlayOn.CustomBones.Count; boneIndex++)
        {

            // init all paths to their indexes
            paths[boneIndex] = SkeletonDataFolderPath + $"BoneRecording_{boneIndex}.csv";
            if(!File.Exists(paths[boneIndex]))
            {
                Debug.LogWarning($"Couldn't find data file BoneRecording_{boneIndex}. Could not load data for this bone");
                break;
            }

            // init a bone list
            BonesPositions[boneIndex] = new List<Vector3>();
            BonesRotations[boneIndex] = new List<Quaternion>();
            BonesScales[boneIndex] = new List<Vector3>();

            // load all bone data into a string array
            string[] allFile = File.ReadAllLines(paths[boneIndex]);
            string[] lineData;

            // extract bone data from string to its bone lists
            for (int frameIndex = 0; frameIndex < allFile.Length; frameIndex++)
            {
                lineData = allFile[frameIndex].Split(',');

                // for some reason some lines are not 10 in length, so ignore them for now
                if (lineData.Length != 10) break;

                Vector3 pos = new Vector3(float.Parse(lineData[0]), float.Parse(lineData[1]), float.Parse(lineData[2]));
                Quaternion rot = new Quaternion(float.Parse(lineData[3]), float.Parse(lineData[4]), float.Parse(lineData[5]), float.Parse(lineData[6]));
                Vector3 scl = new Vector3(float.Parse(lineData[7]), float.Parse(lineData[8]), float.Parse(lineData[9]));

                BonesPositions[boneIndex].Add(pos);
                BonesRotations[boneIndex].Add(rot);
                BonesScales[boneIndex].Add(scl);
            }
        }
        Debug.Log($"Total Lists: {BonesPositions.Length}. Total Frames {BonesPositions[0].Count}. Successfully loaded recorded data to lists. ");
    }

    public void PlaySkeletonData()
    {
        for (int boneIndex = 0; boneIndex < skeletonBones.Count; boneIndex++)
        {
            skeletonBones[boneIndex].localPosition = BonesPositions[boneIndex][currentFrame];
            skeletonBones[boneIndex].localRotation = BonesRotations[boneIndex][currentFrame];
            skeletonBones[boneIndex].localScale = BonesScales[boneIndex][currentFrame];
        }

        currentFrame++;
    }
}
