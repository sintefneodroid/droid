#############################################################################
#    _   _   ______    ____    _____    _____     ____    _____   _____     #
#   | \ | | |  ____|  / __ \  |  __ \  |  __ \   / __ \  |_   _| |  __ \    #
#   |  \| | | |__    | |  | | | |  | | | |__) | | |  | |   | |   | |  | |   #
#   | . ` | |  __|   | |  | | | |  | | |  _  /  | |  | |   | |   | |  | |   #
#   | |\  | | |____  | |__| | | |__| | | | \ \  | |__| |  _| |_  | |__| |   #
#   |_| \_| |______|  \____/  |_____/  |_|  \_\  \____/  |_____| |_____/    #
#                                                                           #
#############################################################################

-------------------------------- TUTORIAL -----------------------------------

In this very simple tutorial you create a Neodroid environment from a new scene, please refer to online resources at documentation.neodroid.ml for more information.

1. Create a new scene with a basic prebuilt simulation manager and prototyping environment setup
    - 1.1 [Right click] the scene view in the editor
    - 1.2 Select Neodroid/Prebuilt/SimpleEnvironment
   
2. Using the Player Reaction component to respond to keyboard input
    - 2.1 Select the "SimpleEnvironment" GameObject in the Scene Hierarchy 
    - 2.2 [Left click] the "Add Component" button
    - 2.3 Search for "player"
    - 2.4 Select the "Player Reaction" component
    - 2.5 [Left click] the little circular icon to the right of the "Player_motions" field in the newly added 
  component.
    - 2.6 Select the "DefaultActorMotions" ScriptableObject
  
3. Testing the scene
    - 3.1 Play scene and try moving the actor object about using the [W,A,S,D,Q,E] keys on your keyboard.

4. Replace the existing TransformMotor component with a RigidbodyMotor component
    - 4.1 Ensure Scene is not playing
    - 4.2 Try opening the accompanying Neodroid:Environment window, by navigating to 
  Window/Neodroid/EnvironmentsWindow in the top menu bar
    - 4.3 A window will appear, inside a scrollable view in the bottom part you will be able to inspect a the 
  Neodroid relevant information the currently open scene
    - 4.4 Expand the "SimpleEnvironmentPrototypingEnvironment"
    - 4.5 Find the "Motor" sub-box
    - 4.6 [Double Left Click] one of the boxes with a tiny GameController icon, this select the occupant 
   GameObject of the Motor component
    - 4.7 Remove the "Euler Transform Motor" by [Right click]ing on the title and selecting [Remove component]
   in the inspector View
    - 4.8 with actor object still selected, scroll to the bottom in the inspector view and [Left Click] the 
  "Add Component" button
    - 4.9 Search for "rigidbody"
    - 4.10 Select the "RigidbodyMotor3DofMotor"
  
5. Configure Motion Space
    - 5.1 Expand the "Motion_value_space"
    - 5.2 Insert the values -100 and 100 in the "Min_Value" and "Max_Value" respectively

6. Disable gravity on the Actors rigidbody 
    - 6.1 Find the rigidbody component of the "Actor"
    - 6.2 Uncheck the "Use Gravity" field 

7. Testing the scene again
    - 7.1 Play scene and try moving the actor object about using the [W,A,S,D,Q,E] keys on your keyboard.
    - 7.2 Now we are affecting the rigidbody of the GameObject through the physics engine instead of the 
  transform directly.

8. ( Assuming you have installed 'neo' Python package )

    Connect an interacting python process to the environment
    - 8.1 While letting the Scene continue playing, open a new shell/console on your computer
    - 8.2 Type in 'neodroid-sample' and hit [Enter]
    - 8.3 Switch back to Unity and watch the python process sample random motions 
    - 8.4 Try disconnecting, reconnecting, stop play and replay the agent will try to reconnect a keep sampling

9. See the observations and signals given by the environment
    - 9.1 Ensure both the Unity scene and Python process is running
    - 9.2 Switch to the console and observe the values being printed, these a values return to the process from 
  the running Unity scene, we call these observations and signals and are necessaries for doing reinforcement learning, the Neodroid eco-system set up of this up for you in the scene you built
    - 9.3 Again refer to the online resources for more information at documentation.neodroid.ml

10. Lastly feel free to report any issues at https://github.com/sintefneodroid/droid/issues.

Have a great day!
