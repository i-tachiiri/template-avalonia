param location string = resourceGroup().location
param prefix string
param sqlAdminUsername string
@secure()
param sqlAdminPassword string

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
    sqlAdminUsername: sqlAdminUsername
    sqlAdminPassword: sqlAdminPassword
  }
}
