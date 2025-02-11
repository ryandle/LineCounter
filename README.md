# Line of Code Counter

This C# console application written almost entirely with github copilot workspace.

## How to Run

1. Clone the repository:
   ```
   git clone https://github.com/githubnext/workspace-blank.git
   ```

2. Navigate to the project directory:
   ```
   cd workspace-blank
   ```

3. Build the project:
   ```
   dotnet build
   ```

4. Run the application with the directory path as an argument:
   ```
   dotnet run --project ./workspace-blank <directory-path>
   ```

Replace `<directory-path>` with the path to the folder you want to analyze.

The application will display the total count of lines of code excluding comments and empty lines.

## Passing File Extensions as Command Argument

You can specify the file extensions to be included in the line count by using the `-FileTypes` argument. For example, to include only `.cs`, `.js`, and `.html` files, run the application as follows:
```
dotnet run --project ./workspace-blank <directory-path> -FileTypes cs,js,html
```

Replace `<directory-path>` with the path to the folder you want to analyze.
