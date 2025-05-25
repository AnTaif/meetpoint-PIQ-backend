# YC
variable "yc_token" { default = "" }
variable "cloud_id" { default = "" }
variable "folder_id" { default = "" }

# Сеть
variable "default_zone" { default = "" }
variable "subnet_cidr" { default = "" }

# БД
variable "db_user" { default = "" }
variable "db_password" { default = "" }
variable "db_port" { default = "" }

# Балансировщик
variable "balancer_port" { default = "" }

# Bucket
variable "bucket_access_key" { default = "" }
variable "bucket_secret_token" { default = "" }

# JWT
variable "jwt_secret" { default = "" }

# Images
variable "account_service_image" { default = "" }
variable "piq_service_image" { default = "" }
