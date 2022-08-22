using PhysicModel2.Math;
using System;

namespace PhysicModel.PhysicalSubstance
{
 
    public class SubstanceAgregateParameters
    {
        public float temperature = 20;
        public float volume;
        public float mass;
        public float density;

        public SubstanceAgregateParameters()
        {
        }

        public void InitMass(float mass)
        {
            this.mass = mass;
        }

        public void InitVolume(float volume)
        {
            this.volume = volume;
        }

        public void SetDensity(float density)
        {
            this.density = density;
        }

        public void BalanceZeroValues()
        {
            if ((volume != 0) && (mass == 0))
            {
                mass = MathOperation.Mass(density, volume);
            }
            else if ((volume == 0) && (mass != 0))
            {
                volume = MathOperation.Volume(density, mass);
            }
            else if ((volume != 0) && (mass != 0))
            {
                //масса не меняется при изменении плотности
                volume = MathOperation.Volume(density, mass);
            }
        }
    }
}