variable "db_cluster_name" { default = "piq-mysql" }
variable "db_version" { default = "8.0" }
variable "db_disk_size" { default = 15 }

variable "db_piq_name" { default = "piq" }
variable "db_accounts_name" { default = "accounts" }

resource "yandex_mdb_mysql_cluster" "db" {
  name        = var.db_cluster_name
  environment = "PRESTABLE"
  network_id  = yandex_vpc_network.main.id
  version     = var.db_version

  resources {
    resource_preset_id = "s2.micro"
    disk_type_id       = "network-hdd"
    disk_size          = var.db_disk_size
  }

  database {
    name = var.db_piq_name
  }

  database {
    name = var.db_accounts_name
  }

  user {
    name     = var.db_user
    password = var.db_password
    permission {
      database_name = var.db_piq_name
      roles         = ["ALL"]
    }
    permission {
      database_name = var.db_accounts_name
      roles         = ["ALL"]
    }
  }

  host {
    zone      = var.default_zone
    subnet_id = yandex_vpc_subnet.main.id
  }
}

output "mysql_host" {
  value = yandex_mdb_mysql_cluster.db.host[0].fqdn
}

output "mysql_databases" {
  value = [
    for db in yandex_mdb_mysql_cluster.db.database : db.name
  ]
}