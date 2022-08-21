using PhysicModel.PhysicalSubstance;
using PhysicModel2.Math;
using System;
using System.Collections.Generic;

namespace PhysicModel2.PhysicalContainer
{
    public class ContainerModel //физическая система
    {
        public event Action OnTemperatureChanged = delegate { };

        public float filledVolume;
        private List<AbstractSubstance> substances;
        private ContainerAgregateParameters containerParameters;

        public ContainerModel()
        {
            substances = new List<AbstractSubstance>();
            containerParameters = new ContainerAgregateParameters();
        }

        public void ChangeTemperature(float temperature)
        {
            containerParameters.averageTemperature = temperature;
            ChangePhase();
            SetAverageTemperatureForSubstances();
            RecalculateContainerParameters();
            Console.WriteLine($"Изменение температуры на {temperature}");
        }

        /// <summary>
        ///  Добавить объем газового вещества, в м^3
        /// </summary>
        /// <param name="substance"></param>
        /// <param name="volume"></param>
        /// <param name="state">Состояние вещества на момент использования</param>
        public void AddSubstanceVolumeGas(AbstractSubstance substance, float volume, float substanceTemperature = 20)
        {
            substance.InitVolume(volume, PhaseState.Gas, substanceTemperature);
            substances.Add(substance);
            FindAverageContainerTemperature();
            SetAverageTemperatureForSubstances();
        }

        /// <summary>
        ///  Добавить объем жидкого вещества, в м^3
        /// </summary>
        /// <param name="substance"></param>
        /// <param name="volume"></param>
        /// <param name="state">Состояние вещества на момент использования</param>
        public void AddSubstanceVolumeLiquid(AbstractSubstance substance, float volume, float substanceTemperature = 20)
        {
            substance.InitVolume(volume, PhaseState.Liquid, substanceTemperature);
            substances.Add(substance);
            FindAverageContainerTemperature();
            SetAverageTemperatureForSubstances();
        }

        /// <summary>
        /// Добавить вещество, в кг
        /// </summary>
        /// <param name="substance"></param>
        /// <param name="mass">kg</param>
        public void AddSubstanceMass(AbstractSubstance substance, float mass, float substanceTemperature = 20)
        {
            substance.InitMass(mass, PhaseState.Solid, substanceTemperature);
            substances.Add(substance);
            FindAverageContainerTemperature();
            SetAverageTemperatureForSubstances();
        }

        private void ChangePhase()
        {
            for (int i = 0; i < substances.Count; i++)
            {
                substances[i].UpdatePhase(containerParameters.averageTemperature);
            }
        }

        private void SetAverageTemperatureForSubstances()
        {
            for (int i = 0; i < substances.Count; i++)
            {
                substances[i].HeatSubstance(containerParameters.averageTemperature);
            }
        }

        private void RecalculateContainerParameters()
        {
            float volume = 0;
            for (int i = 0; i < substances.Count; i++)
            {
                volume += substances[i].substanceParameters.volume;
            }
            containerParameters.filledVolume = volume;

            float mass = 0;
            for (int i = 0; i < substances.Count; i++)
            {
                mass += substances[i].substanceParameters.mass;
            }
            containerParameters.filledMass = mass;
        }

        private void FindAverageContainerTemperature()
        {
            float t = 0;
            for (int i = 0; i < substances.Count; i++)
            {
                t += substances[i].substanceParameters.temperature;
            }
            containerParameters.averageTemperature = t / substances.Count;
        }

        /*
         Вариации смешивания
            -все вещества полностью смешались (вода и спирт)
            -все вещества частично смешались  (вода и соль)
            -все вещества не смешались  (вода и магний/масло)

            - смешались несколько но часть осталась без смешивания
            -

         */

        public void ShowStatus()
        {
            for (int i = 0; i < substances.Count; i++)
            {
                Console.WriteLine("----Substance---");
                Console.WriteLine(Log($"вещество[{substances[i].substanceName}]"));
                Console.WriteLine(Log($"температура вещества[{i}]", substances[i].substanceParameters.temperature));
                Console.WriteLine(Log("фаза", substances[i].phase.phaseState));
                Console.WriteLine(Log("Volume, м^3", substances[i].substanceParameters.volume));
                Console.WriteLine(Log("Mass, кг", substances[i].substanceParameters.mass));
                Console.WriteLine(Log("density, кг/м^3", substances[i].substanceParameters.density));

                Console.WriteLine("----Container---");
                Console.WriteLine(Log("контейнер содержит", substances.Count));
                Console.WriteLine(Log("контейнер_filledvolume, м^3", containerParameters.filledVolume));
                Console.WriteLine(Log("контейнер_filledmass, г", containerParameters.filledMass * 1000));
                Console.WriteLine(Log("контейнер_averdensity кг/м^3", containerParameters.density));
            }
        }

        private string Log(string comment)
        {
            return $"{comment}";
        }

        private string Log(string comment, float value)
        {
            return $"{comment}\t\t\t{value}";
        }

        private string Log(string comment, PhaseState state)
        {
            return $"{comment}\t\t\t{state}";
        }
    }
}