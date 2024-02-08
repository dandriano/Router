using QuikGraph;
using QuikGraph.Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Router.Model
{
    /// <summary>
    /// Network as bidirectional graph
    /// </summary>
    public class Network : BidirectionalGraph<Node, Link>
    {
        #region [Routing]
        public IEnumerable<IEnumerable<Link>> CalculateByYen(Node startingRoot, Node target, int maxRouteCount = 10)
        {
            var edgeRoutes = new List<List<Link>>();

            // вычисляем кратчайший маршрут в графе по дейкстре 
            var firstPath = CalculateByDijkstra(startingRoot, target)
                            .ToList();

            // выходим если маршрута не существует
            if (firstPath == default || double.IsPositiveInfinity(firstPath.Sum(e => e.Weight)))
            {
                return null;
            }

            // добавляем кратчайший маршрут в лист 
            edgeRoutes.Add(firstPath);

            // создаем массив для k-тых маршрутов 
            var spurRoutes = new List<List<Link>>();

            // определяем число требуемых маршрутов (минимум из количества ребер в кратчайшем маршруте и параметром) 
            int routeCount = Math.Min(firstPath.Count, maxRouteCount);

            // находим k-тые маршруты
            for (int k = 1; k <= routeCount; k++)
            {
                for (int i = 0; i <= edgeRoutes[0].Count - 1; i++)
                {
                    // создаем массив для "обесконеченных" ребер
                    var infiniteEdges = new List<Link>();

                    // получаем spur ноду из кратчайшего маршрута
                    var spurNode = Vertices.FirstOrDefault(vertex => vertex.ID == edgeRoutes[0].ElementAt(i).Source.ID);

                    // последовательность ребер от стартового узла до spur
                    var rootPath = i == 0 ? default : edgeRoutes[0].Take(i).ToList();

                    // Удаляем ребра, которые являются частью предыдущих маршрутов
                    foreach (var path in edgeRoutes)
                    {
                        if (i == 0 || rootPath.Select(edge => edge.ID)
                            .SequenceEqual(path.Take(i).Select(edge => edge.ID)))
                        {
                            infiniteEdges.AddRange(Edges.Where(edge => edge.ID == path[i].ID));
                        }
                    }

                    // создаем массив для пройденных узлов
                    var reachedVertices = new List<Node>();

                    if (rootPath != default)
                    {
                        foreach (var edge in rootPath.SkipLast(1))
                        {
                            if (!reachedVertices.Contains(edge.Source))
                            {
                                reachedVertices.Add(edge.Source);
                            }

                            if (!reachedVertices.Contains(edge.Target))
                            {
                                reachedVertices.Add(edge.Target);
                            }
                        }

                        // явно обесконечиваем начальную вершину (она не попадает в reachedVertices ранее для случая маршрута из одного ребра)
                        reachedVertices.Add(startingRoot);

                        foreach (var edge in Edges)
                        {
                            if (!infiniteEdges.Contains(edge) && (reachedVertices.Contains(edge.Source) || reachedVertices.Contains(edge.Target)))
                            {
                                infiniteEdges.Add(edge);
                            }
                        }
                    }

                    // Вычислям маршруты от spur узла до финального узла по дейкстре
                    var spurPath = CalculateByDijkstra(spurNode, target, infiniteEdges, reachedVertices);

                    if (spurPath != default && !double.IsPositiveInfinity(spurPath.Sum(e => e.Weight)))
                    {
                        var totalPath = rootPath == default ? spurPath.ToList() : rootPath.Concat(spurPath).ToList();

                        if (spurRoutes != default && !spurRoutes.Contains(totalPath))
                        {
                            spurRoutes.Add(totalPath);
                        }
                    }
                }

                // Условие для случая, когда альтернативные spur маршруты не найдены или закончились варианты для альтернативы
                if (spurRoutes == default || spurRoutes.Count == 0)
                {
                    break;
                }

                // находим кратчайшего кандидата среди spur маршрутов
                var spurDistances = spurRoutes
                    .Select(path => path.Sum(e => e.Weight))
                    .ToList();
                int minIndex = spurDistances.IndexOf(spurDistances.Min());

                // добавляем кратчайший spur маршрут к окончательным вариантам
                edgeRoutes.Add(spurRoutes[minIndex]);
                spurRoutes.RemoveAt(minIndex);
            }

            return edgeRoutes;
        }

        public IEnumerable<Link> CalculateByDijkstra(Node startingRoot, Node target, IEnumerable<Link> startingInfiniteEdges = default, IEnumerable<Node> reachedVertices = default)
        {
            if (startingRoot.IsOutRoute || target.IsOutRoute)
            {
                return default;
            }

            var toReachList = Vertices.Where(v =>
                                                v.IsInRoute &&
                                                v.ID != startingRoot.ID &&
                                                v.ID != target.ID)
                                                    .Where(vertex => reachedVertices == default ||
                                                            !reachedVertices.Contains(vertex)).ToList();

            //случай отсутствия обязательных к посещению узлов (кроме начального и конечного) обрабатываем отдельно
            if (toReachList == default || toReachList.Count == 0)
            {
                // массив обесконеченных ребер(в начале туда попадают ребра непосещаемых узлов)
                var infiniteEdges = Edges.Where(e =>
                                                                startingInfiniteEdges != default &&
                                                                startingInfiniteEdges.Contains(e) ||
                                                                e.Source.IsOutRoute ||
                                                                e.Target.IsOutRoute)
                                                                .ToList();

                // находим маршрут просто по Дейкстре
                TryFunc<Node, IEnumerable<Link>> getDijkstraPath = this
                    .ShortestPathsDijkstra(e =>
                                               infiniteEdges.Contains(e) ? double.PositiveInfinity : e.Weight, startingRoot);

                return getDijkstraPath(target, out IEnumerable<Link> path) ? path : default;
            }

            var vertexPermutations = GetPermutations(toReachList, toReachList.Count);
            var routes = new List<List<Link>>();
            var distances = new List<double>();
            foreach (IEnumerable<Node> permutation in vertexPermutations)
            {

                var toReach = new List<Node>(permutation);

                //массив обесконеченных ребер (в начале туда попадают ребра непосещаемых узлов и ребра target)
                var infiniteEdges = Edges.Where(e =>
                                                    startingInfiniteEdges != default &&
                                                    startingInfiniteEdges.Contains(e) ||
                                                    e.Source.IsOutRoute ||
                                                    e.Target.IsOutRoute ||
                                                    e.Source.ID == target.ID ||
                                                    e.Target.ID == target.ID)
                                                    .ToList();


                var route = new List<Link>();
                Node root = startingRoot;
                bool pathExists = true;

                // поочередно посещаем все обязательные вершины
                while (toReach.Count > 0 && pathExists)
                {
                    var nearReachRoute = GetNextReachPointRoute(toReach, ref root, infiniteEdges);

                    if (nearReachRoute == default)
                    {
                        route = default;
                        pathExists = false;
                    }

                    else
                    {
                        route.AddRange(nearReachRoute);
                    }
                }

                if (pathExists)
                {
                    // возвращаем конечный вес ребрам вершины target
                    infiniteEdges = infiniteEdges.Where(e =>
                                                            e.Source.ID != target.ID &&
                                                            e.Target.ID != target.ID)
                                                                .ToList();

                    // находим финальный маршрут до target
                    TryFunc<Node, IEnumerable<Link>> getDijkstraPath = this
                        .ShortestPathsDijkstra(e =>
                                                   infiniteEdges.Contains(e) ? double.PositiveInfinity : e.Weight, root);

                    if (getDijkstraPath(target, out IEnumerable<Link> path))
                    {
                        route.AddRange(path);
                        routes.Add(route);
                        distances.Add(route.Sum(e => e.Weight));
                    }
                    else
                    {
                        route = default;
                    }
                }
            }

            if (routes == default || routes.Count == 0)
            {
                return default;
            }

            //возвращаем маршрут с минимальной дистанцией
            int minIndex = distances.IndexOf(distances.Min());

            return double.IsPositiveInfinity(distances[minIndex]) ? default : routes[minIndex];
        }

        /// <summary>
        /// Функция нахождения маршрута до ближайшего узла, обязательного к посещению
        /// </summary>
        /// <param name="toReach"></param>
        /// <param name="root"></param>
        /// <param name="infiniteEdges"></param>
        /// <returns></returns>
        private List<Link> GetNextReachPointRoute(List<Node> toReach, ref Node root, List<Link> infiniteEdges)
        {
            var temporarilyInfiniteEdges = new List<Link>();

            //временно обесконечиваем ребра всех вершин в toReach, кроме первой
            foreach (var edge in Edges)
            {
                if (toReach.Count > 1 && (toReach.Skip(1).Contains(edge.Source) || toReach.Skip(1).Contains(edge.Target)))
                {
                    temporarilyInfiniteEdges.Add(edge);
                }
            }

            var nodeVertex = toReach.FirstOrDefault();
            // находим кратчайший маршрут до следующей вершины, обязательной к посещению

            TryFunc<Node, IEnumerable<Link>> getDijkstraPath = this
                .ShortestPathsDijkstra(e =>
                                            infiniteEdges.Contains(e) ||
                                            temporarilyInfiniteEdges != default &&
                                            temporarilyInfiniteEdges.Contains(e)
                                            ? double.PositiveInfinity
                                            : e.Weight, root);

            if (getDijkstraPath(nodeVertex, out IEnumerable<Link> path))
            {
                if (double.IsPositiveInfinity(path.Sum(p => p.Weight)))
                {
                    return default;
                }
                else
                {
                    var route = new List<Link>(path);
                    var reachedVertices = new List<Node>();
                    // обесконечиваем ребра всех посещенных вершин в кратчайшем маршруте, кроме последней - нового root (у нее обесконечиваем только последний линк к ней и его пару)
                    foreach (Link edge in route.SkipLast(1))
                    {
                        if (!reachedVertices.Contains(edge.Source))
                        {
                            reachedVertices.Add(edge.Source);
                        }

                        if (!reachedVertices.Contains(edge.Target))
                        {
                            reachedVertices.Add(edge.Target);
                        }
                    }

                    // явно обесконечиваем начальную вершину (она не попадает в reachedVertices ранее для случая маршрута из одного ребра)
                    reachedVertices.Add(root);

                    foreach (Link edge in Edges)
                    {
                        if (!infiniteEdges.Contains(edge) && (reachedVertices.Contains(edge.Source) || reachedVertices.Contains(edge.Target)))
                        {
                            infiniteEdges.Add(edge);
                        }
                    }

                    infiniteEdges.AddRange(Edges.Where(edge => edge.ID == route[^1].ID));

                    root = toReach[0];
                    toReach.RemoveAt(0);

                    return route;
                }
            }

            return default;
        }

        /// <summary>
        /// Функция, возвращающая все перестановки списка
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
        {
            if (length == 1)
            {
                return list.Select(t => new T[] { t });
            }

            return GetPermutations(list, length - 1)
                .SelectMany(t => list.Where(e => !t.Contains(e)),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }
        #endregion
    }
}
