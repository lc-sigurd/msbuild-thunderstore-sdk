.PHONY: default debug debug-tasks release release-tasks release-sdk

# the first target is run by default, so make an empty target
default: ;

debug:
	$(MAKE) debug-tasks
	$(MAKE) release-sdk

release:
	$(MAKE) release-tasks
	$(MAKE) release-sdk

debug-tasks:
	dotnet build -c DebugTasks

release-tasks:
	dotnet build -c ReleaseTasks

release-sdk:
	dotnet pack -c ReleaseSdk
