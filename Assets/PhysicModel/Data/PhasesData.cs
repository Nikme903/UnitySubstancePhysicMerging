using PhysicModel.PhysicalSubstance;
using PhysicModel2.Math;
using System.Collections.Generic;

namespace PhysicModel.Data
{
    public class PhasesData
    {
        private Dictionary<string, AbstractSubstance> group;

        public PhasesData()
        {
            group = new Dictionary<string, AbstractSubstance>();
            Initializer();
        }

        public AbstractSubstance Get(string v)
        {
            return group[v];
        }

        private void Initializer()
        {
            PhaseGroup pcol = new PhaseGroup();
            PhaseGroup water2Phase = new PhaseGroup();
            PhaseGroup ferrumPhase = new PhaseGroup();

            pcol.AddPhase(new List<Phase>()
            {
                new Phase(PhaseState.Solid, new PhaseBorder(PhysConstant.ABSOLUTE_ZERO, 0, 1001f, 1000)), //увеличение  плотности - свидельство застывания вещества, частицы уплотняются
                new Phase(PhaseState.Liquid, new PhaseBorder(0, 100, 1000, 950f)),
                new Phase(PhaseState.Gas, new PhaseBorder(100, PhysConstant.POSITIVE_INFINITY, 950f, 400))//уменьшение  плотности - свидельство газового состояния вещества, частицы разлетаются
            });

            water2Phase.AddPhase(new List<Phase>()
            {
                new Phase(PhaseState.Solid,new PhaseBorder(PhysConstant.ABSOLUTE_ZERO, -6, 1010, 1000)),
                new Phase(PhaseState.Liquid,new PhaseBorder(0, 100, 1000, 950)),
                new Phase(PhaseState.Gas,new PhaseBorder(100, PhysConstant.POSITIVE_INFINITY, 950, 400))
            });

            ferrumPhase.AddPhase(new List<Phase>()
            {
                new Phase(PhaseState.Solid, new PhaseBorder(PhysConstant.ABSOLUTE_ZERO, 1535, 7874, 7874)),
                new Phase(PhaseState.Liquid, new PhaseBorder( 1535, 3000, 7874, 6900))
            });

            group.Add("001", new AbstractSubstance("Вода", pcol));

            group.Add("002", new AbstractSubstance("Спирт", water2Phase));

            group.Add("003", new AbstractSubstance("Сталь", ferrumPhase));
        }
    }
}