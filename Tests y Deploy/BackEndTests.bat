cd ..
cd "Codigo/Backend"
dotnet build --configuration Release --no-restore PharmaGo.sln
start "Unit Tests" cmd /c "dotnet test PharnaGo.Test/PharmaGo.Test.csproj & pause"
start "SpecFlow Tests" cmd /c "dotnet test PharmaGo.SpecFlow/PharmaGo.SpecFlow.csproj & pause"