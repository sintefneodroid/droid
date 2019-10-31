using droid.Runtime.Environments.Prototyping;
using droid.Runtime.GameObjects.ChildSensors;
using droid.Runtime.Interfaces;
using droid.Runtime.Prototyping.Actors;
using UnityEngine;

namespace droid.Runtime.Utilities {
  /// <summary>
  /// </summary>
  public static partial class NeodroidRegistrationUtilities {
    /// <summary>
    ///
    /// </summary>
    /// <param name="caller"></param>
    /// <param name="parent"></param>
    /// <param name="on_collision_enter_child"></param>
    /// <param name="on_trigger_enter_child"></param>
    /// <param name="on_collision_exit_child"></param>
    /// <param name="on_trigger_exit_child"></param>
    /// <param name="on_collision_stay_child"></param>
    /// <param name="on_trigger_stay_child"></param>
    /// <param name="debug"></param>
    /// <typeparam name="TChildColliderSensor"></typeparam>
    /// <typeparam name="TCollider"></typeparam>
    /// <typeparam name="TCollision"></typeparam>
    public static void
        RegisterCollisionTriggerCallbacksOnChildren<TChildColliderSensor, TCollider, TCollision>(
            Component caller,
            Transform parent,
            ChildColliderSensor<TCollider, TCollision>.OnChildCollisionEnterDelegate on_collision_enter_child
                = null,
            ChildColliderSensor<TCollider, TCollision>.OnChildTriggerEnterDelegate on_trigger_enter_child =
                null,
            ChildColliderSensor<TCollider, TCollision>.OnChildCollisionExitDelegate on_collision_exit_child =
                null,
            ChildColliderSensor<TCollider, TCollision>.OnChildTriggerExitDelegate on_trigger_exit_child =
                null,
            ChildColliderSensor<TCollider, TCollision>.OnChildCollisionStayDelegate on_collision_stay_child =
                null,
            ChildColliderSensor<TCollider, TCollision>.OnChildTriggerStayDelegate on_trigger_stay_child =
                null,
            bool debug = false)
        where TChildColliderSensor : ChildColliderSensor<TCollider, TCollision> where TCollider : Component {
      var children_with_colliders = parent.GetComponentsInChildren<TCollider>();

      //TODO add check and warning for not all callbacks = null

      foreach (var child in children_with_colliders) {
        var child_sensors = child.GetComponents<TChildColliderSensor>();
        ChildColliderSensor<TCollider, TCollision> collider_sensor = null;
        foreach (var child_sensor in child_sensors) {
          if (child_sensor.Caller != null && child_sensor.Caller == caller) {
            collider_sensor = child_sensor;
            break;
          }

          if (child_sensor.Caller == null) {
            child_sensor.Caller = caller;
            collider_sensor = child_sensor;
            break;
          }
        }

        if (collider_sensor == null) {
          collider_sensor = child.gameObject.AddComponent<TChildColliderSensor>();
          collider_sensor.Caller = caller;
        }

        if (on_collision_enter_child != null) {
          collider_sensor.OnCollisionEnterDelegate = on_collision_enter_child;
        }

        if (on_trigger_enter_child != null) {
          collider_sensor.OnTriggerEnterDelegate = on_trigger_enter_child;
        }

        if (on_collision_exit_child != null) {
          collider_sensor.OnCollisionExitDelegate = on_collision_exit_child;
        }

        if (on_trigger_exit_child != null) {
          collider_sensor.OnTriggerExitDelegate = on_trigger_exit_child;
        }

        if (on_trigger_stay_child != null) {
          collider_sensor.OnTriggerStayDelegate = on_trigger_stay_child;
        }

        if (on_collision_stay_child != null) {
          collider_sensor.OnCollisionStayDelegate = on_collision_stay_child;
        }

        #if NEODROID_DEBUG
        if (debug) {
          Debug.Log($"{caller.name} has created {collider_sensor.name} on {child.name} under parent {parent.name}");
        }
        #endif
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="r"></param>
    /// <param name="c"></param>
    /// <param name="only_parents"></param>
    /// <param name="debug"></param>
    /// <typeparam name="TCaller"></typeparam>
    /// <returns></returns>
    public static IHasRegister<IActuator> RegisterComponent<TCaller>(
        IHasRegister<IActuator> r,
        TCaller c,
        bool only_parents = false,
        bool debug = false) where TCaller : Component, IRegisterable {
      IHasRegister<IActuator> component = null;
      if (r != null) {
        component = r; //.GetComponent<Recipient>();
      } else {
        if (c.GetComponentInParent<Actor>() != null) {
          component = c.GetComponentInParent<Actor>();
        } else if (!only_parents) {
          component = Object.FindObjectOfType<Actor>();
        }
      }

      if (component == null) {
        if (c.GetComponentInParent<PrototypingEnvironment>() != null) {
          component = c.GetComponentInParent<PrototypingEnvironment>();
        } else if (!only_parents) {
          component = Object.FindObjectOfType<PrototypingEnvironment>();
        }
      }

      if (component != null) {
        component.Register((IActuator)c);
      } else {
        #if NEODROID_DEBUG
        if (debug) {
          Debug.Log($"Could not find a IHasRegister<IActuator> recipient during registration");
        }
        #endif
      }

      return component;
    }

    /// <summary>
    /// </summary>
    /// <param name="r"></param>
    /// <param name="c"></param>
    /// <param name="identifier"></param>
    /// <param name="only_parents"></param>
    /// <param name="debug"></param>
    /// <typeparam name="TRecipient"></typeparam>
    /// <typeparam name="TCaller"></typeparam>
    /// <returns></returns>
    public static IHasRegister<IActuator> RegisterComponent<TCaller>(
        IHasRegister<IActuator> r,
        TCaller c,
        string identifier,
        bool only_parents = false,
        bool debug = false) where TCaller : Component, IRegisterable {
      IHasRegister<IActuator> component = null;
      if (r != null) {
        component = r; //.GetComponent<Recipient>();
      } else {
        if (c.GetComponentInParent<Actor>() != null) {
          component = c.GetComponentInParent<Actor>();
        } else if (!only_parents) {
          component = Object.FindObjectOfType<Actor>();
        }
      }

      if (component == null) {
        if (c.GetComponentInParent<PrototypingEnvironment>() != null) {
          component = c.GetComponentInParent<PrototypingEnvironment>();
        } else if (!only_parents) {
          component = Object.FindObjectOfType<PrototypingEnvironment>();
        }
      }

      if (component != null) {
        component.Register((IActuator)c, identifier);
      } else {
        #if NEODROID_DEBUG
        if (debug) {
          Debug.Log($"Could not find a IHasRegister<IActuator> recipient during registration");
        }
        #endif
      }

      return component;
    }

    /// <summary>
    /// </summary>
    /// <param name="r"></param>
    /// <param name="c"></param>
    /// <param name="only_parents"></param>
    /// <param name="debug"></param>
    /// <typeparam name="TRecipient"></typeparam>
    /// <typeparam name="TCaller"></typeparam>
    /// <returns></returns>
    public static TRecipient RegisterComponent<TRecipient, TCaller>(
        TRecipient r,
        TCaller c,
        bool only_parents = false,
        bool debug = false)
        where TRecipient : Object, IHasRegister<TCaller> where TCaller : Component, IRegisterable {
      TRecipient component = null;
      if (r != null) {
        component = r; //.GetComponent<Recipient>();
      } else if (c.GetComponentInParent<TRecipient>() != null) {
        component = c.GetComponentInParent<TRecipient>();
      } else if (!only_parents) {
        component = Object.FindObjectOfType<TRecipient>();
      }

      if (component != null) {
        component.Register(c);
      } else {
        #if NEODROID_DEBUG
        if (debug) {
          Debug.Log($"Could not find a {typeof(TRecipient)} recipient during registration");
        }
        #endif
      }

      return component;
    }

    /// <summary>
    /// </summary>
    /// <param name="r"></param>
    /// <param name="c"></param>
    /// <param name="identifier"></param>
    /// <param name="only_parents"></param>
    /// <param name="debug"></param>
    /// <typeparam name="TRecipient"></typeparam>
    /// <typeparam name="TCaller"></typeparam>
    /// <returns></returns>
    public static TRecipient RegisterComponent<TRecipient, TCaller>(
        TRecipient r,
        TCaller c,
        string identifier,
        bool only_parents = false,
        bool debug = false)
        where TRecipient : Object, IHasRegister<TCaller> where TCaller : Component, IRegisterable {
      TRecipient component = null;
      if (r != null) {
        component = r;
      } else if (c.GetComponentInParent<TRecipient>() != null) {
        component = c.GetComponentInParent<TRecipient>();
      } else if (!only_parents) {
        component = Object.FindObjectOfType<TRecipient>();
      }

      if (component != null) {
        component.Register(c, identifier);
      } else {
        #if NEODROID_DEBUG
        if (debug) {
          Debug.Log($"Could not find a {typeof(TRecipient)} recipient during registration");
        }

        #endif
      }

      return component;
    }
  }
}
