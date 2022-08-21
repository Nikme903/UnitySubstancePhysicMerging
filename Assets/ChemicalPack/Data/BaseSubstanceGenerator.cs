using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class BaseSubstanceGenerator : MonoBehaviour
{
    private MolecularWrapperSO molecular;
    private CASFormulaSO element;

    [ContextMenu("run")]
    public void Run()
    {
        string path = "Assets/Resources/Elements/SimpleVariants/";
        CSVReader tr = new CSVReader();

        for (int i = 0; i < 118; i++)
        {

            element = ScriptableObject.CreateInstance<CASFormulaSO>();
            element.casId = tr.dataSimple[i].casId;
            //element.chemicalVariant = new ChemicalVariant(
            //    technicalReader.technicalData[i].clearFormula,
            //    technicalReader.technicalData[i].engName,
            //    technicalReader.technicalData[i].rusName
            //    );

            element.chemicalVariant = new ChemicalVariant(
              tr.dataSimple[i].clearFormula,
              tr.dataSimple[i].engName,
              tr.dataSimple[i].rusName,
              tr.dataSimple[i].density,
              ParsedPhase(tr.dataSimple[i]) //tr.dataSimple[i].phase
              );

            string mpath = "Assets/Resources/Elements/SimpleMoleculars/";
            string trimmedPhase = tr.dataSimple[i].phase.Trim();
            string mname = $"ml_{tr.dataSimple[i].rawFormula} {trimmedPhase}.asset";
            string mFullpath = mpath + mname;
            molecular = ScriptableObject.CreateInstance<MolecularWrapperSO>();
            molecular.formula = element;
            AssetDatabase.CreateAsset(molecular, mFullpath);
            EditorUtility.SetDirty(molecular);
            //AssetDatabase.SaveAssets();


            string name = $"{tr.dataSimple[i].rawFormula}_{tr.dataSimple[i].engName}.asset";
            string fullpath = path + name;
            AssetDatabase.CreateAsset(element, fullpath);
            EditorUtility.SetDirty(element);
            //AssetDatabase.SaveAssets();
            
            //Debug.Log($"added[{i}]");
        }
        
        //AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
    }

    private PhaseState ParsedPhase(DataContainer dataContainer)
    {
        string phase = dataContainer.phase.Trim();
        if (string.Equals(phase, "liq"))
        {
            return PhaseState.Liquid;
        }
        if (string.Equals(phase, "solid"))
        {
            return PhaseState.Solid;
        }
        if (string.Equals(phase, "gas"))
        {
            return PhaseState.Gas;
        }
        if (string.Equals(phase, "artificial"))
        {
            return PhaseState.Artificial;
        }
        return PhaseState.Unknokwn;
    }

    [ContextMenu("run2")]
    public void Run2()
    {
        CASFormulaSO element;
        string path = "Assets/Resources/Elements/FormulaVariants/";
        CSVReader tr = new CSVReader();

        for (int i = 0; i < tr.dataFormula.Count; i++)
        {
            element = ScriptableObject.CreateInstance<CASFormulaSO>();
            element.casId = tr.dataFormula[i].casId;
            element.chemicalVariant = new ChemicalVariant(
                tr.dataFormula[i].clearFormula,
                tr.dataFormula[i].engName,
                tr.dataFormula[i].rusName
                );

            string mpath = "Assets/Resources/Elements/FormulaMoleculars/";
            string mname = $"ml_{tr.dataFormula[i].rawFormula}.asset";
            string mFullpath = mpath + mname;
            molecular = ScriptableObject.CreateInstance<MolecularWrapperSO>();
            molecular.formula = element;
            AssetDatabase.CreateAsset(molecular, mFullpath);
            EditorUtility.SetDirty(molecular);
            //AssetDatabase.SaveAssets();

            string name = $"{tr.dataFormula[i].rawFormula}_{tr.dataFormula[i].engName}.asset";
            string fullpath = path + name;
            AssetDatabase.CreateAsset(element, fullpath);
            EditorUtility.SetDirty(element);
            //AssetDatabase.SaveAssets();
            //Debug.Log($"added[{i}]");
        }
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
    }

    /*  private TechnicalDataSimple CreateTechInstance(string name, TechnicalReader tr,int index)
      {
          string techpath = "Assets/Resources/Elements/SimpleTech/";
          string techname = $"stech_{Enum.GetName(typeof(ElementName), values.GetValue(index)) }.asset";
          TechnicalDataSimple techSimple = ScriptableObject.CreateInstance<TechnicalDataSimple>();
          string fullpath = techpath + techname;

          techSimple.phase = tr.technicalData[index].phase;
          techSimple.atomicNumber = tr.technicalData[index].atomicNumber;
          techSimple.density = tr.technicalData[index].density;

          AssetDatabase.CreateAsset(techSimple, fullpath);
          AssetDatabase.SaveAssets();

          return techSimple;
      }*/
}