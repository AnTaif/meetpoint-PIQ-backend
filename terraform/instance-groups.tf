locals {
  vm-docker-image-id = "fd87vmd9vdkri77eb4n6" # ID образа с Docker
}

resource "tls_private_key" "ssh_key" {
  algorithm = "RSA"
  rsa_bits  = 4096
}

resource "local_file" "ssh_private_key" {
  filename = "${path.module}/id_rsa"
  content  = tls_private_key.ssh_key.private_key_openssh
}

resource "local_file" "ssh_public_key" {
  filename = "${path.module}/id_rsa.pub"
  content  = tls_private_key.ssh_key.public_key_openssh
}

resource "yandex_compute_instance_group" "account" {
  name               = "account-microservices"
  description        = "account microservices"
  service_account_id = yandex_iam_service_account.piq-sa.id
  deletion_protection = false

  load_balancer {
    target_group_name        = "account-target-group"
    target_group_description = "target group for account microservices"
  }

  instance_template {
    platform_id = "standard-v3"
    resources {
      memory = 2
      cores  = 2
      core_fraction = 20
    }

    boot_disk {
      initialize_params {
        image_id = local.vm-docker-image-id
        size     = 15
      }
    }

    network_interface {
      network_id = yandex_vpc_network.main.id
      subnet_ids = [yandex_vpc_subnet.main.id]
      nat = true
    }
    

    metadata = {
      ssh-keys = "ubuntu:${tls_private_key.ssh_key.public_key_openssh}"

      user-data = <<-EOF
      #cloud-config
      packages:
        - docker.io
        - docker-compose
  
      write_files:
        - path: /home/ubuntu/docker-compose.yml
          content: |
            services:
              account-service:
                image: ${var.account_service_image}
                environment:
                  DB_CONTAINER: ${yandex_mdb_mysql_cluster.db.host[0].fqdn}
                  DATABASE_USER: ${var.db_user}
                  DATABASE_PASSWORD: ${var.db_password}
                  JWT_SECRET: ${var.jwt_secret}
                  S3_ACCESS_KEY_ID: ${yandex_storage_bucket.piq-avatars.access_key}
                  S3_SECRET_TOKEN: ${yandex_storage_bucket.piq-avatars.secret_key}
                restart: always
                ports:
                  - "8080:8080"
      
      runcmd:
        - sudo rm /home/ubuntu/.docker/config.json
        - sudo rm /root/.docker/config.json
        - mkdir /home/ubuntu/.docker
        - echo '${jsonencode({
            "auths" : {
              "cr.yandex" : {
                "auth" : "${base64encode("${yandex_iam_service_account_static_access_key.piq-sa-keys.access_key}:${yandex_iam_service_account_static_access_key.piq-sa-keys.secret_key}")}"
              }
            }
          })}' > /home/ubuntu/.docker/config.json
        - cd /home/ubuntu/
        - docker-compose up -d
      EOF
    }
  }

  scale_policy {
    auto_scale {
      initial_size           = 1
      max_size               = 2
      min_zone_size          = 1
      measurement_duration   = 60
      warmup_duration        = 60
      stabilization_duration = 120

      custom_rule {
        rule_type   = "WORKLOAD"
        metric_type = "GAUGE"
        metric_name = "app_requests_per_second"
        target      = 40 # Если >40 RPS
      }
    }
  }

  allocation_policy {
    zones = ["ru-central1-a"]
  }

  deploy_policy {
    max_unavailable = 1
    max_expansion   = 0
  }
}

resource "yandex_compute_instance_group" "piq" {
  name               = "piq-microservices"
  description        = "piq microservices"
  service_account_id = yandex_iam_service_account.piq-sa.id
  deletion_protection = false

  load_balancer {
    target_group_name        = "piq-target-group"
    target_group_description = "Target group for piq microservices"
  }

  instance_template {
    platform_id = "standard-v3"
    resources {
      memory = 2
      cores  = 2
      core_fraction = 20
    }

    boot_disk {
      initialize_params {
        image_id = local.vm-docker-image-id
        size     = 15
      }
    }

    network_interface {
      network_id = yandex_vpc_network.main.id
      subnet_ids = [yandex_vpc_subnet.main.id]
      nat = true
    }

    metadata = {
      ssh-keys = "ubuntu:${tls_private_key.ssh_key.public_key_openssh}"

      user-data = <<-EOF
      #cloud-config
      packages:
        - docker.io
        - docker-compose
  
      write_files:
        - path: /home/ubuntu/docker-compose.yml
          content: |
            services:
              piq-service:
                image: ${var.piq_service_image}
                environment:
                  DB_CONTAINER: ${yandex_mdb_mysql_cluster.db.host[0].fqdn}
                  DATABASE_USER: ${var.db_user}
                  DATABASE_PASSWORD: ${var.db_password}
                  JWT_SECRET: ${var.jwt_secret}
                  S3_ACCESS_KEY_ID: ${yandex_storage_bucket.piq-avatars.access_key}
                  S3_SECRET_TOKEN: ${yandex_storage_bucket.piq-avatars.secret_key}
                restart: always
                ports:
                  - "8080:8080"
      
      runcmd:
        - sudo rm /home/ubuntu/.docker/config.json
        - sudo rm /root/.docker/config.json
        - mkdir /home/ubuntu/.docker
        - echo '${jsonencode({
            "auths" : {
              "cr.yandex" : {
                "auth" : "${base64encode("${yandex_iam_service_account_static_access_key.piq-sa-keys.access_key}:${yandex_iam_service_account_static_access_key.piq-sa-keys.secret_key}")}"
              }
            }
          })}' > /home/ubuntu/.docker/config.json
        - cd /home/ubuntu/
        - docker-compose up -d
      EOF
    }
  }

  scale_policy {
    auto_scale {
      initial_size           = 1
      max_size               = 3
      min_zone_size          = 1
      measurement_duration   = 60
      warmup_duration        = 60
      stabilization_duration = 120

      custom_rule {
        rule_type   = "WORKLOAD"
        metric_type = "GAUGE"
        metric_name = "app_requests_per_second"
        target      = 40 # Если >40 RPS
      }
    }
  }

  allocation_policy {
    zones = [var.default_zone]
  }

  deploy_policy {
    max_unavailable = 1
    max_expansion   = 0
  }
}
