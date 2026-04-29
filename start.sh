#!/bin/bash

echo "🔨 Building solution..."
cd /c/GitHub/RDS.Forms.Fabrica
dotnet build --no-restore 2>&1 | grep -E "error|succeeded|failed"

echo "🚀 Starting API..."
cd /c/GitHub/RDS.Forms.Fabrica/src/RDS.Forms.Fabrica.API
dotnet run &

echo "⚛️ Starting Frontend..."
cd /c/GitHub/RDS.Forms.Fabrica/rds-forms-fabrica-web
npm run dev &

echo "✅ Done! API: http://localhost:5186 | Frontend: http://localhost:5173"
wait
