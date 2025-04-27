Clone the Repository
Open a terminal or Git Bash.
Execute the following command to clone the system to your local machine:
git clone https://github.com/yourusername/enrollment-system.git

Open the Project in Visual Studio
Launch Visual Studio 2022.
Click File > Open > Project/Solution.
Navigate to the cloned folder and open the .sln (Solution) file.

Configure the Database
Install and open MySQL Workbench.
Create a new database schema (e.g., enrollment_db).
Execute the SQL scripts provided to generate the necessary tables and relationships (refer to the schema and ERD).
Ensure that the database contains all required tables: Students, Programs, Departments, Admins, etc.

Update the Database Connection String
In the Visual Studio project, locate the connection string (usually found in App.config or directly in the codebase).
Update the connection string with your MySQL credentials:
Server=localhost;Database=enrollment_db;Uid=root;Pwd=yourpassword;

Restore NuGet Packages
Right-click the solution and select Restore NuGet Packages to ensure all dependencies are installed (e.g., MySQL Connector).
The Enrollment System will launch, ready for use!
