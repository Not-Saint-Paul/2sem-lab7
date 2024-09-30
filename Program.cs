using System;
using System.Collections;
using System.Collections.Generic;

namespace Lab07
{
    public class BinaryTreeNode<T>
    {
        public T DataInNode { get; set; }
        public BinaryTreeNode<T> LeftLowerNode { get; set; }
        public BinaryTreeNode<T> RightLowerNode { get; set; }
        public BinaryTreeNode<T> Parent { get; set; }

        public int StepsToNode { get; set; }
    }

    public class BinaryTree<T> : IEnumerable<T>
    {
        private BinaryTreeNode<T> _nodeInTree;
        private BinaryTreeNode<T> _rootOfTree;
        private BinaryTreeNode<T> _currentNode;

        public BinaryTree(BinaryTreeNode<T> inputTreeNode)
        {
            _nodeInTree = inputTreeNode;
            _rootOfTree = _nodeInTree;
        }

        public BinaryTreeNode<T> GetNextNode(BinaryTreeNode<T> inputTreeNode)
        {
            if (inputTreeNode.LeftLowerNode != null)
            {
                _currentNode = inputTreeNode.LeftLowerNode;
            }
            else if (inputTreeNode.RightLowerNode != null)
            {
                _currentNode = inputTreeNode.RightLowerNode;
            }
            else
            {
                _currentNode = inputTreeNode;
            }
            return _currentNode;
        }

        public BinaryTreeNode<T> GetParent(BinaryTreeNode<T> inputTreeNode)
        {
            if (inputTreeNode.Parent != null)
            {
                _currentNode = inputTreeNode.Parent;
            }
            else
            {
                _currentNode = inputTreeNode;
            }
            return _currentNode;
        }
        public static BinaryTree<T> operator ++(BinaryTree<T> inputTree)
        {
            inputTree.GetNextNode(inputTree._rootOfTree);
            return inputTree;
        }
        public static BinaryTree<T> operator --(BinaryTree<T> inputTree)
        {
            inputTree.GetParent(inputTree._rootOfTree);
            return inputTree;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return InOrderTraversal(_nodeInTree).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private List<T> InOrderTraversal(BinaryTreeNode<T> inputNode)
        {
            List<T> resultList = new List<T>();
            InOrderTraversalRecursive(inputNode, resultList);
            return resultList;
        }

        private void InOrderTraversalRecursive(BinaryTreeNode<T> inputNode, List<T> resultList)
        {
            if (inputNode != null)
            {
                if (inputNode.LeftLowerNode != null && inputNode.RightLowerNode != null)
                {
                    InOrderTraversalRecursive(inputNode.LeftLowerNode, resultList);
                    resultList.Add(inputNode.DataInNode);
                    InOrderTraversalRecursive(inputNode.RightLowerNode, resultList);
                    resultList.Add(inputNode.DataInNode);
                }
                else if (inputNode.LeftLowerNode == null)
                {
                    InOrderTraversalRecursive(inputNode.RightLowerNode, resultList);
                    resultList.Add(inputNode.DataInNode);
                }
                else
                {
                    InOrderTraversalRecursive(inputNode.LeftLowerNode, resultList);
                    resultList.Add(inputNode.DataInNode);
                }
            }
        }

        public BinaryTreeNode<T> Next(BinaryTreeNode<T> inputNode)
        {
            if (inputNode == null)
            {
                return null;
            }

            if (inputNode.LeftLowerNode == null)
            {
                return inputNode;
            }
            else
            {
                if (inputNode.RightLowerNode == null)
                {
                    BinaryTreeNode<T> temporaryNode = inputNode.LeftLowerNode;

                    while (temporaryNode.LeftLowerNode != null)
                    {
                        temporaryNode = temporaryNode.LeftLowerNode;
                        ++temporaryNode.StepsToNode;
                    }
                    return temporaryNode;
                }
                else
                {
                    BinaryTreeNode<T> firstSplitNode = this.Next(inputNode.LeftLowerNode);
                    BinaryTreeNode<T> secondSplitNode = this.Next(inputNode.RightLowerNode);

                    if (firstSplitNode.StepsToNode <= secondSplitNode.StepsToNode)
                    {
                        return firstSplitNode;
                    }
                    else
                    {
                        return secondSplitNode;
                    }


                }
            }
        }

        public BinaryTreeNode<T> Previous(BinaryTreeNode<T> inputNode)
        {
            if (inputNode == null)
            {
                return null;
            }

            if (inputNode.RightLowerNode == null)
            {
                return inputNode;
            }
            else
            {
                if (inputNode.LeftLowerNode == null)
                {
                    BinaryTreeNode<T> temporaryNode = inputNode.RightLowerNode;

                    while (temporaryNode.LeftLowerNode != null)
                    {
                        temporaryNode = temporaryNode.LeftLowerNode;
                        ++temporaryNode.StepsToNode;
                    }
                    return temporaryNode;
                }
                else
                {
                    BinaryTreeNode<T> firstSplitNode = this.Previous(inputNode.RightLowerNode);
                    BinaryTreeNode<T> secondSplitNode = this.Previous(inputNode.LeftLowerNode);

                    if (firstSplitNode.StepsToNode <= secondSplitNode.StepsToNode)
                    {
                        return firstSplitNode;
                    }
                    else
                    {
                        return secondSplitNode;
                    }


                }
            }
        }

        public IEnumerable<T> GetSortedNodes(Func<T, T, int> comparison)
        {
            List<T> nodeList = new List<T>();
            InOrderTraversalForExternal(this._nodeInTree, nodeList, comparison);
            return nodeList;
        }

        private void InOrderTraversalForExternal(BinaryTreeNode<T> inputNode, List<T> nodeList, Func<T, T, int> comparison)
        {
            if (inputNode != null)
            {
                InOrderTraversalForExternal(inputNode.LeftLowerNode, nodeList, comparison);
                nodeList.Add(inputNode.DataInNode);
                InOrderTraversalForExternal(inputNode.RightLowerNode, nodeList, comparison);
            }
        }

        public void AddValue(T inputValue)
        {
            _nodeInTree = AddValueRecursive(_nodeInTree, inputValue);
        }

        public BinaryTreeNode<T> GetCurrentNode()
        {
            return _currentNode;
        }

        private BinaryTreeNode<T> AddValueRecursive(BinaryTreeNode<T> _currentNode, T inputValue)
        {
            if (_currentNode == null)
            {
                return new BinaryTreeNode<T> { DataInNode = inputValue };
            }

            IComparable<T> comparable = inputValue as IComparable<T>; 
            if (comparable.CompareTo(_currentNode.DataInNode) < 0)
            {
                _currentNode.LeftLowerNode = AddValueRecursive(_currentNode.LeftLowerNode, inputValue);
            }
            else if (comparable.CompareTo(_currentNode.DataInNode) > 0)
            {
                _currentNode.RightLowerNode = AddValueRecursive(_currentNode.RightLowerNode, inputValue);
            }

            return _currentNode;
        }

        public void Reset()
        {
            _currentNode = GetRoot(_rootOfTree);
        }

        private BinaryTreeNode<T> GetRoot(BinaryTreeNode<T> inputNode)
        {
            if (inputNode == null)
            {
                return null;
            }

            while (inputNode.LeftLowerNode != null)
            {
                inputNode = inputNode.LeftLowerNode;
            }

            return inputNode;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            int userChoice;
            int inputNumber;
            bool workYouWeakWilledMachine = true;
            BinaryTreeNode<int> outputNode;

            Console.WriteLine("Введите число для создания дерева:");
            inputNumber = Convert.ToInt32(Console.ReadLine());

            BinaryTreeNode<int> inputTreeNode = new BinaryTreeNode<int> { DataInNode = inputNumber };
            BinaryTree<int> binaryTree = new BinaryTree<int>(inputTreeNode);

            while (workYouWeakWilledMachine)
            {
                Console.WriteLine("Выберите действие: 1 - добавить элемент, 2 - поиск узла без левого дочернего элемента, 3 - сброс, 4 - поиск узла без правого дочернего элемента, " + 
                                  "5 - получить все элементы, 6 - получить все элементы (альтернативный вариант), 7 - текущий элемент, 8 - получить следующий элемент," +
                                  " 9 - получить предыдущий элемент, 10 - выход ");

                userChoice = Convert.ToInt16(Console.ReadLine());

                switch (userChoice)
                {
                    case 1:
                        Console.WriteLine("Введите число");
                        inputNumber = Convert.ToInt32(Console.ReadLine());
                        binaryTree.AddValue(inputNumber);
                        break;
                    case 2:
                        binaryTree.Reset();
                        outputNode = binaryTree.Next(inputTreeNode);
                        Console.WriteLine("Узел без левого дочернего элемента: " + outputNode.DataInNode.ToString());
                        break;
                    case 3:
                        binaryTree.Reset();
                        Console.WriteLine("Сброс. Текущий элемент:" + inputTreeNode.DataInNode.ToString());
                        break;
                    case 4:
                        binaryTree.Reset();
                        binaryTree.Previous(inputTreeNode);
                        Console.WriteLine("Узел без правого дочернего элемента: " + inputTreeNode.DataInNode.ToString());
                        break;
                    case 5:
                        binaryTree.Reset();

                        foreach (int inputValue in binaryTree)
                        {
                            Console.WriteLine(inputValue);
                        }
                        break;
                    case 6:
                        binaryTree.Reset();
                        var sortedNodes = binaryTree.GetSortedNodes((x, y) => x.CompareTo(y));

                        foreach (var inputValue in sortedNodes)
                        {
                            Console.WriteLine(inputValue);
                        }
                        break;
                    case 7:
                        binaryTree.GetCurrentNode();
                        Console.WriteLine("Текущий элемент:" + inputTreeNode.DataInNode.ToString());
                        break;
                    case 8:
                        binaryTree = ++binaryTree;
                        inputTreeNode = binaryTree.GetCurrentNode();
                        Console.WriteLine("Текущий элемент:" + inputTreeNode.DataInNode.ToString());
                        break;
                    case 9:
                        binaryTree = --binaryTree;
                        inputTreeNode = binaryTree.GetCurrentNode();
                        Console.WriteLine("Текущий элемент:" + inputTreeNode.DataInNode.ToString());
                        break;
                    case 10:
                        workYouWeakWilledMachine = false;
                        Console.WriteLine("Нажмите любую кнопку");
                        break;
                    default:
                        Console.WriteLine("Неверная команда. Повторите ввод");
                        break;
                }
            }
            Console.ReadKey();
        }
    }
}