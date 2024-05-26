resource "azurerm_resource_group" "storage" {
  name     = var.resource_group_name
  location = var.resource_group_location
}

resource "azurerm_storage_account" "storage" {
  name                     = var.storage_account_name
  resource_group_name      = azurerm_resource_group.storage.name
  location                 = azurerm_resource_group.storage.location
  account_tier             = var.storage_account_tier
  account_replication_type = var.storage_account_replication_type
}

resource "azurerm_storage_container" "storage" {
  name                     = var.container_name
  storage_account_name     = azurerm_storage_account.storage.name
  container_access_type    = var.container_access_type
}

resource "azurerm_storage_blob" "storage" {
  name                     = var.storage_blob_name
  storage_account_name     = azurerm_storage_account.storage.name
  storage_container_name   = azurerm_storage_container.storage.name
  type                     = var.storage_blob_type
  source                   = var.storage_blob_source
}