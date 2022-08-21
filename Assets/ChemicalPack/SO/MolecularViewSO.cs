using System;
using UnityEngine;

//public enum MolecularPhase
//{
//    None,
//    Concentrate,
//    Dissolved
//}

public class MolecularViewSO : ScriptableObject //агрегатор по сути формула
{
    public RegistrySO formula;
    //public float moleculeCount;

    //[Header("Массовая концетрация %масс")]
    //public bool useMassFraction;
    //public float massFraction;      //отношение массы данного компонента к сумме масс всех компонентов

    //[Header("Объемная концетрация %об ")]
    //public bool useVolumeFraction;
    //public float volumeFraction;    //отношение объёма компонента к сумме объёмов компонентов до смешивания

    [Header("Цвет данного образца")]
    public Color color;
}

[Serializable]
public class MolecularView
{
    public Registy formula;

    //public float molecularCount;
    public Color molecularColor;

    //public float molecularPhase;

    public MolecularView(Registy formula)
    {
        this.formula = formula;
    }

    public MolecularView(Registy formula, MolecularView molecula)
    {
        this.formula = formula;
        this.molecularColor = molecula.molecularColor;
    }

    public MolecularView(MolecularViewSO so)
    {
        formula = new Registy(so);
        this.molecularColor = so.color;
    }
}