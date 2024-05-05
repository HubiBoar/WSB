variable "resource_group_name" {
  type = string
  default = "function-resources"
}

variable "resource_group_location" {
  type = string
  default = "West Europe"
}

variable "storage_account_name" {
  type = string
  default = "storage"
}

variable "storage_account_tier" {
  type = string
  default = "Standard"
}

variable "storage_account_replication_type" {
  type = string
  default = "GRS"
}

variable "service_plan_name" {
  type = string
  default = "service-plan"
}

variable "service_plan_os_type" {
  type = string
  default = "Linux"
}

variable "service_plan_sku_name" {
  type = string
  default = "P1v2"
}


variable "function_app_name" {
  type = string
  default = "function-app"
}

variable "function_app_function_name" {
  type = string
  default = "function"
}

variable "function_app_function_language" {
  type = string
  default = "Python"
}