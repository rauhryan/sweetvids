require 'erb'

class NUnitRunner
	include FileTest
	
	def initialize(paths)
		@sourceDir = paths.fetch(:source, 'source')
		@resultsDir = paths.fetch(:results, 'results')
		@compilePlatform = paths.fetch(:platform, '')
		@compileTarget = paths.fetch(:compilemode, 'debug')		
		@options = paths.fetch(:options, '/framework=4.0.30319')
	

			if ENV["teamcity.dotnet.nunitlauncher"] # check if we are running in TeamCity
				# We are not using the TeamCity nunit launcher. We use NUnit with the TeamCity NUnit Addin which needs tO be copied to our NUnit addins folder
				# http://blogs.jetbrains.com/teamcity/2008/07/28/unfolding-teamcity-addin-for-nunit-secrets/
				# The teamcity.dotnet.nunitaddin environment variable is not available until TeamCity 4.0, so we hardcode it for now
				puts "Add In Path is "
				puts ENV["teamcity.dotnet.nunitaddin"] ? ENV["teamcity.dotnet.nunitaddin"] : 'NOT FOUND'
				#@teamCityAddinPath = ENV["teamcity.dotnet.nunitaddin"] ? ENV["teamcity.dotnet.nunitaddin"] : 'c:/buildAgent/plugins/dotnetPlugin/bin/JetBrains.TeamCity.NUnitAddin-NUnit'
				@teamCityAddinPath = 'c:/buildAgent/plugins/dotnetPlugin/bin/JetBrains.TeamCity.NUnitAddin-NUnit'
				cp @teamCityAddinPath + '-2.5.4.dll', 'lib/nunit/addins/'
			end
		
		@nunitExe = File.join('lib', 'nunit', "nunit-console#{(@compilePlatform.empty? ? '' : "-#{@compilePlatform}")}.exe").gsub('/','\\') + ' /nothread'
	end
	
	def executeTests(assemblies)
		Dir.mkdir @resultsDir unless exists?(@resultsDir)
		
		assemblies.each do |assem|
			file = File.expand_path("#{@sourceDir}/#{assem}/bin/#{@compileTarget}/#{assem}.dll")
			puts "#{@nunitExe} #{@options} \"#{file}\""
			sh "#{@nunitExe} #{@options} \"#{file}\""
		end
	end
	
	def getExecutable(assemblies)
		files = "";
		assemblies.each do |assem|
			file = File.expand_path("#{@sourceDir}/#{assem}/bin/#{@compileTarget}/#{assem}.dll")
			files += file + " "
		end
		
		file = files.chop
		
		@executable = "#{@nunitExe} #{@options} \"#{file}\""
		return @executable
	end
	
	def get_command_line
		return @executable
	end
	
	
end

class NCoverRunner
	include FileTest
	
	def initialize(paths)
		@resultsDir = paths.fetch(:results, 'coverage')
		@nunitCommand = paths.fetch(:nunit, '')
	
		@ncoverExe = File.join('lib', 'ncover', "NCover.Console.exe")
	end
	
	def cover(assemblies)
		Dir.mkdir @resultsDir unless exists?(@resultsDir)
		output = "//x #{@resultsDir}/coverage.xml"
		
		includes = "//a "
		assemblies.each do |assem|
			includes += assem + ";"
		end
		
		assemToCover = includes.chop
		sh "regsvr32 lib/ncover/CoverLib.dll /s"
		sh "#{@ncoverExe} #{output} #{assemToCover} #{@nunitCommand}"
		sh "regsvr32 lib/ncover/CoverLib.dll /u /s"
	end
end

class NCoverExplorer
	
	def initialize(paths)
		@resultsDir = paths.fetch(:results, 'coverage')
		@coverageFile = paths.fetch(:coverage, 'coverage.xml')
		@projectName = paths.fetch(:project, 'Unkown')
		@minAccept = paths.fetch(:minimumLevel, '80')
		
		@ncoverExplorer = File.join('lib', 'ncoverexplorer', 'NCoverExplorer.Console.exe')
		
	
	end
	
	def generateReports()
	
		sh "#{@ncoverExplorer} #{@resultsDir}/#{@coverageFile} /r:ModuleClassSummary /h:#{@resultsDir}/CoverageReport.html /p:#{@projectName} /so:Name"
	
	end
end

class MSBuildRunner
	def self.compile(attributes)
		version = attributes.fetch(:clrversion, 'v4.0.30319')
		compileTarget = attributes.fetch(:compilemode, 'debug')
	    solutionFile = attributes[:solutionfile]
		
		frameworkDir = File.join(ENV['windir'].dup, 'Microsoft.NET', 'Framework', version)
		msbuildFile = File.join(frameworkDir, 'msbuild.exe')
		
		sh "#{msbuildFile} #{solutionFile} /nologo /maxcpucount /v:m /property:BuildInParallel=false /property:Configuration=#{compileTarget} /t:Rebuild"
	end
end

class AspNetCompilerRunner
	def self.compile(attributes)
		
		webPhysDir = attributes.fetch(:webPhysDir, '')
		webVirDir = attributes.fetch(:webVirDir, 'This_Value_Is_Not_Used')
		
		frameworkDir = File.join(ENV['windir'].dup, 'Microsoft.NET', 'Framework', 'v4.0.30319')
		aspNetCompiler = File.join(frameworkDir, 'aspnet_compiler.exe')

		sh "#{aspNetCompiler} -nologo -errorstack -c -p #{webPhysDir} -v #{webVirDir}"
	end
end

class AsmInfoBuilder
	attr_reader :buildnumber

	def initialize(baseVersion, properties)
		@properties = properties;
		
		@buildnumber = baseVersion + (ENV["CCNetLabel"].nil? ? '0' : ENV["CCNetLabel"].to_s)
		@properties['Version'] = @properties['InformationalVersion'] = buildnumber;
	end


	
	def write(file)
		template = %q{
using System;
using System.Reflection;
using System.Runtime.InteropServices;

<% @properties.each {|k, v| %>
[assembly: Assembly<%=k%>Attribute("<%=v%>")]
<% } %>
		}.gsub(/^    /, '')
		  
	  erb = ERB.new(template, 0, "%<>")
	  
	  File.open(file, 'w') do |file|
		  file.puts erb.result(binding) 
	  end
	end
end
