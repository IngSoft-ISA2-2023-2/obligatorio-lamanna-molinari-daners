cd "Backend Release"
start "Backend" cmd /c "PharmaGo.WebApi.exe"
cd ../../Codigo/Frontend
start "Frontend Dev enviroment" cmd /c "ng serve & exit"
timeout /t -1
selenium-side-runner tests/PharmaGo.Selenium.side