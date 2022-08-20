using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

public class CSVReader
{
    public List<DataContainer> dataSimple;
    public List<DataContainer> dataFormula;

    public CSVReader()
    {
        DataSimple();
        DataFormula();
    }

    private void DataSimple()
    {
        dataSimple = new List<DataContainer>();

        string[] values = File.ReadAllText("id_simple.csv").Split('\n');
 
        for (int i = 0; i < values.Length; i++)
        {
            string[] parsedLine = values[i].Split(';');

            //if (parsedLine.Length > 1)
            {
                DataContainer data =
                 new DataContainer()
                 {
                     casId = parsedLine[0],
                     rawFormula = parsedLine[1],
                     clearFormula = parsedLine[2],
                     engName = parsedLine[3],
                     rusName = parsedLine[4],
                     density = float.Parse(parsedLine[5], NumberStyles.Any, CultureInfo.InvariantCulture),
                     phase = parsedLine[6],
                 };
                dataSimple.Add(data);
            }
        }
    }

    private void DataFormula()
    {
        dataFormula = new List<DataContainer>();

        string[] values = File.ReadAllText("id_complex.csv").Split('\n');

        for (int i = 0; i < values.Length; i++)
        {
            string[] parsedLine = values[i].Split(';');

            //if (parsedLine.Length > 1)
            {
                dataFormula.Add(new DataContainer()
                {
                    casId = parsedLine[0],
                    rawFormula = parsedLine[1],
                    clearFormula = parsedLine[2],
                    engName = parsedLine[3],
                    rusName = parsedLine[4]
                });
            }
        }
    }
}