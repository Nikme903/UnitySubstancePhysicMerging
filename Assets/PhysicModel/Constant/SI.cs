namespace   PhysicModel2.Math
{
    public enum PhaseState
    {
        Undefined,
        Liquid,
        Solid,
        Gas,
        Artificial,
    }

    public enum SIBasicUnits
    {
        kilogram
    }

    internal enum SIUnitsprefixes
    {
        //http://www.ebyte.it/library/educards/siunits/TablesOfSiUnitsAndPrefixes.html

        Yotta,  //	10^24
        Zetta,  //	10^21
        Exa,    //	10^18
        Peta,   //	10^15
        Tera,   //	10^12
        Giga,   //	10^9
        Mega,   //	10^6
        Kilo,   //  k
        //hecto,  //	10^0
        //deca,   // 	10
        //deci,   //	0.1
        //centi,  //	0.01
        milli,  //  10-3
        micro,  //  10-6
        nano,   //	10-9
        pico,   //	10-12
        femto,  //	10-15
        atto,   //	10-18
        zepto,  //	10-21
        yocto,  //	10-24
    }

    internal class SI
    {
    }
}