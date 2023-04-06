# ReadMe

## Current DIR
```
k8s-storage-demo\mongodb
```

## Create Application Image
```
# Build image
docker build . `
	-t myacr.azurecr.io/demo/mongodb/api:dev `
	-f .\KubeStorage.Mongo.Api\Dockerfile

# Push image
docker push myacr.azurecr.io/demo/mongodb/api:dev
```

## Create Deployment to AKS
```
# Create deployment
kubectl apply -f .\k8s\deployment.yml

# Delete deployment
kubectl delete -f .\k8s\deployment.yml
```

## Setup ReplicaSet
Shell In
```
kubectl exec --namespace demo-mongodb mongo-0 --stdin --tty  -- mongosh
```

Run
```
rs.initiate()
var cfg = rs.conf()
cfg.members[0].host="mongo-0.mongodb:27017"
rs.reconfig(cfg)
rs.add("mongo-1.mongodb:27017")
rs.add("mongo-2.mongodb:27017")
rs.status()
```