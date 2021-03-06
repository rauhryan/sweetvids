Why does `git status` show that all of my files are modified?
--
SweetVids is built by Windows users, so all of the text files have CRLF line endings. These line endings are stored as-is in git (which means we all have autocrlf turned off).
If you have autocrlf enabled, when you retrieve files from git, it will modify all of your files. Your best bet is to turn off autocrlf, and re-create your clone of SweetVids.

1. Delete your local clone of the SweetVids repository
1. Type: `git config --global core.autocrlf false`
1. Type: `git config --system core.autocrlf false`
1. Clone the SweetVids repository again

Where is CommonAssemblyInfo.cs?
--

CommonAssemblyInfo.cs is generated by the build. The build script requires Ruby with rake installed.

1. Run `InstallGems.bat` to get the ruby dependencies (only needs to be run once per computer)
1. open a command prompt to the root folder and type `rake` to execute rakefile.rb

If you do not have ruby:

1. You need to manually create a src\CommonAssemblyInfo.cs file 

  * type: `echo // > src\CommonAssemblyInfo.cs`
1. open src\SweetVids.sln with Visual Studio and Build the solution

How to get SweetVids running?
--
 > Updated : Tho sweetvids does support some cool things with tarantino and database verisioning
 > Sweetvids now uses sqlite so you should now be able to simply open the project in VS2010 and 
 > launch the application. Depending on your hardware set up you may have some trouble with x64 vs x86 machines
 
Sweet vids uses FluentNHibernate but we are using some custom rake commands to get the database up and running.

1. the rake command `rake dbCreateDev ` should create the required sqlServer container and run the initial schema scripts.

SweetVids does current require a default install of sql server at (localhost) if you are using SqlExpress 
you can change the DBSERVER variable in rakefile.rb to: 

	> DBSERVER = "localhost\sqlexpress"
	
but please do not commit these changes.
