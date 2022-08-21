using System;
using UnityEngine;

public class CASFormulaSO : ScriptableObject
{
    public string casId;
    public ChemicalVariant chemicalVariant;
}

[Serializable]
public class CASFormula
{
    public string casId;
    public ChemicalVariant chemicalVariant;

    public CASFormula(MolecularWrapperSO so)
    {
        this.casId = so.formula.casId;
        this.chemicalVariant = ChemicalVariant.Copy(so.formula.chemicalVariant);
    }

    public CASFormula(string casId, ChemicalVariant chemicalVariant)
    {
        this.casId = casId;
        this.chemicalVariant = ChemicalVariant.Copy(chemicalVariant);
    }

    public static CASFormula Copy(CASFormula cas)
    {
        CASFormula cs = new CASFormula(cas.casId, cas.chemicalVariant);
        return cs;
    }
}