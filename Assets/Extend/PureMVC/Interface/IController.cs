using System;

public interface IController {
	void RegisterCommand(string commandName, Type command);
	void RemoveCommand(string commandName);
	bool HasCommand(string commandName);
}
