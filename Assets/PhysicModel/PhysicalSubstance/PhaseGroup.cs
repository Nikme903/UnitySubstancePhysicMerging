using PhysicModel2.Math;
using System.Collections.Generic;

namespace PhysicModel.PhysicalSubstance

{/*
  Вода
    жидкое
        Liquid
            minT = 0 maxT=100;
            плотность - сохраняется
            масса - сохраняется
            объем - сохраняется
         //при нагревании плотнось уменьшается, при охлаждении -возрастает
        //при нагревании объем увеличивается, при охлаждении -уменьшается

  */

    //точка границы фазы

    public class PhaseBorder
    {
        public class Border
        {
            /// <summary>
            /// Градусы  на границе данной фазы, С Цельсия
            /// </summary>
            public readonly float temperature;

            /// <summary>
            /// Плотность на границе данной фазы, кг/м3. Например: вода 997 kg/m³
            /// </summary>
            public readonly float density;

            public Border(float temperature, float density)
            {
                this.temperature = temperature;
                this.density = density;
            }
        }

        private Border m_left;
        private Border m_right;

        public Border left { get => m_left; set => m_left = value; }
        public Border right { get => m_right; set => m_right = value; }

        /// <summary>
        /// Граница фазы [(t1,d1) (t2,d2) ]
        /// </summary>
        /// <param name="t1">Температура на ЛЕВОЙ границе этой фазы, град Цельсия</param>
        /// <param name="d1">Плотность на ЛЕВОЙ границе этой фазы, кг/м3 </param>
        /// <param name="t2">Температура на ПРАВОЙ границе этой фазы, град Цельсия</param>
        /// <param name="d2">Плотность на ПРАВОЙ границе этой фазы, кг/м3 </param>
        public PhaseBorder(float t1, float t2, float d1, float d2)
        {
            this.left = new Border(t1, d1);
            this.right = new Border(t2, d2);
        }
    }

    //конкретная фаза с двумя границами
    public class Phase
    {
        public string phaseId;
        public string phaseName;
        public PhaseState phaseState;

        public PhaseBorder border;

        public Phase(string stateId, string phaseName, PhaseBorder border)
        {
            this.phaseName = phaseName;
            this.phaseId = stateId;
            this.border = border;
        }

        public Phase(PhaseState agregatePhase, string stateName, PhaseBorder border)
        {
            this.phaseName = stateName;
            this.phaseState = agregatePhase;
            this.border = border;
        }

        public Phase(PhaseState agregatePhase, PhaseBorder border)
        {
            this.phaseName = SelectDefaultName();
            this.phaseState = agregatePhase;
            this.border = border;
        }

        private string SelectDefaultName()
        {
            switch (phaseState)
            {
                case PhaseState.Undefined:
                    return "Не выбрано";

                case PhaseState.Liquid:
                    return "Жидкое";

                case PhaseState.Solid:
                    return "Твердое";

                case PhaseState.Gas:
                    return "Газ";
            }
            return "Error";
        }

        public float GetDensityLerp(float temperature)
        {
            float tempT = MathOperation.InvertedLerp(border.left.temperature, border.right.temperature, temperature);
            return MathOperation.Lerp(border.left.density, border.right.density, tempT);
        }
    }

    //состояние [воды, вещества] характеризуется некотороым набором конкретных фаз
    public class PhaseGroup          //состояние   Liquid,     Solid,    Gas
    {
        public List<Phase> phases;

        public PhaseGroup()
        {
        }

        public void AddPhase(List<Phase> phases)
        {
            this.phases = phases;
        }
    }
}