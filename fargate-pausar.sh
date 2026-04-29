#!/bin/bash
echo "⏸️  Pausando serviços..."
aws ecs update-service --cluster rds-forms-fabrica --service rds-forms-fabrica-api --desired-count 0
aws ecs update-service --cluster rds-forms-fabrica --service rds-forms-fabrica-web --desired-count 0
echo "✅ Pausado."
