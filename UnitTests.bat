cd "Codigo/Backend"
dotnet build --configuration Release --no-restore PharmaGo.sln
dotnet test PharnaGo.Test/PharmaGo.Test.csproj
pauses