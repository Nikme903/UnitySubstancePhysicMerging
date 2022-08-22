using PhysicModel2.Math;
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

    public float temperature = 20;

    [Tooltip("g/cm³==g/ml плотность любой View может отличаться от оригинального чистого образца")]
    public float density = 0.997f;

    [Tooltip("цвет данного образца")]
    public Color color = Color.white;

    public PhaseState phase = PhaseState.Liquid;
    //public float moleculeCount;

    //[Header("Массовая концетрация %масс")]
    //public bool useMassFraction;
    //public float massFraction;      //отношение массы данного компонента к сумме масс всех компонентов

    //[Header("Объемная концетрация %об ")]
    //public bool useVolumeFraction;
    //public float volumeFraction;    //отношение объёма компонента к сумме объёмов компонентов до смешивания
}

[Serializable]
public class MolecularView
{
    public Registy formula;
    public float temperature = 20;

    [Tooltip("g/cm³==g/ml плотность любой View может отличаться от оригинального чистого образца")]
    public float density;

    [Tooltip("цвет данного образца")]
    public Color color;

    public PhaseState phase;

    public MolecularView(Registy formula, MolecularView molecularView)
    {
        this.formula = formula;
        this.color = molecularView.color;
        this.density = molecularView.density;
        this.phase = molecularView.phase;
        this.temperature = molecularView.temperature;
    }

    public MolecularView(MolecularViewSO so)
    {
        formula = new Registy(so);
        this.color = so.color;
        this.density = so.density;
        this.phase = so.phase;
        this.temperature = so.temperature;
    }
}