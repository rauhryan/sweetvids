COMPILE_TARGET = "debug"
require "build_support/BuildUtils.rb"
  
include FileTest
require 'albacore'
require "build_support/Tarantino.rb"

RESULTS_DIR = "results"
BUILD_NUMBER_BASE = "0.1.0"
PRODUCT = "SweetVids"
COPYRIGHT = 'Copyright 2010 Nine Collective. All rights reserved.';
COMMON_ASSEMBLY_INFO = 'src/CommonAssemblyInfo.cs';
CLR_VERSION = "v4.0.30319"

TARANTINO = "lib\\tarantino\\Tarantino.DatabaseManager.Console.exe"
DBNAME = "SweetVidsData"
VERSIONEDDB = "SweetVidsVersioned"
DBSCRIPTS = "dbChangeScripts"
DBSERVER = "localhost"
VIRDBSERVER = "CASQLSRV01"


MSPEC = "lib\\mspec\\mspec.exe"

props = { :archive => "build" }

desc "Compiles, unit tests, generates the database"
task :all => [:default]

desc "**Default**, compiles and runs tests"
task :default => [:compile, :test]

desc "Update the version information for the build"
assemblyinfo :version do |asm|
  asm_version = BUILD_NUMBER_BASE + ".0"
  
  begin
	gittag = `git describe --all --long`.chomp 	# looks something like v0.1.0-63-g92228f4
    gitnumberpart = /-(\d+)-/.match(gittag)
    gitnumber = gitnumberpart.nil? ? '0' : gitnumberpart[1]
    commit = (ENV["BUILD_VCS_NUMBER"].nil? ? `git log -1 --pretty=format:%H` : ENV["BUILD_VCS_NUMBER"])
  rescue
    commit = "git unavailable"
    gitnumber = "0"
  end
  build_number = "#{BUILD_NUMBER_BASE}.#{gitnumber}"
  tc_build_number = ENV["BUILD_NUMBER"]
  puts "##teamcity[buildNumber '#{build_number}-#{tc_build_number}']" unless tc_build_number.nil?
  asm.trademark = commit
  asm.product_name = "#{PRODUCT} #{gittag}"
  asm.description = build_number
  asm.version = asm_version
  asm.file_version = build_number
  asm.custom_attributes :AssemblyInformationalVersion => asm_version
  asm.copyright = COPYRIGHT
  asm.output_file = COMMON_ASSEMBLY_INFO
end

desc "Prepares the working directory for a new build"
task :clean do
	#TODO: do any other tasks required to clean/prepare the working directory
	Dir.mkdir props[:archive] unless exists?(props[:archive])
end


desc "Compiles the app"
task :compile => [:clean, :version, :compileSln, :compileAsp]

desc "Compiles the app"
task :compileAsp => [:clean, :version] do

  AspNetCompilerRunner.compile :webPhysDir => "src/SweetVids.Web", :webVirDir => "localhost/xyzzyplugh"
   
  
  copyOutputFiles "src/SweetVids.Web/bin/", "*.*", props[:archive]
  copyOutputFiles "src/SweetVids.Web/Views/", "*.*", props[:archive]
  #copyOutputFiles "src/FubuMVC.UI/bin/#{COMPILE_TARGET}", "FubuMVC.UI.{dll,pdb}", props[:archive]
  #copyOutputFiles "src/Spark.Web.FubuMVC/bin/#{COMPILE_TARGET}", "*Spark*.{dll,pdb}", props[:archive]
end

desc "Compiles the SLN file"
task :compileSln do
	MSBuildRunner.compile :compilemode => COMPILE_TARGET, :solutionfile => 'src/SweetVids.sln', :clrversion => CLR_VERSION
  
end

def copyOutputFiles(fromDir, filePattern, outDir)
  Dir.glob(File.join(fromDir, filePattern)){|file| 		
	copy(file, outDir) if File.file?(file)
  } 
end

desc "Runs unit tests"
task :test => [:unit_test, :specs]

desc "Runs unit tests"
task :unit_test => :compile do
  runner = NUnitRunner.new :compilemode => COMPILE_TARGET, :source => 'src', :platform => 'x86'
  runner.executeTests ['SweetVids.Tests']
end

desc "Run the machine.specification tests"
task :specs => :compileSln do
  runner = MSpecRunner.new :compilemode => COMPILE_TARGET, :source => 'src', :options => '--html build\\mspec-test.html'
  runner.executeTests ['SweetVids.MSpec.Tests']
end

desc "Target used for the CI server"
task :ci => [:dbCreate,:dbUpdate,:unit_test,:zip]

desc "ZIPs up the build results"
zip do |zip|
	zip.directories_to_zip = [props[:archive]]
	zip.output_file = 'SweetVids.zip'
	zip.output_path = 'build'
end

#TARANTINO = "lib\\tarantino\\Tarantino.DatabaseManager.Console.exe"
#DBNAME = "KokugenData"
#VERSIONEDDB = "KokugenDataVersioned"
#DBSCRIPTS = "dbChangeScripts"
#DBSERVER = "localhost"

desc "Create the database"
task :dbCreate do
puts "#{TARANTINO} Create #{DBSERVER} #{DBNAME} #{DBSCRIPTS}"

	Tarantino.manageSQLDatabase :dbname => DBNAME, :dbserver => DBSERVER, :action => 'Create', :scriptdir => DBSCRIPTS

end

desc "Drops the database"
task :dbDrop do
	Tarantino.manageSQLDatabase :dbname => DBNAME, :dbserver => DBSERVER, :action => 'Drop', :scriptdir => DBSCRIPTS
end

desc "Resets the database"
task :dbReset => [:compileSln, :dbDrop, :dbCreate] do

   runner = NUnitRunner.new :compilemode => COMPILE_TARGET, :source => 'src', :platform => 'x86', :options => '/include:DBSchema /framework=4.0.30319'
  runner.executeTests ['SweetVids.Tests']
	
end

desc "Creates the Versioned Database"
task :dbCreateVersioned do
	Tarantino.manageSQLDatabase :dbname => DBNAME+'Versioned', :dbserver => DBSERVER, :action => 'Drop', :scriptdir => DBSCRIPTS
	
	Tarantino.manageSQLDatabase :dbname => DBNAME+'Versioned', :dbserver => DBSERVER, :action => 'Create', :scriptdir => DBSCRIPTS
	
end

desc "Creates the Development Database"
task :dbCreateDev do
	Tarantino.manageSQLDatabase :dbname => DBNAME+'Dev', :dbserver => DBSERVER, :action => 'Drop', :scriptdir => DBSCRIPTS
	
	Tarantino.manageSQLDatabase :dbname => DBNAME+'Dev', :dbserver => DBSERVER, :action => 'Create', :scriptdir => DBSCRIPTS
	
	Tarantino.manageSQLDatabase :dbname => DBNAME+'Dev', :dbserver => DBSERVER, :action => 'Update', :scriptdir => DBSCRIPTS
end

desc "Creates the Development Database"
task :dbCreateDesign do
	Tarantino.manageSQLDatabase :dbname => DBNAME+'Dev', :dbserver => VIRDBSERVER, :action => 'Drop', :scriptdir => DBSCRIPTS
	
	Tarantino.manageSQLDatabase :dbname => DBNAME+'Dev', :dbserver => VIRDBSERVER, :action => 'Create', :scriptdir => DBSCRIPTS
	
	Tarantino.manageSQLDatabase :dbname => DBNAME+'Dev', :dbserver => VIRDBSERVER, :action => 'Update', :scriptdir => DBSCRIPTS
end

desc "Update the database"
task :dbUpdateDesign do
Tarantino.manageSQLDatabase :dbname => DBNAME+'Dev', :dbserver => VIRDBSERVER, :action => 'Update', :scriptdir => DBSCRIPTS
end


desc "Update the database"
task :dbUpdateDev do
Tarantino.manageSQLDatabase :dbname => DBNAME+'Dev', :dbserver => DBSERVER, :action => 'Update', :scriptdir => DBSCRIPTS
end

desc "Update the database"
task :dbUpdate do
Tarantino.manageSQLDatabase :dbname => DBNAME, :dbserver => DBSERVER, :action => 'Update', :scriptdir => DBSCRIPTS
end

desc 'Deploy database changes to the database'
task :dbDeploy do
	puts "Database name is #{ENV['dbname']}"
	Tarantino.manageSQLDatabase :dbname => ENV['dbname'], :dbserver => DBSERVER, :action => 'Update', :scriptdir => DBSCRIPTS
end

desc 'Creates the next migration script'
task :dbMigrate => [:dbReset, :dbCreateVersioned] do
	
Tarantino.CompareDatabases :dbname => DBNAME, :dbserver => DBSERVER, :scriptdir => DBSCRIPTS
	
end

