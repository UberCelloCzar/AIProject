using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heap // the min-heap used with A*
{
    public double[] keys = new double[MAX_SIZE]; // Keys (sort priority IDs) (g(n) distance)
    public int[][] nodeIndexes = new int[MAX_SIZE][]; // Nodes (indexes (in nodes[]) of each node)
    public int size = 0; // Number of nodes in the min-heap

    public Heap() // Constructor
    {

    }

    const int MAX_SIZE = 100; // Constant for the maximum number of keys and nodes in the min-heap

    int heapifyUp(int i) // Checks and moves current node up if needed
    {
        if (i <= 0) return 0;
        int j = getParent(i);
        if (keys[i] < keys[j]) swap(i, j); // If this node is smaller than parent, move up until it's not
        heapifyUp(j);
        return 0;
    }

    int heapifyDown(int i) // Checks and moves current node down if needed
    {
        if (getChild('l', i) > size - 1) return 0; // If there are no children

        int j;
        if (getChild('r', i) > size - 1) // If there is no right child
        {
            j = getChild('l', i);
        }
        else // If both children are present
        {
            j = (keys[getChild('l', i)] < keys[getChild('r', i)]) ? (getChild('l', i)) : (getChild('r', i)); // Check against the lesser child
        }

        if (keys[i] > keys[j]) swap(i, j);
        heapifyDown(j);
        return 0;
    }

    int getParent(int i) // Gets the key of the parent of the current node
    {
        return (i - 1) / 2;
    }

    int getChild(char side, int i) // Gets the key of either child of the current node
    {
        if (side == 'r') // Right child
        {
            return 2 * i + 2;
        }
        else // Left child
        {
            return 2 * i + 1;
        }
    }

    int swap(int i, int j) // Swaps 2 nodes
    {
        double tempKey = keys[i];
        int[] tempVal = nodeIndexes[i];
        keys[i] = keys[j];
        nodeIndexes[i] = nodeIndexes[j];
        keys[j] = tempKey;
        nodeIndexes[j] = tempVal;
        return 0;
    }

    public int push(double key, int[] nodeIndex) // Adds a node to the min-heap, then re-heapifies
    {
        if (size == MAX_SIZE) // If this entry would make the heap too big, gracefully fail
        {
            return 1;
        }
        nodeIndexes[size] = nodeIndex; // Add the node
        keys[size] = key; // Add the priority of the node
        ++size;
        heapifyUp(size - 1); // Check the heap
        return 0;
    }

    public int[] pop() // Gets the top value and re-heapifies
    {
        int[] temp = nodeIndexes[0];
        keys[0] = keys[size - 1]; // Dump the last node into the first spot, remove the last spot, and re-heapify
        nodeIndexes[0] = nodeIndexes[size - 1];
        keys[size - 1] = 0;
        nodeIndexes[size - 1] = new int[] { 0, 0 };
        --size;
        heapifyDown(0);
        return temp;
    }
}
