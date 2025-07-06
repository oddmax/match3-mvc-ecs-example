# Match3 example game

# How to test
Open MainScene.scene and press 'Play'. Game was optimized for mobile portait mode so it will look best with a landscape aspect ratio.

# Description
A simple game to demonstraite a clean and robust architecture for the match-3 game which can be easily extended with any extra functionality. 
Uses a hybrid architecture - as foundation I use MVC-like architecture based on **Zenject** with **Command** pattern, but for match3 mechanics I used **Unity DOTS Entity Component System** as it works very well with this kind of logic.

*Built with Unity 6000.1.6f1*

# Funcionality

Has base match-3 mechanics and different levels. All levels and settings could be setup as Scriptable Objects.

# General structure

* Uses very simple **MVC/MVP architecture** and **Command** pattern for main application logic
* Uses **Unity DOTS Entity Component System** for match-3 gameplay mechanics.
* Uses **Zenject** of inversion of control and dependencies injections
* Uses **SignalBus** from Zenject to decouple communication between different classes
* Implemented a custom State Machine to switch between global states (Boot -> LevelMap -> GameBoard) with a support of more complex State Charts
* Same State Chart is used to flow through match-3 states with a State pattern (PlyaerTurn -> Swap -> MatchesDestuction -> etc)
* Implemented a simple Pooling class for optimizing board gems usage
* Scripts/Core folder holds all core funtionality (Main state machine, Main Context installer)
* Scripts/Features folder holds feature related logic

# Main classes and their descriptions

## Models 
* Is Plain C# class
* Holds application state
* Injectable into Presenters, Commands and Services (in case of adding backend). Not injectable to other models.
* Usually doesn't have injected dependencies
* Serialiazable for both sending to the backend or to be saved on the device

## Views
* Is Subclass of MonoBehaviour
* Can have references to a group of View Elements and child Views that share the same purpose
* Listens to user input on the view elements and forwards them to the Presenter
* Not injectable
* Doesn't have injected dependencies

## Presenter
Has references to Views
* Listens to changes in Models and updates the Views it is responsible for
* Has injected dependencies to Models, SignalBus, and Services.
* Listens to user inputs passed from the views or directly and executes commands

## Configs located in Config folder
* Is a ScriptableObject assets
* Gets serialized 
* Holds static information of the objects which doesn't change such as levels, how much score points for different matches to give, etc

## Signals
* Is Plain C# class
* Used to notify other parts of the system of models updates or to execute a command

## Command
* Is Plain C# class
* Micro controller responsible to execute one action and update all necessary models based on result of this action

## DOTS Components
* Data container for ECS systems

## DOTS Systems
* Base on archetype of entity will execute some data changes to components and can notify about them via signals.


