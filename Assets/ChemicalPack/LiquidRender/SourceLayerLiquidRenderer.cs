using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LayerData
{
    public Entry entry;
    public float density;
    public Color color;
    public LiquidVolumeFX.LiquidVolume.LiquidLayer layer;
    public string casID;
    public float volume;

    public LayerData(Entry entry, float containerVolume)
    {
        this.entry = entry;
        casID = entry.molecularView.formula.casId;
        //chemicalName = entry.molecule.formula.chemicalVariant.chemicalEnName;
        volume = entry.entryVolume;
        density = entry.molecularView.density;
        color = entry.molecularView.color;
        this.layer = new LiquidVolumeFX.LiquidVolume.LiquidLayer();
        this.layer.color = entry.molecularView.color;

        this.layer.density = entry.molecularView.density;
        float mas = entry.molecularView.density * (entry.entryVolume / containerVolume);
        this.layer.amount = mas;
    }
}

public class SourceLayerLiquidRenderer : MonoBehaviour
{
    public LiquidVolumeFX.LiquidVolume liquidVolume;
    public ContainerSource sourceContainer;
    public Entry currentLayerState;

    private void Start()
    {
        liquidVolume = GetComponent<LiquidVolumeFX.LiquidVolume>();
    }

    public void Create()
    {
        List<Entry> entries = sourceContainer.entries;
        liquidVolume.liquidLayers = new LiquidVolumeFX.LiquidVolume.LiquidLayer[entries.Count];
        for (int i = 0; i < entries.Count; i++)
        {
            liquidVolume.liquidLayers[i].color = entries[i].molecularView.color;
            liquidVolume.liquidLayers[i].density = entries[i].molecularView.density;
            float mas = entries[i].molecularView.density * (entries[i].entryVolume / sourceContainer.containerVolume);
            liquidVolume.liquidLayers[i].amount = mas;
        }
        liquidVolume.alpha = 1;
        liquidVolume.UpdateLayers();
    }
}