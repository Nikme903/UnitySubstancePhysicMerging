using PhysicModel2.Math;
using System.Collections.Generic;

namespace PhysicModel.PhysicalSubstance
{
    public class AbstractSubstance  //абстрактное вещество
    {
        public string substanceName { get; set; }
        public CASFormula formula;
        public PhaseGroup phaseCollector;
        public SubstanceAgregateParameters substanceParameters;
        public Phase phase;

        public AbstractSubstance(string name, PhaseGroup phaseCollector)
        {
            this.substanceName = name;
            this.phaseCollector = phaseCollector;
            substanceParameters = new SubstanceAgregateParameters();
        }

        public void InitVolume(float volume, PhaseState state, float substanceTemperature)
        {
            substanceParameters.InitVolume(volume);
            AddVariable(state, substanceTemperature);
        }

        public void InitMass(float mass, PhaseState state, float substanceTemperature)
        {
            substanceParameters.InitMass(mass);
            AddVariable(state, substanceTemperature);
        }

        public void UpdatePhase(float temperature)
        {
            SelectPhaseByTemp(temperature);
            UpdateSubstanceeDensity(temperature);
        }

        private void AddVariable(PhaseState state, float substanceTemperature)
        {
            substanceParameters.temperature = substanceTemperature;
            phase = FindPhase(state);
            UpdateSubstanceeDensity(substanceTemperature);
            substanceParameters.BalanceZeroValues();
        }

        private Phase FindPhase(PhaseState state)
        {
            return phaseCollector.phases.Find((x) => { return x.phaseState == state; });
        }

        private void SelectPhaseByTemp(float temp)
        {
            List<Phase> phases = phaseCollector.phases;
            for (int j = 0; j < phases.Count; j++)
            {
                if (((temp >= phases[j].border.left.temperature) && (temp <= phases[j].border.right.temperature)))
                {
                    phase = phases[j];
                }
            }
        }

        private void UpdateSubstanceeDensity(float temperature)
        {
            if (phase != null)
            {
                float d = phase.GetDensityLerp(temperature);
                substanceParameters.SetDensity(d);
                substanceParameters.BalanceZeroValues();
            }
            else
            {            /* Debug.Log($" Состояние не описано");      */
            }
        }

        public void HeatSubstance(float t)
        {
            substanceParameters.temperature = t;
        }
    }
}