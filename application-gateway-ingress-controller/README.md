## Deploy an AKS cluster with an Application Gateway Ingress Controller (AGIC)
Reference: [Tutorial: Enable the ingress controller add-on for a new AKS cluster with a new application gateway instance](https://learn.microsoft.com/en-us/azure/application-gateway/tutorial-ingress-controller-add-on-new?WT.mc_id=AZ-MVP-5000570)

#### az login, then select subscription
```powershell
az login
az account set --subscription <subscriptionId>
az account show
```

#### Create Resource Group
```powershell
az group create `
    --name rg-aks-appgw-demo-001 `
    --location eastus2
```

#### Create AKS cluster with the  AppGW Ingress addon
```powershell
az aks create `
    --resource-group rg-aks-appgw-demo-001 `
    --node-resource-group rg-mc-aks-appgw-demo-001 <# custom node resource group name #> `
    --name aks-appgw-demo-001 `
    --network-plugin azure <# only Azure CNI supports AGIC #> `
    --enable-managed-identity `
    --enable-addon ingress-appgw <# enable AGIC add-on #> `
    --appgw-name agw-aks-appgw-demo-001 <# name of the Application Gateway #> `
    --appgw-subnet-cidr "10.225.0.0/16" `
    --generate-ssh-keys
```
* `node-resource-group`: I'm using a custom name for the Node Resource Group as a perfonal preference. If you don't specify this, the default name will be used. 
  * Default name: `MC_<resourceGroupName>_<clusterName>_<location>`
* `network-plugin`: The Kubernetes network plugin to use.
  * Accepted values: azure, kubenet, none
  * Only Azure CNI (Container Networking Interface) supports AGIC
* `appgw-subnet-cidr`: The address space must be within the AKS virtual network without overlapping with AKS subnet. Since we haven't defined any particular virtual network to be used, defaults will be used.
  * AKS virtual Network: 10.224.0.0/12
  * AKS subnet: 10.224.0.0/12
  
#### Attach ACR
```powershell
az aks update `
    --resource-group rg-aks-appgw-demo-001 `
    --name aks-appgw-demo-001 `
    --attach-acr "/subscriptions/<subscriptionId>/resourceGroups/rg-ravana-acr/providers/Microsoft.ContainerRegistry/registries/ravana"
```

#### Connect to AKS cluster
```powershell
az aks get-credentials `
    --resource-group rg-aks-appgw-demo-001 `
    --name aks-appgw-demo-001
```

#### Deploy K8s resources
```powershell
# deploy the configuration
kubectl apply -f .\deployment.yaml

# make sure all pods are running
kubectl get pods -o wide

# make sure ingress is created
kubectl get ingress

# troubleshooting. Ingress pod is created in the default namespace
kubectl describe pods -n kube-system -l app=ingress-appgw
```

#### Test
```powershell
$INGRESS_IP=$(k get ingress -o=jsonpath='{.items[0].status.loadBalancer.ingress[0].ip}')
echo $INGRESS_IP
curl http://$INGRESS_IP/api/customers/
curl http://$INGRESS_IP/api/customers/fibonacci?number=5
curl http://$INGRESS_IP/api/orders/
curl http://$INGRESS_IP/api/orders/fibonacci?number=10
```

#### Cleanup resources
```powershell
az group delete --name rg-aks-appgw-demo-001
```