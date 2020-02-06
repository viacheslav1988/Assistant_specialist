using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assistant_specialist.Tools
{
    static class DecimalExtension
    {

        #region Стиль суммы прописью

        public enum Style
        {
            Empty,
            Minimum,
            Middle,
            Full
        }

        #endregion

        #region Сумма прописью

        public static string AmountInWords(this decimal number, Style style = Style.Full, bool register = true)
        {

            #region Массив worlds

            string[][] worlds =
            {
                new string[] {"", "сто ", "двести ", "триста ", "четыреста ", "пятьсот ", "шестьсот ", "семьсот ", "восемьсот ", "девятьсот "},
                new string[] {"", "", "двадцать ", "тридцать ", "сорок ", "пятьдесят ", "шестьдесят ", "семьдесят ", "восемьдесят ", "девяносто "},
                new string[] {"", "одна ", "две ", "три ", "четыре ", "пять ", "шесть ", "семь ", "восемь ", "девять ", "десять ", "одиннадцать ", "двенадцать ", "тринадцать ", "четырнадцать ", "пятнадцать ", "шестнадцать ", "семнадцать ", "восемнадцать ", "девятнадцать "},
                new string[] {"", "один ", "два ", "три ", "четыре ", "пять ", "шесть ", "семь ", "восемь ", "девять ", "десять ", "одиннадцать ", "двенадцать ", "тринадцать ", "четырнадцать ", "пятнадцать ", "шестнадцать ", "семнадцать ", "восемнадцать", "девятнадцать"},
                new string[] {"октиллион ", "октиллиона ", "октиллионов "},
                new string[] {"септиллион ", "септиллиона ", "септиллионов "},
                new string[] {"секстиллион ", "секстиллиона ", "секстиллионов "},
                new string[] {"квинтиллион ", "квинтиллиона ", "квинтиллионов "},
                new string[] {"квадриллион ", "квадриллиона ", "квадриллионов "},
                new string[] {"триллион ", "триллиона ", "триллионов "},
                new string[] {"миллиард ", "миллиарда ", "миллиардов "},
                new string[] {"миллион ", "миллиона ", "миллионов "},
                new string[] {"тысяча ", "тысячи ", "тысяч "},
                new string[] {" рубль ", " рубля ", " рублей "},
                new string[] {" копейка", " копейки", " копеек"},
            };

            #endregion

            //Вычисление целой части числа и последних два рубля
            decimal integer = Math.Truncate(number);
            int ruble = (int)(integer % 100m);

            //Вычисление дробной части числа и первые две копейки
            decimal calculate = number;
            decimal discharge = 1m;
            for (; calculate % 1 != 0; calculate *= 10m, discharge *= 10m) ;
            decimal fractional = Math.Truncate((number - integer) * discharge);
            int kopek = (int)Math.Truncate(fractional / discharge * 100m);

            //Вычисление классов числа
            int[] numberClasses = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int classesAmount = numberClasses.Length - 1;
            calculate = integer;
            int count = 1;
            while ((calculate /= 10) > 1) count++;
            calculate = integer;
            count = count % 3 > 0 ? count / 3 + 1 : count / 3;
            for (int i = 0; i < count; i++)
            {
                calculate /= 1000m;
                numberClasses[classesAmount--] = decimal.ToInt32((calculate - Math.Truncate(calculate)) * 1000m);
                calculate = Math.Truncate(calculate);
            }

            //Преобразование в сумму числа по классам
            StringBuilder result = new StringBuilder();
            if (numberClasses[0] > 0) result.Append(Convert(numberClasses[0], worlds[0], worlds[1], worlds[3], worlds[4]));
            if (numberClasses[1] > 0) result.Append(Convert(numberClasses[1], worlds[0], worlds[1], worlds[3], worlds[5]));
            if (numberClasses[2] > 0) result.Append(Convert(numberClasses[2], worlds[0], worlds[1], worlds[3], worlds[6]));
            if (numberClasses[3] > 0) result.Append(Convert(numberClasses[3], worlds[0], worlds[1], worlds[3], worlds[7]));
            if (numberClasses[4] > 0) result.Append(Convert(numberClasses[4], worlds[0], worlds[1], worlds[3], worlds[8]));
            if (numberClasses[5] > 0) result.Append(Convert(numberClasses[5], worlds[0], worlds[1], worlds[3], worlds[9]));
            if (numberClasses[6] > 0) result.Append(Convert(numberClasses[6], worlds[0], worlds[1], worlds[3], worlds[10]));
            if (numberClasses[7] > 0) result.Append(Convert(numberClasses[7], worlds[0], worlds[1], worlds[3], worlds[11]));
            if (numberClasses[8] > 0) result.Append(Convert(numberClasses[8], worlds[0], worlds[1], worlds[2], worlds[12]));
            if (numberClasses[9] > 0) result.Append(Convert(numberClasses[9], worlds[0], worlds[1], worlds[3], null));
            if (numberClasses[9] == 0 & numberClasses[8] == 0 & numberClasses[7] == 0 & numberClasses[6] == 0 &
                numberClasses[5] == 0 & numberClasses[4] == 0 & numberClasses[3] == 0 & numberClasses[2] == 0 &
                numberClasses[1] == 0 & numberClasses[0] == 0) result.Append("ноль ");

            //Первая буква преобразуется в заглавную в зависимости от register
            string raw = result.ToString().Trim();
            result.Clear();
            if (register) raw = raw.Substring(0, 1).ToUpper() + raw.Substring(1);

            //Преобразование в финальную строку в зависимости от style            
            switch (style)
            {
                case Style.Empty:
                    {
                        result.Append(raw);
                        break;
                    }
                case Style.Minimum:
                    {
                        result.Append(raw).Append(" руб. ").Append(fractional.ToString().Substring(0, 2)).Append(" коп.");
                        break;
                    }
                case Style.Middle:
                    {
                        result.Append(raw);
                        Add(ruble, result, worlds[13]);
                        if (kopek < 10) result.Append('0');
                        result.Append(kopek);
                        Add(kopek, result, worlds[14]);
                        break;
                    }
                case Style.Full:
                    {
                        result.Append(integer).Append(" (").Append(raw).Append(")");
                        Add(ruble, result, worlds[13]);
                        if (kopek < 10) result.Append('0');
                        result.Append(kopek);
                        Add(kopek, result, worlds[14]);
                        break;
                    }
            }
            return result.ToString();
        }

        #endregion

        #region Метод Convert

        private static StringBuilder Convert(int number, string[] hundreds, string[] tens, string[] units, string[] name)
        {
            StringBuilder result = new StringBuilder();
            int hundred = number / 100;
            int exception = number - hundred * 100;
            int ten = (number - hundred * 100) / 10;
            int unit = number - hundred * 100 - ten * 10;
            if (1 <= hundred & hundred <= 9) result.Append(hundreds[hundred]);
            if (exception > 19)
            {
                if (2 <= ten & ten <= 9) result.Append(tens[ten]);
                if (1 <= unit & unit <= 9) result.Append(units[unit]);
            }
            else if (1 <= exception & exception <= 19) result.Append(units[exception]);
            if (name != null) Add(exception, result, name);
            return result;
        }

        #endregion

        #region Метод Add

        private static void Add(int unit, StringBuilder result, string[] name)
        {
            if (11 <= unit & unit <= 19) result.Append(name[2]);
            else
            {
                unit = unit - (unit / 10) * 10;
                if (unit == 1) result.Append(name[0]);
                else if (2 <= unit & unit <= 4) result.Append(name[1]);
                else result.Append(name[2]);
            }
        }

        #endregion

    }
}
