using System;
using System.Collections.Generic;
[Serializable]
public class MixImage
{
    public string reactionId;
    public List<Entry> mixedEntries;
    public float volume;

    public MixImage(string reactionId, List<Entry> list)
    {
        this.reactionId = reactionId;
        this.mixedEntries = list;

        float volumeSum = 0;
        for (int i = 0; i < list.Count; i++)
        {
            volumeSum += list[i].entryVolume;
        }

        this.volume = volumeSum;
    }
}