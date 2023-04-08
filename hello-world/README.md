# Setup
```powershell
cd "D:\My GitHub\Jaliya Udagedara\aks-examples\hello-world"
```

# Docker
```powershell
docker build . `
	-t ravana.azurecr.io/aspnetcore/hello-world/api:latest 

docker push ravana.azurecr.io/aspnetcore/hello-world/api:latest
```