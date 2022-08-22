using System;
using System.Collections.Generic;

[Serializable]
public class Entry
{
    public MolecularView molecularView;
    public float entryVolume;
 
    public Entry(MolecularViewSO so, float entryVolume)
    {
        this.molecularView = new MolecularView(so);
        this.entryVolume = entryVolume;
    }

    public Entry(MolecularView moleculeView, float entryVolume)
    {
        this.molecularView = moleculeView;
        this.molecularView.color = moleculeView.color;
        this.molecularView.density = moleculeView.density;
        this.molecularView.phase = moleculeView.phase;
        this.molecularView.temperature = moleculeView.temperature;
        this.entryVolume = entryVolume;
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