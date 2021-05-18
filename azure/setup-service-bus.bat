az account set --subscription "172d74c0-182e-4951-93fa-c451f1c945e5"

rem az group delete -g ursatile-workshops-resource-group --yes

az servicebus namespace delete --resource-group ursatile-workshops-resource-group --name autobarn-namespace

az servicebus namespace create --resource-group ursatile-workshops-resource-group --name autobarn-namespace
az servicebus topic create --resource-group ursatile-workshops-resource-group --namespace-name autobarn-namespace --name autobarn-new-vehicle-topic
az servicebus topic subscription create --resource-group ursatile-workshops-resource-group --namespace-name autobarn-namespace --topic-name autobarn-new-vehicle-topic --name autobarn-auditlog-subscription
az servicebus topic subscription create --resource-group ursatile-workshops-resource-group --namespace-name autobarn-namespace --topic-name autobarn-new-vehicle-topic --name autobarn-notifier-subscription


