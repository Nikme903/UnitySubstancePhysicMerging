using System;
using System.Collections.Generic;

[Serializable]
public class Entry
{
    public MolecularView molecule;
    public float entryVolume;

    //public Entry(float volume)
    //{
    //    this.molecule = null;
    //    this.entryVolume = volume;
    //}

    public Entry(MolecularViewSO so, float entryVolume)
    {
        this.molecule = new MolecularView(so);
        this.entryVolume = entryVolume;
    }

    public Entry(MolecularView molecule, float entryVolume)
    {
        this.molecule = molecule;
        this.molecule.molecularColor = molecule.molecularColor;
        this.entryVolume = entryVolume;
    }

    //public static Entry Copy(MolecularWrapper molecule, float entryVolume)
    //{
    //    //CASFormula cas = new CASFormula(molecule.formula.casId, molecule.formula.chemicalVariant);
    //    //MolecularWrapper mw = new MolecularWrapper(cas, molecule);
    //    Entry r = new Entry(molecule, entryVolume);
    //    return r;
    //}

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
            Registy cas = new Registy(e.molecule.formula.casId, e.molecule.formula.chemicalVariant);
            MolecularView mw = new MolecularView(cas, e.molecule);
            Entry r = new Entry(mw, 0);
            tempCopy.Add(r);
        }

        return tempCopy;
    }

    public float GetEntryMass()
    {
        return ToMass(molecule.formula.chemicalVariant.density, entryVolume);
    }

    private static Entry Copy(Entry e)
    {
        Registy cas = new Registy(e.molecule.formula.casId, e.molecule.formula.chemicalVariant);
        MolecularView mw = new MolecularView(cas, e.molecule);
        Entry r = new Entry(mw, e.entryVolume);
        return r;
    }

    private static float ToMass(float dens, float V)
    { return dens * V; }
}