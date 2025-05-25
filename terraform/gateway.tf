# Локальные значения для получения адресов балансировщиков
locals {
  account_nlb_address = one([
    for listener in yandex_lb_network_load_balancer.account_nlb.listener :
    one(listener.external_address_spec[*].address)
  ])

  piq_nlb_address = one([
    for listener in yandex_lb_network_load_balancer.piq_nlb.listener :
    one(listener.external_address_spec[*].address)
  ])
}

resource "yandex_api_gateway" "api_gateway" {
  name              = "service-gateway"
  description       = "Api-Gateway for PIQ-service"
  execution_timeout = 10

  spec = <<-EOT
    openapi: "3.0.0"
    info:
      title: "Microservices Gateway"
      version: "1.0"
    paths:
      /auth/{path+}:
        x-yc-apigateway-any-method:
          parameters:
          - name: path
            in: path
            required: false
          x-yc-apigateway-integration:
            type: "http"
            url: "http://${local.account_nlb_address}:8080/auth/{path}"
            method: ANY
            headers:
              Host: "account-service"

      /users/{path+}:
        x-yc-apigateway-any-method:
          parameters:
          - name: path
            in: path
            required: false
          x-yc-apigateway-integration:
            type: "http"
            url: "http://${local.account_nlb_address}:8080/users/{path}"
            method: ANY
            headers:
              Host: "account-service"

      /assessments/{path+}:
        x-yc-apigateway-any-method:
          parameters:
          - name: path
            in: path
            required: false
          x-yc-apigateway-integration:
            type: "http"
            url: "http://${local.piq_nlb_address}:8080/assessments/{path}"
            method: ANY
            headers:
              Host: "piq-service"

      /events/{path+}:
        x-yc-apigateway-any-method:
          parameters:
          - name: path
            in: path
            required: false
          x-yc-apigateway-integration:
            type: "http"
            url: "http://${local.piq_nlb_address}:8080/events/{path}"
            method: ANY
            headers:
              Host: "piq-service"

      /teams/{path+}:
        x-yc-apigateway-any-method:
          parameters:
          - name: path
            in: path
            required: false
          x-yc-apigateway-integration:
            type: "http"
            url: "http://${local.piq_nlb_address}:8080/teams/{path}"
            method: ANY
            headers:
              Host: "piq-service"
  EOT
}
