#!/bin/bash
# for stuff like: dotnet ef database update && dotnet run
dotnet run
exec "$@"