resource "local_file" "foo" {
  content  = "print(\"Hello World\")"
  filename = "${var.file_name}.py"
}

terraform {
  backend "local" {
    path = "./terraform.tfstate"
  }
}