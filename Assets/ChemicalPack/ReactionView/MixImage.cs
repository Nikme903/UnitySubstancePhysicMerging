using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class MixImage
{
    [HideInInspector]
    public string reactionId;
    public List<Entry> mixedEntries;
    public List<Entry> MixedEntries { get { return mixedEntries; }   }
    public float volume;
    public float mass;
    //public float averDensity;

    public MixImage(string reactionId, List<Entry> entries)
    {
        this.reactionId = reactionId;
        this.mixedEntries = entries;

        float volumeSum = 0;
        float volumeMass = 0;

        for (int i = 0; i < entries.Count; i++)
        {
            volumeSum += entries[i].entryVolume;
            volumeMass += entries[i].entryMass;
        }

        this.volume = volumeSum;
        this.mass = volumeMass;

    }
}