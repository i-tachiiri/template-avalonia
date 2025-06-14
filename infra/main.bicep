param location string = resourceGroup().location
param prefix string

module common 'common/main.bicep' = {
  name: 'common'
  params: {
    location: location
    prefix: prefix
  }
}

module backup 'svc-backup/main.bicep' = {
  name: 'backup'
  params: {
    location: location
    prefix: prefix
  }
}
