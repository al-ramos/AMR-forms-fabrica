#!/usr/bin/env bash
INTERVAL=60
BINANCE_URL="https://api.binance.com/api/v3/ticker/24hr"
NASDAQ_URL="https://query1.finance.yahoo.com/v7/finance/quote?symbols=NQ%3DF&fields=regularMarketPrice,regularMarketPreviousClose,regularMarketDayHigh,regularMarketDayLow,regularMarketChangePercent,preMarketPrice,preMarketChangePercent,postMarketPrice,postMarketChangePercent,marketState"

for cmd in curl jq; do
  if ! command -v "$cmd" &>/dev/null; then
    echo "Dependencia nao encontrada: $cmd"
    exit 1
  fi
done

clear
echo "=================================================="
echo "   MULTI TICKER -- Binance + Yahoo Finance"
echo "   Atualiza a cada ${INTERVAL}s | Ctrl+C sair"
echo "=================================================="
echo ""
echo ""
echo ""
echo ""
echo ""
echo ""
echo ""
echo ""

fetch_crypto() {
  local SYMBOL=$1
  local RESPONSE=$(curl -s --max-time 10 "${BINANCE_URL}?symbol=${SYMBOL}")
  if [[ -z "$RESPONSE" ]]; then printf "%-8s | ERRO" "$SYMBOL"; return; fi

  local PRICE=$(echo  "$RESPONSE" | jq -r '.lastPrice // "0"')
  local CHANGE=$(echo "$RESPONSE" | jq -r '.priceChangePercent // "0"')
  local HIGH=$(echo   "$RESPONSE" | jq -r '.highPrice // "0"')
  local LOW=$(echo    "$RESPONSE" | jq -r '.lowPrice  // "0"')

  local CHANGE_ROUNDED=$(printf "%.2f" "${CHANGE:-0}")
  local IS_POSITIVE=$(awk "BEGIN { print ($CHANGE_ROUNDED >= 0) ? 1 : 0 }")
  local SINAL=""; [[ "$IS_POSITIVE" == "1" ]] && SINAL="+"

  printf "%-8s | $%-10s | 24h:%s%s%% | H:\$%-10s | L:\$%s" \
    "$SYMBOL" \
    "$(printf "%.2f" "${PRICE:-0}")" \
    "$SINAL" "$CHANGE_ROUNDED" \
    "$(printf "%.2f" "${HIGH:-0}")" \
    "$(printf "%.2f" "${LOW:-0}")"
}

fetch_nasdaq() {
  local RESPONSE=$(curl -s --max-time 10 \
    -H "User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/124.0.0.0 Safari/537.36" \
    -H "Accept: application/json" \
    -H "Accept-Language: pt-BR,pt;q=0.9,en-US;q=0.8" \
    -H "Referer: https://finance.yahoo.com/" \
    "$NASDAQ_URL")

  local STATUS=$(echo     "$RESPONSE" | jq -r '.quoteResponse.result[0].marketState // "N/A"')
  local PREV=$(echo       "$RESPONSE" | jq -r '.quoteResponse.result[0].regularMarketPreviousClose // "0"')
  local REG_PRICE=$(echo  "$RESPONSE" | jq -r '.quoteResponse.result[0].regularMarketPrice // "0"')
  local REG_CHG=$(echo    "$RESPONSE" | jq -r '.quoteResponse.result[0].regularMarketChangePercent // "0"')
  local HIGH=$(echo       "$RESPONSE" | jq -r '.quoteResponse.result[0].regularMarketDayHigh // "0"')
  local LOW=$(echo        "$RESPONSE" | jq -r '.quoteResponse.result[0].regularMarketDayLow  // "0"')
  local PRE_PRICE=$(echo  "$RESPONSE" | jq -r '.quoteResponse.result[0].preMarketPrice // ""')
  local PRE_CHG=$(echo    "$RESPONSE" | jq -r '.quoteResponse.result[0].preMarketChangePercent // "0"')
  local POST_PRICE=$(echo "$RESPONSE" | jq -r '.quoteResponse.result[0].postMarketPrice // ""')
  local POST_CHG=$(echo   "$RESPONSE" | jq -r '.quoteResponse.result[0].postMarketChangePercent // "0"')

  local REG_ROUNDED=$(printf "%.2f" "${REG_CHG:-0}")
  local REG_SINAL=""; awk "BEGIN { exit ($REG_ROUNDED >= 0) ? 0 : 1 }" && REG_SINAL="+"

  if [[ "$REG_PRICE" == "0" ]]; then
    printf "%-8s | Aguardando Yahoo Finance..." "NQ=F"
  else
    printf "%-8s | Fech:\$%-8s | $%-10s | 24h:%s%s%% | H:\$%-8s | L:\$%-8s | [%s]" \
      "NQ=F" \
      "$(printf "%.2f" "$PREV")" \
      "$(printf "%.2f" "$REG_PRICE")" \
      "$REG_SINAL" "$REG_ROUNDED" \
      "$(printf "%.2f" "$HIGH")" \
      "$(printf "%.2f" "$LOW")" \
      "$STATUS"
  fi

  tput cup $((ROW+5)) 0; tput el
  if [[ -n "$PRE_PRICE" && "$PRE_PRICE" != "null" && "$PRE_PRICE" != "0" ]]; then
    local PRE_ROUNDED=$(printf "%.2f" "${PRE_CHG:-0}")
    local PRE_SINAL=""; awk "BEGIN { exit ($PRE_ROUNDED >= 0) ? 0 : 1 }" && PRE_SINAL="+"
    printf "         | Pre-Mercado: \$%-10s | Var: %s%s%%" \
      "$(printf "%.2f" "$PRE_PRICE")" "$PRE_SINAL" "$PRE_ROUNDED"
  elif [[ -n "$POST_PRICE" && "$POST_PRICE" != "null" && "$POST_PRICE" != "0" ]]; then
    local POST_ROUNDED=$(printf "%.2f" "${POST_CHG:-0}")
    local POST_SINAL=""; awk "BEGIN { exit ($POST_ROUNDED >= 0) ? 0 : 1 }" && POST_SINAL="+"
    printf "         | Pos-Mercado: \$%-10s | Var: %s%s%%" \
      "$(printf "%.2f" "$POST_PRICE")" "$POST_SINAL" "$POST_ROUNDED"
  else
    printf "         | Pre/Pos-Mercado: fora do horario estendido"
  fi
}

update_all() {
  local UPDATED=$(date "+%d/%m/%Y %H:%M:%S")
  tput cup $ROW 0;        tput el; fetch_crypto "BTCUSDT"
  tput cup $((ROW+1)) 0; tput el; fetch_crypto "ETHUSDT"
  tput cup $((ROW+2)) 0; tput el; fetch_crypto "SOLUSDT"
  tput cup $((ROW+3)) 0; tput el; fetch_crypto "SUIUSDT"
  tput cup $((ROW+4)) 0; tput el; fetch_nasdaq
  tput cup $((ROW+7)) 0; tput el
  printf "Atualizado: %s  |  Proxima: %ss  |  ENTER p/ atualizar" "$UPDATED" "$INTERVAL"
}

ROW=3
update_all

while true; do
  read -t "$INTERVAL" -s -n 1 INPUT
  update_all
done
