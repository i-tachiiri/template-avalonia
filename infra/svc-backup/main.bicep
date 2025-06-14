param location string
param prefix string
param sqlAdminUsername string
@secure()
param sqlAdminPassword string

resource storage 'Microsoft.Storage/storageAccounts@2023-01-01' = {
  name: '${prefix}storage'
  location: location
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
}

resource sqlServer 'Microsoft.Sql/servers@2022-05-01-preview' = {
  name: '${prefix}-sql'
  location: location
  properties: {
    administratorLogin: sqlAdminUsername
    administratorLoginPassword: sqlAdminPassword
    version: '12'
  }
}

resource sqlDb 'Microsoft.Sql/servers/databases@2022-05-01-preview' = {
  name: '${sqlServer.name}/appdb'
  sku: {
    name: 'GP_S_Gen5_1'
    tier: 'GeneralPurpose'
    capacity: 1
    family: 'Gen5'
  }
  properties: {
    autoPauseDelay: 60
    minCapacity: 0.5
    maxSizeBytes: 5368709120
  }
}

resource functionapp 'Microsoft.Web/sites@2023-01-01' = {
  name: '${prefix}-func'
  location: location
  kind: 'functionapp'
  properties: {
    serverFarmId: ''
  }
}
