variable "service_account_name" {
  default = "piq-sa"
}

resource "yandex_iam_service_account" "piq-sa" {
  name        = var.service_account_name
  description = "sa for PIQ-service"
}

resource "yandex_resourcemanager_folder_iam_member" "piq_roles" {
  for_each = toset([
    "container-registry.images.puller", # Для скачивания образов контейнеров
    "mdb.viewer",                       # Для подключения к кластеру MySQL (без администрирования)
    "vpc.publicAdmin",                  # Для NAT и публичного доступа ВМ
    "compute.editor",                   # Для создания/управления ВМ
    "monitoring.editor",                # Для мониторинга (дашборды и метрики)
    "storage.uploader",                 # Для загрузки данных в Object Storage
    "api-gateway.editor",               # Для управления API Gateway
    "editor",
  ])
  folder_id = var.folder_id
  role      = each.key
  member    = "serviceAccount:${yandex_iam_service_account.piq-sa.id}"
}

resource "yandex_iam_service_account_static_access_key" "piq-sa-keys" {
  service_account_id = yandex_iam_service_account.piq-sa.id
  description        = "terraform config"
}
