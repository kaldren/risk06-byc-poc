resource storageaccount 'Microsoft.Storage/storageAccounts@2021-02-01' = {
  name: 'storageDeploy'
  location: location
  kind: 'StorageV2'
  sku: {
    name: 'Premium_LRS'
  }
}
