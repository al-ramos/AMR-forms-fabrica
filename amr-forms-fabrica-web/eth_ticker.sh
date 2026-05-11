#!/usr/bin/env bash
SYMBOL="ETHUSDT"
INTERVAL=5
API_URL="https://api.binance.com/api/v3/ticker/24hr?symbol=${SYMBOL}"

for cmd in curl jq; do
  if ! command -v "$cmd" &>/dev/null; then
    echo "Dependencia nao encontrada: $cmd"
    exit 1
  fi
done

clear
echo "=================================="
echo "   ETH TICKER -- Binance"
echo "   Atualiza a cada ${INTERVAL}s | Ctrl+C sair"
echo "=================================="
echo ""

while true; do
  RESPONSE=$(curl -s --max-time 10 "$API_URL")

  if [[ -z "$RESPONSE" ]]; then
    echo "Falha na requisicao. Tentando em ${INTERVAL}s..."
    sleep "$INTERVAL"
    continue
  fi

  PRICE=$(echo "$RESPONSE"  | jq -r ".lastPrice")
  CHANGE=$(echo "$RESPONSE" | jq -r ".priceChangePercent")
  HIGH=$(echo "$RESPONSE"   | jq -r ".highPrice")
  LOW=$(echo "$RESPONSE"    | jq -r ".lowPrice")
  VOLUME=$(echo "$RESPONSE" | jq -r ".volume")

  CHANGE_ROUNDED=$(printf "%.2f" "$CHANGE")
  IS_POSITIVE=$(awk "BEGIN { print ($CHANGE >= 0) ? 1 : 0 }")

  if [[ "$IS_POSITIVE" == "1" ]]; then
    SINAL="+"
  else
    SINAL=""
  fi

  UPDATED=$(date "+%d/%m/%Y %H:%M:%S")

  printf "\rETH/USDT: \$%-10s | 24h: %s%s%% | Max: \$%s | Min: \$%s | Vol: %s | %s" \
    "$(printf "%.2f" "$PRICE")" \
    "$SINAL" "$CHANGE_ROUNDED" \
    "$(printf "%.2f" "$HIGH")" \
    "$(printf "%.2f" "$LOW")" \
    "$(printf "%.0f" "$VOLUME")" \
    "$UPDATED"

  sleep "$INTERVAL"
done
