start powershell -noexit -command "dotnet restore; dotnet run"
cd frontend
start powershell -noexit -command "npm install; npm run dev"
echo "Opened"