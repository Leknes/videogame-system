# Video Game Core Game Framework
This framework provides the groundwork for deviding a game into the core game experience and the main menu. 
This is done, using the `CoreGameManager` with requires implementations of the `ICoreGameStarter` and `ICoreGameExiter` interface. 
Then, `ILoadable` objects can be subscribed to the manager which will then load and unload, corresponding to if the core game is started or exited.
