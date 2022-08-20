using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseContainer : MonoBehaviour
{
    public event Action<float> OnBaseVolumeChanged = delegate { };

    public float containerVolume;

    //[HideInInspector]
    public float filledVolume;

    private List<Entry> m_entries;

    public List<Entry> entries { get => m_entries; set { m_entries = value;  } }
   
    public void AssignEntryGroup(List<Entry> entries)
    {
        this.entries = entries;
    }

    public virtual void EntriesVolumeUpdate()
    {
        
    }

    public bool IsVolumeLowerAndEqual(List<Entry> entries)
    {
        return SummVolume(entries) <= containerVolume;
    }

    public static float SummVolume(List<Entry> entries)
    {
        float sum = 0;
        for (int i = 0; i < entries.Count; i++)
        {
            sum += entries[i].entryVolume;
        }
        return sum;
    }
}