using System.Collections.Generic;
using UnityEngine;

public class ContainerTransitive : BaseContainer
{
    public List<MixImage> imagesQueue;
    public float lastMixedVolume;
    public bool isMixedStatus;

    public List<Entry> lastMixedEntries
    { get { return (imagesQueue.Count > 0) ? imagesQueue[imagesQueue.Count - 1].mixedEntries : null; } }

    private void Start()
    {
        ContainerTransitiveInit();
    }

    public void ContainerTransitiveInit()
    {
        entries = new List<Entry>();
        imagesQueue = new List<MixImage>();
    }

    public void ClearContainer()
    {
        imagesQueue.Clear();
        entries.Clear();

        filledVolume = 0;
    }

    public void ClearContainerQueue()
    {
        imagesQueue.Clear();
    }

    public override void EntriesVolumeUpdate()
    {
        filledVolume = GetFilledVolumeWithLast();
    }

    public void AddEntry(MolecularView molecula, float volumeCubicCm)
    {
        Entry e = IsEntryExist(molecula);
        if (e != null)
        {
            Debug.Log("Компонент уже существует. Добавление ");
            PlusVolumeOfTheEntry(e.molecule, volumeCubicCm);
        }
        else
        {
            entries.Add(new Entry(molecula, volumeCubicCm));
        }
        /* if (e == null)
         {
             entries.Add(new Entry(molecula, volumeCubicCm));
         }
         else
         {
             Debug.Log("компонент внутри уже существует ");
         }*/
    }

    public void RemoveEntry(MolecularView molecula)
    {
        Entry e = IsEntryExist(molecula);
        if (e != null)
        {
            entries.Remove(e);
        }
    }

    public void ResetMixStatus()
    {
        isMixedStatus = false;
    }

    public float GetFilledVolumeWithLast()
    {
        float sum = 0;
        if (entries != null)
        {
            if (entries.Count > 0)
            {
                sum += SummVolume(entries);
            }
        }
        if (lastMixedEntries != null)
        {
            if (lastMixedEntries.Count > 0)
            {
                sum += SummVolume(lastMixedEntries);
            }
        }
        return sum;
    }

    private Entry IsEntryExist(MolecularView molecula)
    {
        Entry e = null;
        if (entries != null)
        {
            e = entries.Find((x) => { return x.molecule.formula.casId == molecula.formula.casId; });
        }
        return e;
    }

    private void PlusVolumeOfTheEntry(MolecularView molecula, float volumeCubicCm)
    {
        Entry e = IsEntryExist(molecula);
        if (e != null)
        {
            e.entryVolume += volumeCubicCm;
        }
        else
        {
            Debug.Log("Не найден компонент внутри");
        }
    }

    public void CreateOperationImage(MixImage image)
    {
        imagesQueue.Add(image);
        ImageInfo(image);
        entries.Clear();

        isMixedStatus = true;
    }

    private static void ImageInfo(MixImage image)
    {
        string s = $"Created next_image:\n";
        for (int i = 0; i < image.mixedEntries.Count; i++)
        {
            s += $"\tgenerated id={image.mixedEntries[i].molecule.formula.casId} volume={image.mixedEntries[i].entryVolume}\n";
        }
        Debug.Log(s);
    }
}