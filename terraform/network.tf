locals {
  vpc_name = "piq-network"
}

resource "yandex_vpc_network" "main" {
  name = local.vpc_name
}

resource "yandex_vpc_subnet" "main" {
  name           = "${local.vpc_name}-subnet"
  zone           = var.default_zone
  network_id     = yandex_vpc_network.main.id
  v4_cidr_blocks = [var.subnet_cidr]
}
