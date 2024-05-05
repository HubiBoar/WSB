resource "azurerm_resource_group" "function" {
  name     = var.resource_group_name
  location = var.resource_group_location
}

resource "azurerm_storage_account" "function" {
  name                     = var.storage_account_name
  resource_group_name      = azurerm_resource_group.function.name
  location                 = azurerm_resource_group.function.location
  account_tier             = var.storage_account_tier
  account_replication_type = var.storage_account_replication_type
}

resource "azurerm_service_plan" "function" {
  name                = var.service_plan_name
  resource_group_name = azurerm_resource_group.function.name
  location            = azurerm_resource_group.function.location
  os_type             = var.service_plan_os_type
  sku_name            = var.service_plan_sku_name
}

resource "azurerm_linux_function_app" "function" {
  name                = var.function_app_name
  resource_group_name = azurerm_resource_group.function.name
  location            = azurerm_resource_group.function.location

  storage_account_name       = azurerm_storage_account.function.name
  storage_account_access_key = azurerm_storage_account.function.primary_access_key
  service_plan_id            = azurerm_service_plan.function.id

  site_config {}
}


resource "azurerm_function_app_function" "function" {
  name            = var.function_app_function_name
  function_app_id = azurerm_linux_function_app.function.id
  language        = var.function_app_function_language
  test_data = jsonencode({
    "name" = "Azure"
  })
  config_json = jsonencode({
    "bindings" = [
      {
        "authLevel" = "function"
        "direction" = "in"
        "methods" = [
          "get",
          "post",
        ]
        "name" = "req"
        "type" = "httpTrigger"
      },
      {
        "direction" = "out"
        "name"      = "$return"
        "type"      = "http"
      },
    ]
  })
}