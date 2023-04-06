# ReadMe

## Current DIR
```
k8s-storage-demo\azure-file-share
```

## Create Application Image
```
# Build image
docker build . `
	-t myacr.azurecr.io/demo/azure-file-share/api:dev `
	-f .\KubeStorage.AzFileShare.Api\Dockerfile

# Push image
docker push myacr.azurecr.io/demo/azure-file-share/api:dev
```

## Create Deployment to AKS
```
# Create deployment
kubectl apply -f .\k8s\deployment.yml

# Delete deployment
kubectl delete -f .\k8s\deployment.yml
```