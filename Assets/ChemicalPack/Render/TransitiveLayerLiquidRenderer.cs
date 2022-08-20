using System.Collections.Generic;
using UnityEngine;

public class TransitiveLayerLiquidRenderer : MonoBehaviour
{
    public LiquidVolumeFX.LiquidVolume liquidVolume;
    public ContainerTransitive transitContainer;
    public bool isWorking;
    public List<LayerData> datas;

    private void Start()
    {
        datas = new List<LayerData>();
        liquidVolume = GetComponent<LiquidVolumeFX.LiquidVolume>();
    }

    public void UpdateLiquidLayers()
    {
        datas.Clear();
        List<Entry> nextStateEntries = CombineEntries(transitContainer);
        AddLiquidLayer(nextStateEntries);
        liquidVolume.UpdateLayers();

        liquidVolume.alpha = 1;
    }

    private void AddLiquidLayer(List<Entry> nextEntries)
    {
        liquidVolume.liquidLayers = new LiquidVolumeFX.LiquidVolume.LiquidLayer[nextEntries.Count];
        for (int i = 0; i < nextEntries.Count; i++)
        {
            LayerData layerData = new LayerData(nextEntries[i], transitContainer.containerVolume);
            datas.Add(layerData);
            liquidVolume.liquidLayers[i] = layerData.layer;
            liquidVolume.liquidLayers[i].adjustmentSpeed = 3;
        }
    }

    private static List<Entry> CombineEntries(ContainerTransitive container)
    {
        List<Entry> temp = new List<Entry>();
        if (container.lastMixedEntries != null)
        {
            if (container.lastMixedEntries.Count > 0)
            {
                temp.AddRange(container.lastMixedEntries);
            }
        }
        temp.AddRange(container.entries);

        return temp;
    }
}