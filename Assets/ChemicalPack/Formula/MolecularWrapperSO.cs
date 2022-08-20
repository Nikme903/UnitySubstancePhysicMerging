using System;
using UnityEngine;

public enum PhaseState
{
    Unknokwn,
    Liquid,
    Solid,
    Gas,
    Artificial
}

public enum MolecularPhase
{
    None,
    Concentrate,
    Dissolved
}

public class MolecularWrapperSO : ScriptableObject //агрегатор по сути формула
{
    public CASFormulaSO formula;
    public float moleculeCount;
    public Color molecularColor;
    public MolecularPhase molecularPhase;
}

[Serializable]
public class MolecularWrapper
{
    public CASFormula formula;
    public float molecularCount;
    public Color molecularColor;
    public MolecularPhase molecularPhase;

    public MolecularWrapper(CASFormula formula)
    {
        this.formula = formula;
    }

    public MolecularWrapper(CASFormula formula, MolecularWrapper molecula)
    {
        this.formula = formula;
        this.molecularColor = molecula.molecularColor;
    }

    public MolecularWrapper(MolecularWrapperSO so)
    {
        formula = new CASFormula(so);
        this.molecularColor = so.molecularColor;
    }
}