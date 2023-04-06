# Setup
```powershell
cd "D:\My GitHub\Jaliya Udagedara\aks-examples\hello-world"
```

# Docker
```powershell
docker build . `
	-f .\hello-world\Dockerfile `
	-t ravana.azurecr.io/aspnetcore/hello-world/api:latest 

docker push ravana.azurecr.io/aspnetcore/hello-world/api:latest
```