using System;
using UnityEngine;

//public enum MolecularPhase
//{
//    None,
//    Concentrate,
//    Dissolved
//}

public class MolecularWrapperSO : ScriptableObject //агрегатор по сути формула
{
    public CASFormulaSO formula;
    //public float moleculeCount;

    [Header("Массовая концетрация %масс")]
    public bool useMassFraction;
    public float massFraction;      //отношение массы данного компонента к сумме масс всех компонентов

    [Header("Объемная концетрация %об ")]
    public bool useVolumeFraction;
    public float volumeFraction;    //отношение объёма компонента к сумме объёмов компонентов до смешивания

    [Header("Цвет данного образца")]
    public Color molecularColor;
}

[Serializable]
public class MolecularWrapper
{
    public CASFormula formula;

    //public float molecularCount;
    public Color molecularColor;

    public float molecularPhase;

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