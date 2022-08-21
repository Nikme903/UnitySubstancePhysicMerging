using System.Collections.Generic;

public class ContainerSource : BaseContainer
{
    public MolecularWrapperSO sourceMolecular;
    public bool isInfinite = true;
    private SourceLayerLiquidRenderer renderer => GetComponent<SourceLayerLiquidRenderer>();

    private void Start()
    {
        entries = new List<Entry>();
        ContainerSourceInit(sourceMolecular);
        if (renderer != null) { renderer.Create(); }
    }

    public override void EntriesVolumeUpdate()
    {
    }

    private void ContainerSourceInit(MolecularWrapperSO so)
    {
        entries.Add(new Entry(so, filledVolume));
    }

    public void MinusVolumeOfTheEntry(Entry entry, float volumeCubicCm)
    {
        if (isInfinite)
        { }
        else
        {
            entry.entryVolume -= volumeCubicCm;
            UpdateFilledValue();
            if (renderer != null) { renderer.Create(); }
        }
    
    }
}