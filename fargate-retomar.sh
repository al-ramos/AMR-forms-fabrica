#!/bin/bash
echo "▶️  Retomando serviços..."
aws ecs update-service --cluster rds-forms-fabrica --service rds-forms-fabrica-api --desired-count 1
aws ecs update-service --cluster rds-forms-fabrica --service rds-forms-fabrica-web --desired-count 1
echo "✅ Retomado. Aguarde ~60s e verifique o IP público."
