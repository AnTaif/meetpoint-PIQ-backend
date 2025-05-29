resource "yandex_iam_service_account" "piq-storage-sa" {
  name        = "piq-storage-sa"
  description = "sa for piq object storage access"
}

resource "yandex_resourcemanager_folder_iam_member" "sa-storage-editor" {
  folder_id = var.folder_id
  role      = "storage.editor"
  member    = "serviceAccount:${yandex_iam_service_account.piq-storage-sa.id}"
}

resource "yandex_iam_service_account_static_access_key" "piq-storage-sa-keys" {
  service_account_id = yandex_iam_service_account.piq-storage-sa.id
  description        = "sa keys for piq object storage"
}

resource "yandex_storage_bucket" "piq-avatars" {
  bucket = "piq-avatars"
  #acl    = "public-read"

  access_key = yandex_iam_service_account_static_access_key.piq-storage-sa-keys.access_key
  secret_key = yandex_iam_service_account_static_access_key.piq-storage-sa-keys.secret_key
}
