# Балансировщик для account-service
resource "yandex_lb_network_load_balancer" "account_nlb" {
  name = "account-load-balancer"

  listener {
    name = "account-listener"
    port = 8080
    external_address_spec {
      ip_version = "ipv4"
    }
  }

  attached_target_group {
    target_group_id = yandex_compute_instance_group.account.load_balancer[0].target_group_id

    healthcheck {
      name = "http-healthcheck"
      http_options {
        port = 8080
        path = "/health"
      }
    }
  }
}

# Балансировщик для piq-service
resource "yandex_lb_network_load_balancer" "piq_nlb" {
  name = "piq-load-balancer"

  listener {
    name = "piq-listener"
    port = 8080
    external_address_spec {
      ip_version = "ipv4"
    }
  }

  attached_target_group {
    target_group_id = yandex_compute_instance_group.piq.load_balancer[0].target_group_id

    healthcheck {
      name = "http-healthcheck"
      http_options {
        port = 8080
        path = "/health"
      }
    }
  }
}