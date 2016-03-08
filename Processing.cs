using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroMap
{
    class Processing
    {
        public Processing(Tuple<int, int, string>[] stationList, double[,] stationsRelations)
        {
            StationList = stationList;
            WeightMatrix = GetWeightMatrix(stationsRelations);
            HistoryMatrix = GetHistoryMatrix();
            ShortestPathMatrix = CalculateFloyd();
        }

        private readonly double Infinity = Double.PositiveInfinity;
        private readonly Tuple<int, int, string>[] StationList;

        public List<int> StationIds = new List<int>();

        public double[,] HistoryMatrix;
        public double[,] ShortestPathMatrix;
        public double[,] WeightMatrix;


        /// <summary>
        /// Получение первоначальной матрицы веса исходя из состояний
        /// </summary>
        /// <returns></returns>
        private double[,] GetWeightMatrix(double[,] stationsRelations)
        {
            var w = new double[28, 28];
            for (int i = 0; i < w.GetLength(0); i++)
            {
                for (int j = 0; j < w.GetLength(0); j++)
                {
                    for (int k = 0; k < stationsRelations.GetLength(0); k++)
                    {
                        if (stationsRelations[k, 0] == i + 1 &&
                            stationsRelations[k, 1] == j + 1)
                        {
                            w[j, i] = w[i, j] = stationsRelations[k, 2];
                        }
                        
                    }
                    if (w[i, j] == 0)
                        w[i, j] = Infinity;
                    if (w[j, i] == 0)
                        w[j, i] = Infinity;

                }
            }
            return w;
        }


        /// <summary>
        /// Подсчет суммарной стоимости
        /// </summary>
        /// <returns></returns>
        public int GetCost(List<int> stationIds)
        {
            int Cost = 0;
            var stationTypesIds = from t in StationList
                                  where stationIds.Contains(t.Item1)
                                  select t.Item2;

            if (stationTypesIds.Contains(1))
                Cost += 1;
            if (stationTypesIds.Contains(2))
                Cost += 3;
            if (stationTypesIds.Contains(3))
                Cost += 2;
            if (stationTypesIds.Contains(4))
                Cost += 4;

            return Cost;

        }



        /// <summary>
        /// инициализация таблицы истории
        /// </summary>
        /// <returns></returns>
        private double[,] GetHistoryMatrix()
        {
            var history = new double[28, 28];
            for (int i = 0; i < WeightMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < WeightMatrix.GetLength(1); j++)
                {
                    if (WeightMatrix[i, j] == Infinity)
                        history[i, j] = 0;
                    else
                        history[i, j] = i + 1;
                }
            }
            return history;
        }


        /// <summary>
        /// Получение идентификаторов проездных станций 
        /// </summary>
        public void GetStationIds(int startId, int endId)
        {
            var val = Convert.ToInt32(ShortestPathMatrix[endId - 1, startId - 1]);
            if (val == endId)
            {
                return;
            }
            StationIds.Add(val);
            GetStationIds(val, endId);
        }


        /// <summary>
        /// Алгоритм Флойда
        /// </summary>
        private double[,] CalculateFloyd()
        {
            var shortestPathMatrix = HistoryMatrix;
            for (int k = 0; k < WeightMatrix.GetLength(0); k++)
            {
                for (int i = 0; i < WeightMatrix.GetLength(0); i++)
                {
                    for (int j = 0; j < WeightMatrix.GetLength(0); j++)
                    {
                        if (WeightMatrix[i, k] != Infinity && WeightMatrix[k, j] != Infinity && (WeightMatrix[i, j] > WeightMatrix[k, j] + WeightMatrix[i, k]))
                        {
                            WeightMatrix[i, j] = WeightMatrix[i, k] + WeightMatrix[k, j];
                            shortestPathMatrix[i, j] = HistoryMatrix[k, j];
                        }
                    }
                }
            }
            return shortestPathMatrix;
        }

    }
}
