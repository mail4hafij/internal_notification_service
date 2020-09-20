# ---------------- build code --------------------
FROM microsoft/aspnetcore-build:2.0 AS build-env
WORKDIR /app/src

# copy csproj and restore as distinct layers
COPY ./src/src.csproj .
RUN dotnet restore

# copy everything else and build
COPY ./src/ .
RUN dotnet publish -c Release -o out
# ---------------- /build code --------------------




# ------------- create final image for deployment -----------------------
FROM microsoft/aspnetcore:2.0 as runtime-image
WORKDIR /app

# copy just the build and tested dll
COPY --from=build-env /app/src/out/ ./src/out/
ENTRYPOINT ["dotnet", "./src/out/src.dll"]
# ------------- /create final image for deployment -----------------------