# Take - Chat

This project represents a chat with some functionalities (like message, private message, nickname ) implemented in a WebSocket using .Net Core 3.1 framework.

## Getting Started

1. Clone the entire solution.
2. Open the solution on Visual Studio
3. Adjust the target project to the project you want to debug.
(Optional)
3.1. To debug TakeProject.Server
3.1.1. Select TakeProject.Server as the startup project. 
3.1.2. Run project on Visual Studio using IIS.
3.1.3. Start the number of users you want as PowerShell or Prompt instances and go to the cloned repository.
3.1.4. Go to TakeProject.Client folder. 
3.1.5. Run "dotnet build" in one instance of PS/CMD.
3.1.6. Run "dotnet run" in all instances you want to create as a client of chat.
(Optional)
3.2. To debug TakeProject.Client
3.1.1. Select TakeProject.Client as the startup project. 
3.1.2. Start a PowerShell or Prompt instance to chat server and go to the cloned repository.
3.1.3. Start the number of users you want as PowerShell or Prompt instances and go to the cloned repository.
3.1.4. Go to TakeProject.Server folder in one PS/CMD. 
3.1.5. Run "dotnet build" in server instance of PS/CMD.
3.1.6. Run project on Visual Studio.
3.1.6. Run "dotnet run" in all other instances you want to create as a client of chat.
4. Enjoy.


### Prerequisites

Visual Studio (recommended 2019) with .net Core 3.1.

## Running the tests

1. Open the solution on Visual Studio.
2. In "solution explorer", right click on TakeProject.Tests project.
3. Click in "Run tests".

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details
