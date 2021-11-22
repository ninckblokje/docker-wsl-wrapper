clean:
	dotnet clean

build:
	dotnet restore
	dotnet build

watch:
	dotnet watch

publish:
	dotnet publish -c Release
