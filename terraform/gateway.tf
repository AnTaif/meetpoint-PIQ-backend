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
      # AccountService endpoints
      /auth/login:
        post:
          x-yc-apigateway-integration:
            type: "http"
            url: "http://${local.account_nlb_address}:8080/auth/login"
            headers:
              Host: "account-service"
              Content-Type: application/json
              Authorization: $request.header.Authorization
      
      /users/current:
        get:
          x-yc-apigateway-integration:
            type: "http"
            url: "http://${local.account_nlb_address}:8080/users/current"
            headers:
              Host: "account-service"
              Authorization: Bearer $request.header.Authorization
      
      /users/upload-avatar:
        post:
          x-yc-apigateway-integration:
            type: "http"
            url: "http://${local.account_nlb_address}:8080/users/upload-avatar"
            headers:
              Host: "account-service"
              Content-Type: application/json
              Authorization: $request.header.Authorization
      
      /users/delete-avatar:
        delete:
          x-yc-apigateway-integration:
            type: "http"
            url: "http://${local.account_nlb_address}:8080/users/delete-avatar"
            headers:
              Host: "account-service"
              Content-Type: application/json
              Authorization: $request.header.Authorization

      # PIQService endpoints - Assessments
      /assessments/{id}:
        put:
          parameters:
          - name: id
            in: path
            required: true
          x-yc-apigateway-integration:
            type: "http"
            url: "http://${local.piq_nlb_address}:8080/assessments/{id}"
            headers:
              Host: "piq-service"
              Content-Type: application/json
              Authorization: $request.header.Authorization
        delete:
          parameters:
          - name: id
            in: path
            required: true
          x-yc-apigateway-integration:
            type: "http"
            url: "http://${local.piq_nlb_address}:8080/assessments/{id}"
            headers:
              Host: "piq-service"
              Content-Type: application/json
              Authorization: $request.header.Authorization
      
      /assessments/{id}/used-forms:
        get:
          parameters:
          - name: id
            in: path
            required: true
          x-yc-apigateway-integration:
            type: "http"
            url: "http://${local.piq_nlb_address}:8080/assessments/{id}/used-forms"
            headers:
              Host: "piq-service"
              Content-Type: application/json
              Authorization: $request.header.Authorization
      
      /assessments/{assessmentId}/assess-users:
        get:
          parameters:
          - name: assessmentId
            in: path
            required: true
          x-yc-apigateway-integration:
            type: "http"
            url: "http://${local.piq_nlb_address}:8080/assessments/{assessmentId}/assess-users"
            headers:
              Host: "piq-service"
              Content-Type: application/json
              Authorization: $request.header.Authorization
      
      /assessments/{assessmentId}/assess-users/{assessedUserId}/choices:
        get:
          parameters:
          - name: assessmentId
            in: path
            required: true
          - name: assessedUserId
            in: path
            required: true
          x-yc-apigateway-integration:
            type: "http"
            url: "http://${local.piq_nlb_address}:8080/assessments/{assessmentId}/assess-users/{assessedUserId}/choices"
            headers:
              Host: "piq-service"
              Content-Type: application/json
              Authorization: $request.header.Authorization
      
      /assessments/{assessmentId}/assess-users/{assessedUserId}/assess:
        post:
          parameters:
          - name: assessmentId
            in: path
            required: true
          - name: assessedUserId
            in: path
            required: true
          x-yc-apigateway-integration:
            type: "http"
            url: "http://${local.piq_nlb_address}:8080/assessments/{assessmentId}/assess-users/{assessedUserId}/assess"
            headers:
              Host: "piq-service"
              Content-Type: application/json
              Authorization: $request.header.Authorization

      # PIQService endpoints - Events
      /events/current:
        get:
          x-yc-apigateway-integration:
            type: "http"
            url: "http://${local.piq_nlb_address}:8080/events/current"
            headers:
              Host: "piq-service"
              Content-Type: application/json
              Authorization: $request.header.Authorization
      
      /events/assessments:
        post:
          x-yc-apigateway-integration:
            type: "http"
            url: "http://${local.piq_nlb_address}:8080/events/assessments"
            headers:
              Host: "piq-service"
              Content-Type: application/json
              Authorization: $request.header.Authorization

      # PIQService endpoints - Teams
      /teams/{teamId}/assessments:
        get:
          parameters:
          - name: teamId
            in: path
            required: true
          x-yc-apigateway-integration:
            type: "http"
            url: "http://${local.piq_nlb_address}:8080/teams/{teamId}/assessments"
            headers:
              Host: "piq-service"
              Content-Type: application/json
              Authorization: $request.header.Authorization
        post:
          parameters:
          - name: teamId
            in: path
            required: true
          x-yc-apigateway-integration:
            type: "http"
            url: "http://${local.piq_nlb_address}:8080/teams/{teamId}/assessments"
            headers:
              Host: "piq-service"
              Content-Type: application/json
              Authorization: $request.header.Authorization
  EOT
}
