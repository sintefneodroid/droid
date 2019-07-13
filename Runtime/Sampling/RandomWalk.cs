using System.Collections.Generic;
using UnityEngine;

namespace droid.Runtime.Sampling {
  public class RandomWalk : MonoBehaviour //Self-Avoiding Random Walk algorithm
  {
    //How many steps do we want to take before we stop?
    public int stepsToTake;

    //Final random walk positions
    List<Vector3> _random_walk_positions;

    //The walk directions we can take
    List<Vector3> _all_possible_directions = new List<Vector3> {
                                                                   new Vector3(0f, 0f, 1f),
                                                                   new Vector3(0f, 0f, -1f),
                                                                   new Vector3(-1f, 0f, 0f),
                                                                   new Vector3(1f, 0f, 0f)
                                                               };

    void Update() {
      if (Input.GetKeyDown(KeyCode.Return)) {
        this._random_walk_positions = this.GenerateSelfAvoidingRandomWalk();

        //Debug.Log(randomWalkPositions.Count);
      }

      //Display the path with lines
      if (this._random_walk_positions != null && this._random_walk_positions.Count > 1) {
        for (var i = 1; i < this._random_walk_positions.Count; i++) {
          Debug.DrawLine(this._random_walk_positions[i - 1], this._random_walk_positions[i]);
        }
      }
    }

    public List<Vector3> GenerateSelfAvoidingRandomWalk() {
      //Create the node we are starting from
      var start_pos = Vector3.zero;

      var current_node = new WalkNode(start_pos, null, new List<Vector3>(this._all_possible_directions));

      //How many steps have we taken, so we know when to stop the algorithm
      var steps_so_far = 0;

      //So we dont visit the same node more than once
      var visited_nodes = new List<Vector3> {start_pos};

      while (true) {
        //Check if we have walked enough steps
        if (steps_so_far == this.stepsToTake) {
          //Debug.Log("Found path");

          break;
        }

        //Need to backtrack if we cant move in any direction from the current node
        while (current_node._PossibleDirections.Count == 0) {
          current_node = current_node._PreviousNode;

          //Dont need to remove nodes thats not a part of the final path from the list of visited nodes
          //because there's no point in visiting them again because we know we cant find a path from those nodes

          steps_so_far -= 1;
        }

        //Walk in a random direction from this node
        var random_dir_pos = Random.Range(0, current_node._PossibleDirections.Count);

        var random_dir = current_node._PossibleDirections[random_dir_pos];

        //Remove this direction from the list of possible directions we can take from this node
        current_node._PossibleDirections.RemoveAt(random_dir_pos);

        //Whats the position after we take a step in this direction
        var next_node_pos = current_node._Pos + random_dir;

        //Have we visited this position before?
        if (!this.HasVisitedNode(next_node_pos, visited_nodes)) {
          //Walk to this node
          current_node = new WalkNode(next_node_pos,
                                      current_node,
                                      new List<Vector3>(this._all_possible_directions));

          visited_nodes.Add(next_node_pos);

          steps_so_far += 1;
        }
      }

      //Generate the final path
      var random_walk_positions = new List<Vector3>();

      while (current_node._PreviousNode != null) {
        random_walk_positions.Add(current_node._Pos);

        current_node = current_node._PreviousNode;
      }

      random_walk_positions.Add(current_node._Pos);

      //Reverse the list so it begins at the step we started from
      random_walk_positions.Reverse();

      return random_walk_positions;
    }

    //Checks if a position is in a list of positions
    bool HasVisitedNode(Vector3 pos, List<Vector3> list_pos) {
      var has_visited = false;

      foreach (var t in list_pos) {
        var dist_sqr = Vector3.SqrMagnitude(pos - t);

        //Cant compare exactly because of floating point precisions
        if (dist_sqr < 0.001f) {
          has_visited = true;

          break;
        }
      }

      return has_visited;
    }

    //Help class to keep track of the steps
    class WalkNode {
      //The position of this node in the world
      public Vector3 _Pos;

      public WalkNode _PreviousNode;

      //Which steps can we take from this node?
      public List<Vector3> _PossibleDirections;

      public WalkNode(Vector3 pos, WalkNode previous_node, List<Vector3> possible_directions) {
        this._Pos = pos;

        this._PreviousNode = previous_node;

        this._PossibleDirections = possible_directions;
      }
    }
  }
}
