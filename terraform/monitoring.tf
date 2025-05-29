resource "yandex_monitoring_dashboard" "piq_mysql_dashboard" {
  name        = "piq_dashboard"
  title       = "PIQ MySQL Monitoring"
  description = "Дашборд для мониторинга БД"

  widgets {
    chart {
      chart_id = "chart1id"
      title = "Оперативка на Кластер"
      queries {
        target {
        query = <<-EOT
          mem.used_bytes{
            service="managed-mysql",
            host="${yandex_mdb_mysql_cluster.db.host[0].fqdn}"
          }
        EOT
          text_mode = true
        }
      }
      visualization_settings {
        type        = "VISUALIZATION_TYPE_LINE"
        aggregation = "SERIES_AGGREGATION_AVG"
        show_labels = true
      }
    }
    position {
      x = 0
      y = 0
      w = 10
      h = 10
    }
  }

  widgets {
    chart {
      chart_id = "chart2id"
      title = "Диск на кластер"
      queries {
        target {
        query = <<-EOT
          disk.used_bytes{
            service="managed-mysql",
            host="${yandex_mdb_mysql_cluster.db.host[0].fqdn}"
          }
        EOT
          text_mode = true
        }
      }
      visualization_settings {
        type        = "VISUALIZATION_TYPE_LINE"
        aggregation = "SERIES_AGGREGATION_AVG"
        show_labels = true
      }
    }
    position {
      x = 10
      y = 0
      w = 10
      h = 10
    }
  }
}

resource "yandex_monitoring_dashboard" "api_gateway_dashboard" {
  name        = "api_gateway_dashboard"
  title       = "API Gateway Monitoring"
  description = "Дашборд для мониторинга API Gateway"

  widgets {
    chart {
      chart_id = "api_gw_requests"
      title = "Запросы к API Gateway"
      queries {
        target {
          query = <<-EOT
            api_gateway.requests_count_per_second{
              folderId="${yandex_api_gateway.api_gateway.folder_id}",
              service="serverless-apigateway",
              gateway="${yandex_api_gateway.api_gateway.id}",
              path="total",
              operation="total",
              release!="total"
            }
          EOT
          text_mode = true
        }
      }
      visualization_settings {
        type        = "VISUALIZATION_TYPE_LINE"
        aggregation = "SERIES_AGGREGATION_SUM"
        show_labels = true
      }
    }
    position {
      x = 0
      y = 0
      w = 12
      h = 10
    }
  }

  widgets {
    chart {
      chart_id = "api_gw_errors"
      title = "Ошибки API Gateway"
      queries {
        target {
          query = <<-EOT
            api_gateway.errors_count_per_second{
              folderId="${yandex_api_gateway.api_gateway.folder_id}",
              service="serverless-apigateway",
              gateway="${yandex_api_gateway.api_gateway.id}",
              path="total",
              operation="total",
              release!="total"
            }
          EOT
          text_mode = true
        }
      }
      visualization_settings {
        type        = "VISUALIZATION_TYPE_LINE"
        aggregation = "SERIES_AGGREGATION_SUM"
        show_labels = true
      }
    }
    position {
      x = 12
      y = 0
      w = 12
      h = 10
    }
  }

  widgets {
    chart {
      chart_id = "api_gw_latency"
      title = "Задержка API Gateway (мс)"
      queries {
        target {
          query = <<-EOT
            api_gateway.request_latency_milliseconds{
              folderId="${yandex_api_gateway.api_gateway.folder_id}",
              service="serverless-apigateway",
              gateway="${yandex_api_gateway.api_gateway.id}",
              path="total",
              operation="total",
              release!="total"
            }
          EOT
          text_mode = true
        }
      }
      visualization_settings {
        type        = "VISUALIZATION_TYPE_LINE"
        aggregation = "SERIES_AGGREGATION_AVG"
        show_labels = true
      }
    }
    position {
      x = 0
      y = 10
      w = 12
      h = 10
    }
  }

  widgets {
    chart {
      chart_id = "api_gw_5xx"
      title = "5xx Ошибки API Gateway"
      queries {
        target {
          query = <<-EOT
            api_gateway.http_5xx_count_per_second{
              folderId="${yandex_api_gateway.api_gateway.folder_id}",
              service="serverless-apigateway",
              gateway="${yandex_api_gateway.api_gateway.id}",
              path="total",
              operation="total",
              release!="total"
            }
          EOT
          text_mode = true
        }
      }
      visualization_settings {
        type        = "VISUALIZATION_TYPE_LINE"
        aggregation = "SERIES_AGGREGATION_SUM"
        show_labels = true
      }
    }
    position {
      x = 12
      y = 10
      w = 12
      h = 10
    }
  }

  widgets {
    chart {
      chart_id = "api_gw_4xx"
      title = "4xx Ошибки API Gateway"
      queries {
        target {
          query = <<-EOT
            api_gateway.http_4xx_count_per_second{
              folderId="${yandex_api_gateway.api_gateway.folder_id}",
              service="serverless-apigateway",
              gateway="${yandex_api_gateway.api_gateway.id}",
              path="total",
              operation="total",
              release!="total"
            }
          EOT
          text_mode = true
        }
      }
      visualization_settings {
        type        = "VISUALIZATION_TYPE_LINE"
        aggregation = "SERIES_AGGREGATION_SUM"
        show_labels = true
      }
    }
    position {
      x = 0
      y = 20
      w = 12
      h = 10
    }
  }
}
