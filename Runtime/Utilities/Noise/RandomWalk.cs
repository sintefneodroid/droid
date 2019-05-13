using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace droid.Runtime.Utilities.Noise
{
  public class RandomWalk : MonoBehaviour //Self-Avoiding Random Walk algorithm
  {
    //How many steps do we want to take before we stop?
    public int stepsToTake;

    //Final random walk positions
    List<Vector3> _randomWalkPositions;

    //The walk directions we can take
    List<Vector3> _allPossibleDirections = new List<Vector3>
    {
      new Vector3(0f, 0f, 1f),
      new Vector3(0f, 0f, -1f),
      new Vector3(-1f, 0f, 0f),
      new Vector3(1f, 0f, 0f)
    };

    void Update()
    {
      if (Input.GetKeyDown(KeyCode.Return))
      {
        this._randomWalkPositions = this.GenerateSelfAvoidingRandomWalk();

        //Debug.Log(randomWalkPositions.Count);
      }

      //Display the path with lines
      if (this._randomWalkPositions != null && this._randomWalkPositions.Count > 1)
      {
        for (var i = 1; i < this._randomWalkPositions.Count; i++)
        {
          Debug.DrawLine(this._randomWalkPositions[i - 1], this._randomWalkPositions[i]);
        }
      }
    }

    public List<Vector3> GenerateSelfAvoidingRandomWalk()
    {
      //Create the node we are starting from
      var startPos = Vector3.zero;

      var currentNode = new WalkNode(startPos, null, new List<Vector3>(this._allPossibleDirections));

      //How many steps have we taken, so we know when to stop the algorithm
      var stepsSoFar = 0;

      //So we dont visit the same node more than once
      var visitedNodes = new List<Vector3> {startPos};


      while (true)
      {
        //Check if we have walked enough steps
        if (stepsSoFar == this.stepsToTake)
        {
          //Debug.Log("Found path");

          break;
        }

        //Need to backtrack if we cant move in any direction from the current node
        while (currentNode.PossibleDirections.Count == 0)
        {
          currentNode = currentNode.PreviousNode;

          //Dont need to remove nodes thats not a part of the final path from the list of visited nodes
          //because there's no point in visiting them again because we know we cant find a path from those nodes

          stepsSoFar -= 1;
        }

        //Walk in a random direction from this node
        var randomDirPos = Random.Range(0, currentNode.PossibleDirections.Count);

        var randomDir = currentNode.PossibleDirections[randomDirPos];

        //Remove this direction from the list of possible directions we can take from this node
        currentNode.PossibleDirections.RemoveAt(randomDirPos);

        //Whats the position after we take a step in this direction
        var nextNodePos = currentNode.Pos + randomDir;

        //Have we visited this position before?
        if (!this.HasVisitedNode(nextNodePos, visitedNodes))
        {
          //Walk to this node
          currentNode = new WalkNode(nextNodePos, currentNode, new List<Vector3>(this._allPossibleDirections));

          visitedNodes.Add(nextNodePos);

          stepsSoFar += 1;
        }
      }

      //Generate the final path
      var randomWalkPositions = new List<Vector3>();

      while (currentNode.PreviousNode != null)
      {
        randomWalkPositions.Add(currentNode.Pos);

        currentNode = currentNode.PreviousNode;
      }

      randomWalkPositions.Add(currentNode.Pos);

      //Reverse the list so it begins at the step we started from
      randomWalkPositions.Reverse();

      return randomWalkPositions;
    }

    //Checks if a position is in a list of positions
    bool HasVisitedNode(Vector3 pos, List<Vector3> listPos)
    {
      var hasVisited = false;

      foreach (var t in listPos)
      {
        var distSqr = Vector3.SqrMagnitude(pos - t);

        //Cant compare exactly because of floating point precisions
        if (distSqr < 0.001f)
        {
          hasVisited = true;

          break;
        }
      }

      return hasVisited;
    }

    //Help class to keep track of the steps
    class WalkNode
    {
      //The position of this node in the world
      public Vector3 Pos;

      public WalkNode PreviousNode;

      //Which steps can we take from this node?
      public List<Vector3> PossibleDirections;

      public WalkNode(Vector3 pos, WalkNode previousNode, List<Vector3> possibleDirections)
      {
        this.Pos = pos;

        this.PreviousNode = previousNode;

        this.PossibleDirections = possibleDirections;
      }
    }
  }
}