using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroMap
{
    class Program
    {
        /// <summary>
        /// Входные параметры для хранения в БД(номер, тип, название)
        /// </summary>
        private static readonly Tuple<int, int, string>[] StationNames = {
            Tuple.Create(1, 1, "Очаковское"),
            Tuple.Create(2, 1, "Балтика"),
            Tuple.Create(3, 1, "Охота"),
            Tuple.Create(4, 1, "Жигули"),
            Tuple.Create(5, 1, "Бочка"),
            Tuple.Create(6, 1, "Хамовники"),
            Tuple.Create(7, 1, "Афанасий"),
            Tuple.Create(8, 2, "Ловенбрау"),
            Tuple.Create(9, 2, "Варштайнер"),
            Tuple.Create(10, 2, "Гессер"),
            Tuple.Create(11, 2, "Бакс"),
            Tuple.Create(12, 2, "Паулайнер"),
            Tuple.Create(13, 2, "Кромбахен"),
            Tuple.Create(14, 2, "Амстел"),
            Tuple.Create(15, 3, "Старопрамен"),
            Tuple.Create(16, 3, "Крушовице"),
            Tuple.Create(17, 3, "Козел"),
            Tuple.Create(18, 3, "Вельвет"),
            Tuple.Create(19, 3, "Будвайзер"),
            Tuple.Create(20, 3, "Урквелл"),
            Tuple.Create(21, 3, "Гамбринус"),
            Tuple.Create(22, 4, "Хугарден"),
            Tuple.Create(23, 4, "Бавария"),
            Tuple.Create(24, 4, "Карлсберг"),
            Tuple.Create(25, 4, "Леффе"),
            Tuple.Create(26, 4, "Стелла"),
            Tuple.Create(27, 4, "Хайникен"),
            Tuple.Create(28, 4, "Туборг")
        };


        /// <summary>
        /// Входные параметры для хранения в БД (номер станции1, номер станции2, расстояние между ними)
        /// </summary>
        private static readonly double[,] StationsRelations = new double[30, 3]
        {
        {1, 2, 7},
        {2, 3, 8},
        {3, 4, 2},
        {3, 10, 4},
        {4, 5, 1},
        {4, 25, 3},
        {5, 6, 3},
        {5, 18, 1},
        {6, 7, 4},
        {8, 9, 5},
        {9, 10, 7},
        {10, 11, 2},
        {11, 12, 4},
        {11, 26, 5},
        {12, 13, 3},
        {12, 19, 3},
        {13, 14, 9},
        {15, 16, 7},
        {16, 17, 3},
        {17, 18, 4},
        {17, 24, 4},
        {18, 19, 3},
        {19, 20, 3},
        {20, 21, 8},
        {22, 23, 5},
        {23, 24, 4},
        {24, 25, 5},
        {25, 26, 3},
        {26, 27, 5},
        {27, 28, 8}
        };


        static void Main(string[] args)
        {
            int intval;
            Processing proc = new Processing(StationNames, StationsRelations);

            Console.WriteLine("Список всех станций:");
            foreach (Tuple<int, int, string> name in StationNames.OrderBy((t)=>t.Item3))
            {
                Console.WriteLine(name.Item3 + "-" + name.Item1);
            }

            Console.WriteLine("Введите номер начальной станции(1-28):");
            var startStationId = Console.ReadLine();
            Console.WriteLine("Введите номер конечной станции(1-28):");
            var endStationId = Console.ReadLine();
            //валидация
            while (!Int32.TryParse(startStationId, out intval) || !Int32.TryParse(endStationId, out intval)
                || Convert.ToInt32(startStationId) < 1 || Convert.ToInt32(startStationId) > 28
                || Convert.ToInt32(endStationId) < 1 || Convert.ToInt32(endStationId) > 28
                || startStationId == endStationId)
            {
                Console.WriteLine("Номер некорректен, попробуйте еще раз.");
                Console.WriteLine("Введите номер начальной станции(1-28):");
                startStationId = Console.ReadLine();
                Console.WriteLine("Введите номер конечной станции(1-28):");
                endStationId = Console.ReadLine();
            }
            

            Console.WriteLine("Кратчайшее время будет(мин): ");
            Console.WriteLine(proc.WeightMatrix[Convert.ToInt32(startStationId) - 1, Convert.ToInt32(endStationId) - 1].ToString());
            Console.WriteLine("Кратчайший маршрут будет: ");
            proc.StationIds.Add(Convert.ToInt32(startStationId));
            proc.GetStationIds(Convert.ToInt32(startStationId), Convert.ToInt32(endStationId));
            proc.StationIds.Add(Convert.ToInt32(endStationId));
            foreach (int stationId in proc.StationIds)
            {
                var station = StationNames.FirstOrDefault(t => t.Item1 == stationId);
                if (station != null)
                    Console.WriteLine(station.Item3);
            }
            Console.WriteLine("Стоимость билета будет составлять(руб): " + proc.GetCost(proc.StationIds));

            Console.ReadLine();
        }


    }
}
