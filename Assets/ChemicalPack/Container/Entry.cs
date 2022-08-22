using System;
using System.Collections.Generic;

[Serializable]
public class Entry
{
    public MolecularView molecularView;
    public float entryVolume;
    public float entryMass;
    public Entry(MolecularViewSO so, float entryVolume)
    {
        this.molecularView = new MolecularView(so);
        this.entryVolume = entryVolume;
        this.entryMass = ToMass(molecularView.density, entryVolume);
    }

    public Entry(MolecularView molecularView, float entryVolume)
    {
        this.molecularView = molecularView;
        this.molecularView.color = molecularView.color;
        this.molecularView.density = molecularView.density;
        this.molecularView.phase = molecularView.phase;
        this.molecularView.temperature = molecularView.temperature;
        this.entryVolume = entryVolume;
        this.entryMass = ToMass(molecularView.density, entryVolume);
    }
 
    public static List<Entry> Copy(List<Entry> entries)
    {
        List<Entry> tempCopy = new List<Entry>();
        for (int i = 0; i < entries.Count; i++)
        {
            tempCopy.Add(Copy(entries[i]));
        }

        return tempCopy;
    }

    public static List<Entry> CopyWithZeroVolumeEntries(List<Entry> entries)
    {
        List<Entry> tempCopy = new List<Entry>();
        for (int i = 0; i < entries.Count; i++)
        {
            Entry e = entries[i];
            Registy cas = new Registy(e.molecularView.formula.casId, e.molecularView.formula.chemicalVariant);
            MolecularView mw = new MolecularView(cas, e.molecularView);
            Entry r = new Entry(mw, 0);
            tempCopy.Add(r);
        }

        return tempCopy;
    }

    public float GetEntryMass()
    {
        return ToMass(molecularView.density, entryVolume);
    }

    private static Entry Copy(Entry e)
    {
        Registy cas = new Registy(e.molecularView.formula.casId, e.molecularView.formula.chemicalVariant);
        MolecularView mw = new MolecularView(cas, e.molecularView);
        Entry r = new Entry(mw, e.entryVolume);
        return r;
    }

    private static float ToMass(float dens, float V)
    { return dens * V; }
}