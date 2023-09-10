using System;

namespace VirtueSky.DataType
{
    public partial struct ShortDouble
    {
        static Unit FindUnit(double value)
        {
            return _unitFinder.Invoke(value);
        }

        public static class Unit0
        {
            static readonly Unit[] Units;
            static readonly Unit Infinity;
            static readonly Unit Zero;

            static Unit0()
            {
                Infinity.exponent = 0;
                Infinity.name = "(VeryBIG)";
                Zero.exponent = 0;
                Zero.name = "";

                Units = new Unit[120];
                var i = 0;

                Units[i++].name = "";
                Units[i - 1].exponent = (i - 1) * 3;

                Units[i++].name = "k";
                Units[i - 1].exponent = (i - 1) * 3;

                Units[i++].name = "m";
                Units[i - 1].exponent = (i - 1) * 3;

                Units[i++].name = "b";
                Units[i - 1].exponent = (i - 1) * 3;

                Units[i++].name = "t";
                Units[i - 1].exponent = (i - 1) * 3;


                for (var c0 = 'a'; c0 <= 'z'; c0++)
                {
                    for (var c1 = c0; c1 <= 'z'; c1++)
                    {
                        if (i >= Units.Length)
                        {
                            break;
                        }

                        Units[i++].name = c0.ToString() + c1.ToString();
                        Units[i - 1].exponent = (i - 1) * 3;
                    }
                }
            }

            public static Unit Find(double value)
            {
                //extract

                var e = Math.Log10(Math.Abs(value));
                var fe = Math.Floor(e);

                var exponent = Math.DivRem((long)fe, 3, out _) * 3;

                //find
                if (exponent < 0)
                    return Zero;
                return exponent / 3 < Units.Length ? Units[exponent / 3] : Infinity;
            }
        }

        public static class Unit1
        {
            static readonly Unit[] Units;
            static readonly Unit Infinity;
            static readonly Unit Zero;

            static Unit1()
            {
                Infinity.exponent = 0;
                Infinity.name = "(VeryBIG)";
                Zero.exponent = 0;
                Zero.name = "";

                Units = new Unit[304];
                var i = 0;

                Units[i++].name = "";
                Units[i - 1].exponent = (i - 1) * 3;

                Units[i++].name = "K";
                Units[i - 1].exponent = (i - 1) * 3;

                Units[i++].name = "M";
                Units[i - 1].exponent = (i - 1) * 3;

                Units[i++].name = "B";
                Units[i - 1].exponent = (i - 1) * 3;

                Units[i++].name = "T";
                Units[i - 1].exponent = (i - 1) * 3;

                var exp = 14;
                for (var j = i; j < Units.Length; j++)
                {
                    Units[j].name = "e" + (++exp);
                    Units[j].exponent = exp;
                }
            }

            public static Unit Find(double value)
            {
                //extract
                long exponent;

                var e = Math.Log10(Math.Abs(value));
                var fe = Math.Floor(e);

                if (fe < 15)
                {
                    exponent = Math.DivRem((long)fe, 3, out _) * 3;
                }
                else
                    exponent = (long)fe;

                //find
                if (exponent < 0)
                    return Zero;
                if (exponent < 15)
                    return Units[exponent / 3];
                return exponent < Units.Length + 5 ? Units[15 / 3 + exponent - 15] : Infinity;
            }
        }

        public static class Unit2
        {
            private static readonly Unit[] Units;
            private static readonly Unit Infinity;
            private static readonly Unit Zero;

            private static readonly string[] Signs =
            {
                "", "k", "m", "b", "t", "q", "Q", "s", "S", "o", "n", "d", "u",
                "Du", "Tr", "Qu", "Qi", "Sx", "Sp", "Oc", "No", "Vi", "Ce"
            };

            static Unit2()
            {
                Infinity.exponent = 0;
                Infinity.name = "(VeryBIG)";
                Zero.exponent = 0;
                Zero.name = "";

                Units = new Unit[Signs.Length];
                for (var i = 0; i < Signs.Length; i++)
                {
                    Units[i].name = Signs[i];
                    Units[i].exponent = i * 3;
                }
            }

            public static Unit Find(double value)
            {
                var e = Math.Log10(Math.Abs(value));
                var fe = Math.Floor(e);

                var exponent = Math.DivRem((long)fe, 3, out long remainder) * 3;

                if (exponent < 0)
                    return Zero;
                return exponent / 3 < Units.Length ? Units[exponent / 3] : Infinity;
            }
        }

        public struct Unit
        {
            public int exponent;
            public string name;
        }
    }
}