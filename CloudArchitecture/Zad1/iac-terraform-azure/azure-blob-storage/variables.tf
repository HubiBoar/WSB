variable "resource_group_name" {
  type = string
  default = "storage-resources"
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

variable "container_name" {
  type = string
  default = "container"
}

variable "container_access_type" {
  type = string
  default = "private"
}


variable "storage_blob_name" {
  type = string
  default = "private"
}

variable "storage_blob_type" {
  type = string
  default = "Block"
}

variable "storage_blob_source" {
  type = string
  default = "some-local-file.zip"
}