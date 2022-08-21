using PhysicModel2.Math;
using System;

[Serializable]
public class ChemicalVariant
{
    public string molecularFormula;
    public string chemicalEnName;
    public string chemicalRuName;

    public float density; //g/cm3
    public PhaseState phase;

    public ChemicalVariant(string molecularFormula, string chemicalEnName, string chemicalRuName)
    {
        this.molecularFormula = molecularFormula;
        this.chemicalEnName = chemicalEnName;
        this.chemicalRuName = chemicalRuName;
    }

    public ChemicalVariant(string molecularFormula, string chemicalEnName, string chemicalRuName, float density, PhaseState phase)
    {
        this.molecularFormula = molecularFormula;
        this.chemicalEnName = chemicalEnName;
        this.chemicalRuName = chemicalRuName;
        this.density = density;
        this.phase = phase;
    }

    public static ChemicalVariant Copy(ChemicalVariant cv)
    {
        ChemicalVariant ret = new ChemicalVariant(cv.molecularFormula, cv.chemicalEnName,cv.chemicalRuName,cv.density,cv.phase);
        return ret;
    }
}