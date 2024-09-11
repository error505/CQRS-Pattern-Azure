// Parameters
param location string = resourceGroup().location
param appServicePlanName string = 'cqrsAppServicePlan'
param apiAppServiceName string = 'cqrsAppServiceAPI'
param eventHubNamespaceName string = 'cqrsEventHubNamespace'
param eventHubName string = 'cqrsEventHub'
param commandDbName string = 'cqrsCommandDB'
param queryDbName string = 'cqrsQueryDB'
param functionAppPlanName string = 'cqrsFunctionAppPlan'
param commandHandlerFunctionName string = 'commandHandlerFunction'
param updateProcessorFunctionName string = 'updateProcessorFunction'
param queryHandlerFunctionName string = 'queryHandlerFunction'
param appInsightsName string = 'cqrsAppInsights'

// Azure App Service Plan
resource appServicePlan 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: appServicePlanName
  location: location
  sku: {
    name: 'S1'
    tier: 'Standard'
  }
}

// Azure App Service API
resource apiAppService 'Microsoft.Web/sites@2022-03-01' = {
  name: apiAppServiceName
  location: location
  kind: 'app'
  properties: {
    serverFarmId: appServicePlan.id
    siteConfig: {
      appSettings: [
        {
          name: 'FUNCTIONS_WORKER_RUNTIME'
          value: 'dotnet'
        }
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: appInsights.properties.InstrumentationKey
        }
        {
          name: 'EventHubConnectionString'
          value: listKeys(resourceId('Microsoft.EventHub/namespaces/authorizationRules', eventHubNamespace.name, 'RootManageSharedAccessKey'), '2021-06-01').primaryConnectionString
        }
      ]
    }
  }
}

// Azure Event Hub Namespace
resource eventHubNamespace 'Microsoft.EventHub/namespaces@2021-06-01' = {
  name: eventHubNamespaceName
  location: location
  sku: {
    name: 'Standard'
    tier: 'Standard'
  }
}

// Azure Event Hub
resource eventHub 'Microsoft.EventHub/namespaces/eventhubs@2021-06-01' = {
  parent: eventHubNamespace
  name: eventHubName
}

// Azure Cosmos DB Account for Command DB
resource commandDb 'Microsoft.DocumentDB/databaseAccounts@2023-05-15' = {
  name: commandDbName
  location: location
  kind: 'GlobalDocumentDB'
  properties: {
    databaseAccountOfferType: 'Standard'
    locations: [
      {
        locationName: location
        failoverPriority: 0
      }
    ]
    consistencyPolicy: {
      defaultConsistencyLevel: 'Session'
    }
  }
}

// Azure Cosmos DB Account for Query DB
resource queryDb 'Microsoft.DocumentDB/databaseAccounts@2023-05-15' = {
  name: queryDbName
  location: location
  kind: 'GlobalDocumentDB'
  properties: {
    databaseAccountOfferType: 'Standard'
    locations: [
      {
        locationName: location
        failoverPriority: 0
      }
    ]
    consistencyPolicy: {
      defaultConsistencyLevel: 'Session'
    }
  }
}

// Application Insights
resource appInsights 'Microsoft.Insights/components@2022-07-01' = {
  name: appInsightsName
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
    RetentionInDays: 30
  }
}

// Azure Function App Plan
resource functionAppPlan 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: functionAppPlanName
  location: location
  sku: {
    name: 'Y1'
    tier: 'Dynamic'
  }
}

// Command Handler Function App
resource commandHandlerFunctionApp 'Microsoft.Web/sites@2022-03-01' = {
  name: commandHandlerFunctionName
  location: location
  kind: 'functionapp'
  properties: {
    serverFarmId: functionAppPlan.id
    siteConfig: {
      appSettings: [
        {
          name: 'FUNCTIONS_WORKER_RUNTIME'
          value: 'dotnet'
        }
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: appInsights.properties.InstrumentationKey
        }
        {
          name: 'EventHubConnectionString'
          value: listKeys(resourceId('Microsoft.EventHub/namespaces/authorizationRules', eventHubNamespace.name, 'RootManageSharedAccessKey'), '2021-06-01').primaryConnectionString
        }
        {
          name: 'CosmosDBConnectionString'
          value: commandDb.listKeys().primaryMasterKey
        }
      ]
    }
  }
}

// Update Processor Function App
resource updateProcessorFunctionApp 'Microsoft.Web/sites@2022-03-01' = {
  name: updateProcessorFunctionName
  location: location
  kind: 'functionapp'
  properties: {
    serverFarmId: functionAppPlan.id
    siteConfig: {
      appSettings: [
        {
          name: 'FUNCTIONS_WORKER_RUNTIME'
          value: 'dotnet'
        }
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: appInsights.properties.InstrumentationKey
        }
        {
          name: 'EventHubConnectionString'
          value: listKeys(resourceId('Microsoft.EventHub/namespaces/authorizationRules', eventHubNamespace.name, 'RootManageSharedAccessKey'), '2021-06-01').primaryConnectionString
        }
        {
          name: 'CosmosDBConnectionString'
          value: queryDb.listKeys().primaryMasterKey
        }
      ]
    }
  }
}

// Query Handler Function App
resource queryHandlerFunctionApp 'Microsoft.Web/sites@2022-03-01' = {
  name: queryHandlerFunctionName
  location: location
  kind: 'functionapp'
  properties: {
    serverFarmId: functionAppPlan.id
    siteConfig: {
      appSettings: [
        {
          name: 'FUNCTIONS_WORKER_RUNTIME'
          value: 'dotnet'
        }
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: appInsights.properties.InstrumentationKey
        }
        {
          name: 'CosmosDBConnectionString'
          value: queryDb.listKeys().primaryMasterKey
        }
      ]
    }
  }
}
