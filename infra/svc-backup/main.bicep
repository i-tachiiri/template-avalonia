param location string
param prefix string

resource storage 'Microsoft.Storage/storageAccounts@2023-01-01' = {
  name: '${prefix}storage'
  location: location
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
}

resource sql 'Microsoft.DBforPostgreSQL/flexibleServers@2022-01-20-preview' = {
  name: '${prefix}-sql'
  location: location
  sku: {
    name: 'Standard_B0ms'
    tier: 'Burstable'
  }
  properties: {
    administratorLogin: 'sqladmin'
    administratorLoginPassword: 'REPLACE_ME'
    version: '12'
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
