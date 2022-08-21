using System;
using UnityEngine;

public class RegistrySO : ScriptableObject
{
    public string casId;
    public ChemicalVariant chemicalVariant;
}

[Serializable]
public class Registy
{
    public string casId;
    public ChemicalVariant chemicalVariant;

    public Registy(MolecularViewSO so)
    {
        this.casId = so.formula.casId;
        this.chemicalVariant = ChemicalVariant.Copy(so.formula.chemicalVariant);
    }

    public Registy(string casId, ChemicalVariant chemicalVariant)
    {
        this.casId = casId;
        this.chemicalVariant = ChemicalVariant.Copy(chemicalVariant);
    }

    public static Registy Copy(Registy cas)
    {
        Registy cs = new Registy(cas.casId, cas.chemicalVariant);
        return cs;
    }
}