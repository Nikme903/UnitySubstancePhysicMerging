using UnityEngine;

public class FillController : MonoBehaviour
{
    public ContainerSource sWater;
    public ContainerSource sAcid;
    public ContainerSource sCa;
    public ContainerSource sBa;
    public ContainerTransitive transit1;
    public ContainerTransitive transit2;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
    }

    public void RunSourceOut()
    {
        SplitMediator.SourceOutOperation(transit1, sWater, 25);
        SplitMediator.SourceOutOperation(transit1, sCa, 25);
        SplitMediator.FilledInfo(transit1);
        UpdateTransitRender(transit1);
    }

    public void RunTtoT()
    {
        SplitMediator.TransitContainerToContainer(transit2, transit1, 0.7f);
        SplitMediator.FilledInfo(transit2);
        SplitMediator.FilledInfo(transit1);
        UpdateTransitRender(transit1);
        UpdateTransitRender(transit2);
    }

    public void ClearT1()
    {
        SplitMediator.ClearAllEntries(transit1);
        UpdateTransitRender(transit1);
    }

    public void ClearT2()
    {
        SplitMediator.ClearAllEntries(transit2);
        UpdateTransitRender(transit2);
    }

    public void RunT1toT2()
    {
        SplitMediator.TransitContainerToContainer(transit2, transit1, 1.0f);
        SplitMediator.FilledInfo(transit2);
        SplitMediator.FilledInfo(transit1);
        UpdateTransitRender(transit1);
        UpdateTransitRender(transit2);
    }

    public void RunT2fromHalfT1()
    {
        SplitMediator.TransitContainerToContainer(transit2, transit1, 0.25f);
        SplitMediator.FilledInfo(transit2);
        SplitMediator.FilledInfo(transit1);
        UpdateTransitRender(transit1);
        UpdateTransitRender(transit2);
    }
    public void Add1()
    {
        SplitMediator.SourceOutOperation(transit1, sWater, 20);
        SplitMediator.SourceOutOperation(transit1, sAcid, 20);
        SplitMediator.FilledInfo(transit1);
        UpdateTransitRender(transit1);
    }

    public void Mix1()
    {
        ReactionMediator.FindReactionAndTryMix(transit1);
        SplitMediator.FilledInfo(transit1);
        UpdateTransitRender(transit1);
    }

    public void Add2()
    {
        SplitMediator.SourceOutOperation(transit1, sBa, 10);
        SplitMediator.FilledInfo(transit1);
        UpdateTransitRender(transit1);
    }

    public void Mix2()
    {
        ReactionMediator.FindReactionAndTryMix(transit1);
        SplitMediator.FilledInfo(transit1);
        UpdateTransitRender(transit1);
    }

    public void UpdateTransitRender(ContainerTransitive c)
    {
        TransitiveLayerLiquidRenderer render = c.GetComponent<TransitiveLayerLiquidRenderer>();
        render.UpdateLiquidLayers();
    }
}