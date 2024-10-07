-----------------------------
*                           *
*   EnemyAI Documentation   *
*                           *
-----------------------------

OVERVIEW (Files & Folders):

The Enemies Folder:
    Holds all coded enemies; just drop into a game object.

The Sensors Folder:
    Holds all coded sensors; just drop into a game object

The States Folder:
    Holds all states an enemy can have; use this when assembling enemies, rather than hard coding it into the enemy class.

The EnemyAI Folder:
    Ideally you will not need to acess this file, it holds the parent classes, interfaces, FSM code, and EnemySensorManagers code.
    * EnemyAI :             Parent to all enemy classes                         (RECCOMENDED NOT TO EDIT!)
    * IState :              Interface of all states                             (100% DO NOT EDIT!)
    * StateMachine :        The foundation of all state machine instances       (100% DO NOT EDIT!)
    * DetectableTarget :    Attach this to any target you wish to be visually or proximity detecable (100% DO NOT EDIT)
    * DetectablesManager :  Tracks detectables, must be in scene                (100% DO NOT EDIT!)
    * SensorManager :       Tracks all sensors, must be in scene, can edit to add more sensor types
    * EnemyVisionManager :  Tracks which sensors the enemy looks through, and how to interprete what it sees (probably don't edit(?))

---------------------------------------------------------------------
We now discuss how to code enemies, states, and sensors (focus not for level design team, but for coding team)
---------------------------------------------------------------------

>                         <
>   Making an Enemy 101   <
>                         <

Golden Rules:
* Enemies all inherit from the EnemyAI class
* Never call Update() inside an enemy; always use Tick() !
* To use sensors, make an instance of the Enemy[SensorTypeHere]Manager
* You must tell the sensor which things you want it to listen to, you can get all existing sensors of a certain type from SensorManager

FSM
* Follow the template:
* Setup the FSM, the states, the initial state, and the transition functions

Adding Sensors:
* To add a sensor, initialize an instance of the corrosponding Enemy[SensorTypeHere]Manager class
* You may need to look through said class to know what inputs the constructor requires
* And you are done.

IMPLEMENTATION NOTICE: Make sure you add a NavMeshAgent to the enemy in unity (see unity documentation on navigation meshes!)

---------------------------------------------------------------------

>                        <
>   Making a State 101   <
>                        <

Golden Rules:
* Always leave the state exactly as you entered it!

Baiscs:
* Follow the template:
* Define your constructor, onEntry, onTick, and onExit functions

---------------------------------------------------------------------

>                         <
>   Making a Sensor 101   <
>                         <

Golden Rules:
* Warning: sensors are by far the most complex and one-off parts; make sure you know what you are doing before continuing!
* The ideal sensor has three parts: 
    * a method to check if it senses anything, 
    * a method to tell all listening enemies its senses have changed,
    * and methods to register itself to the SensorManager (and let enemies register to this sensor)

Coding Step-by-Step:
* Make your new sensor class
* Add a new sensor HashSet in SensorManager to track all instances of this sensor type
* Register the sensor to the SensorManager, and dont forget to deregister on destroy!
* Now write code as to what you want to sense and how
* Now make a new Enemy[SensorName]Manager file in the SensorManagers Folder
* This file will be responsible for collecting all information for sensors of your type that the enemy is "listening" to
* Setup functions to register and deregister enemy sensor manager instances to the sensor (dont forget "onDestroy"!)
* Now implement some function in this sensor manager you want called by a sensor it listens to, when such a sensor changes state
* Now back in the sensor code; add a method to call said function for all "listening" managers