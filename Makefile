GIT_SHORT_REV := $(shell git rev-parse --short HEAD)

clean:
	dotnet clean

build:
	dotnet restore
	dotnet build

watch:
	dotnet watch

publish:
	dotnet publish -c Release --version-suffix $(GIT_SHORT_REV)
