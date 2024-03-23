using GG.Language;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace GG.TileMapSystem
{
    public class TileSearcher
    {
        private LanguageModel _languageModel;
        private TileMap _tileMap;

        List<Node> nodes;

        public TileSearcher(TileMap tileMap, LanguageModel languageModel)
        {
            _languageModel = languageModel;
            _tileMap = tileMap;
        }

        public async Task<bool> CanFormAnyWord()
        {
            nodes = CreateNodes();

            return await TestNodes(nodes);
        }

        private List<Node> CreateNodes()
        {
            var result = new List<Node>();
            var initialTiles = _tileMap.GetInteractableTiles();
            foreach (var tile in initialTiles)
            {
                var node = new Node(null, tile, null);
                result.Add(node);
            }
            foreach (var node in result)
            {
                node.AddConnections(result.FindAll(x => x != node));
                node.CreateChildrenNodes(_tileMap);
            }

            return result;
        }

        private async Task<bool> TestNodes(List<Node> nodes)
        {
            Stack<Route> routeStack = new();
            
            foreach(Node node in nodes)
            {
                PushAll(routeStack, node.GetPossibleRoutes(null));
            }

            var result = false;
            int iterations = 0;
            int testCount = 0;

            while (routeStack.Count > 0)
            {
                testCount++;
                if(testCount == 50)
                {
                    testCount = 0;
                    await Task.Delay(20);
                }

                iterations++;
                if(iterations > 10000)
                {
#if UNITY_EDITOR
                    Debug.LogWarning("Iteration limit reached");
#endif
                    result = false;
                    break;
                }

                var route = routeStack.Pop();
                var word = route.GetWord();
#if UNITY_EDITOR
                Debug.Log("Looking for word: " + word);
#endif
                if (_languageModel.WordExist(route.GetWord()))
                {
                    Debug.Log("Word " + word + " can be formed");
                    result = true;
                    break;
                }

                if (route.GetRouteLength() >= 7) continue;

                PushAll(routeStack, route.GetAllPossibleRoutes());
            }

            return result;
        }

        private Stack<T> PushAll<T>(Stack<T> stack, List<T> items)
        {
            foreach(var item in items)
            {
                stack.Push(item);
            }

            return stack;
        }
    }

    public class Route
    {
        public Route ParentRoute;
        public Node StartNode;
        public Node EndNode;

        public Route(Route parentRoute, Node startNode, Node endNode)
        {
            ParentRoute = parentRoute;
            StartNode = startNode;
            EndNode = endNode;
        }

        public List<Route> GetAllPossibleRoutes()
        {
            return EndNode.GetPossibleRoutes(this);
        }

        public int GetRouteLength()
        {
            if(ParentRoute == null) return 2;

            return ParentRoute.GetRouteLength() + 1;
        }

        public string GetWord()
        {
            if(ParentRoute == null)
            {
                string result = string.Empty;
                result += StartNode.Letter;
                result += EndNode.Letter;
                return result;
            }

            var parentWord = ParentRoute.GetWord();
            parentWord += EndNode.Letter;
            return parentWord;
        }

        public bool ContainsNode(Node node)
        {
            bool itselfContains = StartNode == node || EndNode == node;

            if (ParentRoute == null)
            {
                return itselfContains;
            }

            return itselfContains || ParentRoute.ContainsNode(node);
        }
    }

    public class Node
    {
        public Node ParentNode;
        public BaseTile BaseTile;
        public List<Node> Connections;

        public string Letter => BaseTile.LevelTile.character.ToLower();

        public Node(Node parentNode, BaseTile baseTile, List<Node> connections)
        {
            ParentNode = parentNode;
            BaseTile = baseTile;
            Connections = connections;
            if(ParentNode != null) AddConnections(parentNode.Connections);
        }

        public void AddConnections(List<Node> connections)
        {
            Connections ??= new();

            foreach(var con in connections)
            {
                if (con == this || Connections.Contains(con)) continue;

                Connections.Add(con);
            }
        }

        public void CreateChildrenNodes(TileMap tileMap)
        {
            var tilesToUnlock = tileMap.GetTilesWillBeUnlocked(BaseTile);
            var nodes = new List<Node>();
            
            foreach (var tile in tilesToUnlock)
            {
                var node = new Node(this, tile, null);
            }

            foreach(var node in nodes)
            {
                node.AddConnections(nodes.FindAll(x => x != node));
                node.CreateChildrenNodes(tileMap);
            }
        }

        public List<Route> GetPossibleRoutes(Route parentRoute)
        {
            var result = new List<Route>();

            foreach(var node in Connections)
            {
                if(parentRoute != null)
                {
                    if(parentRoute.ContainsNode(node)) continue;
                }

                result.Add(new Route(parentRoute, this, node));
            }

            return result;
        }
    }
}