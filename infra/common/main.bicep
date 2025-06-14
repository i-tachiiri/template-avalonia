param location string
param prefix string
resource log 'Microsoft.OperationalInsights/workspaces@2021-06-01' = {
  name: '${prefix}-log'
  location: location
  properties: {
    sku: {
      name: 'PerGB2018'
    }
    retentionInDays: 30
  }
}
