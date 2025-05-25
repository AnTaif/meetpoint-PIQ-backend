resource "yandex_monitoring_dashboard" "piq_dashboard" {
  name        = "piq_dashboard"
  title       = "PIQ Monitoring"
  description = "Дашборд для мониторинга микросервисов и базы данных"

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

  widgets {
    chart {
      chart_id = "chart3id"
      title = "Ошибки на Gateway"
      queries {
        target {
        query = <<-EOT
          api_gateway.errors_count_per_second{
            service="serverless-apigateway"
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
      y = 20
      w = 10
      h = 10
    }
  }

  # Мониторинг RPS для account-service
  widgets {
    chart {
      chart_id = "chart4id"
      title = "RPS: Account Service"
      queries {
        target {
          query = <<-EOT
            app_requests_per_second{
              service="account-service"
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
      y = 30
      w = 10
      h = 10
    }
  }

  # Мониторинг RPS для piq-service
  widgets {
    chart {
      chart_id = "chart5id"
      title = "RPS: PIQ Service"
      queries {
        target {
          query = <<-EOT
            app_requests_per_second{
              service="piq-service"
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
      y = 30
      w = 10
      h = 10
    }
  }

  # График сравнения RPS обоих сервисов
  widgets {
    chart {
      chart_id = "chart6id"
      title = "Сравнение RPS сервисов"
      queries {
        target {
          query = <<-EOT
            app_requests_per_second{
              service="account-service"
            }
          EOT
          text_mode = true
        }
      }
      queries {
        target {
          query = <<-EOT
            app_requests_per_second{
              service="piq-service"
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
      y = 40
      w = 20
      h = 10
    }
  }
}